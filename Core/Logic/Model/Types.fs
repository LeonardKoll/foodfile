namespace FoodFile

open Newtonsoft.Json.Linq

type Participant = {
    ID: string; // 6 Zeichen
    Name: string option;
    API: string option;
}

type Location = {
    Name: string; // Später: Koordinaten, Identifier oder Adresse?
    Coordinates: string
}

type EntityCreation = {
    InEntities: string list;
    Location: Location option;
}

type TrackingInfo = {
    Location: Location
    }

type BuyingInfo = {
    Seller : string;
}

type EntityDescription = {
    Name: string
    Certificates: string list option // Später: Liste signierter Zertifikate
}

type AtomContent = 
    | Creation of EntityCreation
    | Location of Location
    | Ownership of Participant
    | EntityDescription of EntityDescription

type Atom = {
    AtomID: string;             // 4 Zeichen
    EntityID: string;           // 10 Zeichen
    ProducerID: string;         // 6 Zeichen - ParticipantID
    Version: int;
    Timestamp: int64
    Data : AtomContent;
} with

    member this.CompleteEntityID =
        this.ProducerID + "-" + this.EntityID

    member this.CompleteAtomID =
        this.CompleteEntityID + "-" + this.AtomID + "-" + this.Version.ToString();

(*
Entities are never shipped (serialized).
An Entity MUST always be created or modified with the Methods below.
*)
type Entity = {
    Atoms: Atom list;
    CompleteID:string;
}

module Types =
    
    // Entity ID is none if Atoms empty or if Atom-Entities not consistent.
    let GetVerifiedEntityID = fun (entity:Entity) ->
        let atoms = entity.Atoms
        if atoms.IsEmpty then None
        elif List.forall (fun (atom:Atom) -> atom.CompleteEntityID=atoms.Head.CompleteEntityID) entity.Atoms
        then Some(atoms.Head.CompleteEntityID)
        else None
    
    let VerifyEntity = fun (entity:Entity) ->
        match GetVerifiedEntityID entity with
        | None -> None
        | _ -> Some(entity)