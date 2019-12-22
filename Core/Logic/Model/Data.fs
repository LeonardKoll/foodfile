namespace FoodFile

type Party = {
    Id: string;
}

type Location = {
    Name: string; // Später: Koordinaten, Identifier oder Adresse?
    Coordinates: string
}

type TransformationInfo = {
    InEntities: string array;
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

type MetaContent = {
    Timestamp: int64
}

type AtomContent = 
    | Transformation of TransformationInfo
    | Tracking of TrackingInfo
    | Registration of BuyingInfo
    | EntityDescription of EntityDescription

type Atom = {
    ShortID: string; // Atom ID. Wird im DB-Dokument-Objekt zusätzlich als Key verwendet.
    EntityID: string;
    Version: int;
    Owners: Party array;
    Data : AtomContent;
    Meta: MetaContent option;
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