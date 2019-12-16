module Data

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
    ID: string; // Atom ID. Wird im DB-Dokument-Objekt zusätzlich als Key verwendet.
    Owners: Party array;
    Data : AtomInfo;
    Meta: MetaInfo;
}

type Entity = {
    ID: string; // Entity ID
    Atoms: Atom list;
}

let AddAtomToEntity = fun (atom:Atom) (entity:Entity) ->
    let atoms = entity.Atoms @ [atom]
    {ID=entity.ID; Atoms=atoms}

// Kann man jetzt transform und tracking gleichzeitig ablegen?
let testatom =
    {ID = "007"; Owners = [|{Id = "Leonard"}|]; Data = Transformation ({OutEntities = [|"123"; "26"|]}); Meta = {Timestamp = 29852389472L}}

let basisEntity =
    {ID="QuickTest"; Atoms = []}

let TestEntity =
    AddAtomToEntity testatom basisEntity