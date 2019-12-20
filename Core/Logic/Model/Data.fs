namespace FoodFile

type Party = {
    Id: string;
}

type Location = {
    Name: string; // Später: Koordinaten, Identifier oder Adresse?
}

type TransformationInfo = {
    OutEntities: string array
}

type TrackingInfo = 
    | Owner of Party
    | Location of Location

type MetaInfo = {
    Timestamp: int64
}

type AtomInfo = 
    | Transformation of TransformationInfo
    | Tracking of TrackingInfo

type Atom = {
    ShortID: string; // Atom ID. Wird im DB-Dokument-Objekt zusätzlich als Key verwendet.
    EntityID: string;
    Version: int;
    Owners: Party array;
    Data : AtomInfo;
    Meta: MetaInfo;
} with
    
    member this.LongID =
        this.EntityID + "-" + this.ShortID + "-" + this.Version.ToString();


type Entity = {
    Atoms: Atom list;
} with
    
    // Entity ID is none if Atoms empty or if Atom-Entities not consistent.
    member this.ID =
        if List.isEmpty this.Atoms then None 
        else List.fold (fun result (atom:Atom) -> 
            match result with
            | Some(x) -> if x=atom.EntityID then result else None
            | None -> None
            | _ -> None) (Some this.Atoms.Head.EntityID) this.Atoms


module Data = 
    
    let AddAtomToEntity = fun (atom:Atom) (entity:Entity) ->
        let atoms = entity.Atoms @ [atom]
        {Atoms=atoms}

    // Kann man jetzt transform und tracking gleichzeitig ablegen?
    let testatom =
        {ShortID = "A1"; EntityID="E1"; Version=1; Owners = [|{Id = "Leonard"}|]; Data = Transformation ({OutEntities = [|"123"; "26"|]}); Meta = {Timestamp = 29852389472L}}

    let basisEntity =
        {Atoms = []}

    let TestEntity =
        AddAtomToEntity testatom basisEntity