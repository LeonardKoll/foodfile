namespace FoodFile

open Newtonsoft.Json.Linq
open Newtonsoft.Json
open System



type Member = {
    ID: string; // 6 Zeichen
    Name: string option;
    API: string option;
    // Future: Signature Key, Expires (when we need to discard memory cahce)
}

type Location = {
    Name: string option;
    Coordinates: string
}

type EntityCreation = {
    InEntities: string list;
    Responsible: string option; // IDs
    Location: Location option;
    Timestamp: int64;
}

type EntityDescription = {
    Name: string
    Certificates: string list // Später: Liste signierter Zertifikate
}

type EntityTransfer = {
    TrackPoints: (Location * int64) list
    Responsible: string option;
}

type DestructionReason = Consumed | Lost | Wasted

type EntityDestruction = {
    Reason: DestructionReason option
    Location: Location option
    Responsible: string option
}

type ChainDirection = 
    | Downchain
    | Upchain
    | Bidirectional
    | Unspecified

type EntityInvolvement = {
    Member: string;
    Direction: ChainDirection;
}

type AtomInformation = 
    | Creation of EntityCreation
    | Destruction of EntityDestruction
    | Transfer of EntityTransfer
    | Description of EntityDescription
    | Involvement of EntityInvolvement

type Signature = {
    Signer:string; // Member ID
    Value:string;
    Timestamp: int64;
}

type Atom = {
    AtomID: string;             // 4 Zeichen
    EntityID: string;           // 10 Zeichen
    Version: int;
    Information: AtomInformation;
    // A signature covers everything above. A new signature does not change the Atom version; every other change does.
    Signatures: Signature list;
}with

    member this.CompleteID =
        this.EntityID + "-" + this.AtomID + "-" + this.Version.ToString();
    
    [<JsonIgnore>]
    member this.Merge = fun (other:Atom) ->
        if this.CompleteID=other.CompleteID
        then Some( {this with Signatures = List.distinct (this.Signatures@other.Signatures) } )
        else None

    [<JsonIgnore>]
    member this.MergeInto = fun (basis:Atom list) ->
        basis
        |> List.fold (fun (state:(bool * Atom list)) cAtom ->
            match state with
            | (true, result) -> (true, cAtom::result)
            | (false, result) -> 
                this.Merge cAtom
                |> function
                    | None -> (false, cAtom::result)
                    | Some mergedAtom -> (true, mergedAtom::result)
            ) (false,[]) // State: (isMerged, resultlist)
        |> function
            | (true, result) -> result          // Entity existed already, atoms merged :)
            | (false, result) -> this::result   // Entity did not exist in list. Add now.

type Entity = {
    Atoms: Atom list;
    ID:string;
} with
    
    [<JsonIgnore>]
    member this.VerifiedID =
        if this.Atoms.IsEmpty then None
        elif List.forall (fun (atom:Atom) -> atom.EntityID=this.Atoms.Head.EntityID) this.Atoms
        then Some(this.Atoms.Head.EntityID)
        else None
    
    [<JsonIgnore>]
    member this.InvolvedEntities =
        this.Atoms
        |> List.fold (fun (state:string list) atom ->
            match atom.Information with
            | Creation x -> state@x.InEntities 
            | _ -> state
            ) []

    [<JsonIgnore>]
    member this.InvolvedMembers =
        this.Atoms
        |> List.fold (fun (state:string list) atom ->
            
            let signers =
                List.map (fun signature -> signature.Signer) atom.Signatures

            match atom.Information with
            | Involvement x ->  x.Member::state
            | Creation x ->     match x.Responsible with Some id -> id::state | None -> state
            | Destruction x ->  match x.Responsible with Some id -> id::state | None -> state
            | Transfer x ->     match x.Responsible with Some id -> id::state | None -> state
            | _ -> state
            |> List.append signers
            |> List.distinct

            ) []

    [<JsonIgnore>]
    member this.Verify =
        match this.VerifiedID with
        | None -> false
        | Some(id) -> if id=this.ID then true else false

    [<JsonIgnore>]
    member this.Merge = fun (other:Entity) ->
        if this.ID=other.ID
        then other.Atoms
            |> List.fold ( fun (state:Atom list) (cAtom:Atom) ->
                cAtom.MergeInto state) this.Atoms
            |> fun mergedAtoms -> Some({ Atoms=mergedAtoms ; ID=this.ID })
        else None
        
    
    [<JsonIgnore>]
    member this.MergeInto = fun (basis:Entity list) ->
        basis
        |> List.fold (fun (state:(bool * Entity list)) cEntity ->
            match state with
            | (true, result) -> (true, cEntity::result)
            | (false, result) -> 
                this.Merge cEntity
                |> function
                    | None -> (false, cEntity::result)
                    | Some mergedEntity -> (true, mergedEntity::result)
            ) (false,[]) // State: (isMerged, resultlist)
        |> function
            | (true, result) -> result          // Entity existed already, atoms merged :)
            | (false, result) -> this::result   // Entity did not exist in list. Add now.

module Types =

    [<Literal>]
    let EntityIDLength = 10

    [<Literal>]
    let AtomIDLength = 4

    let private newID = fun (length:int) ->
        let r = Random()
        let chars = Array.concat([[|'A' .. 'Z'|];[|'0' .. '9'|]])
        let sz = Array.length chars in
        String(Array.init length (fun _ -> chars.[r.Next sz]))

    let newEntityID = newID EntityIDLength
    let newAtomID = newID AtomIDLength