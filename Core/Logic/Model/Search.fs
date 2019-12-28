namespace FoodFile

open Elastic
open FSharp.Data
open Newtonsoft.Json.Linq
open System

(*
    The Serch Module coordinates all entity-searches
    accross the local Elastic-Instance as well as
    accross the FoodFile Network.
    It relies on the Elastic-Module for the local search
    and on the Network-Module for distributed search
*)

// Aktuell folgen wir in der Suche ausschließlich den Transforms. Für die lokale suche ist das okay weil wir ja zuverlässig alle Atome zu einer Entity gelifert bekommen die lokal da sind.
// Für Remote reicht das aber nicht weil wir uns nicht darauf verlassen können von Producer alles zu bekommen was es zu wissen gibt.
// Hier müssen wir daher nicht nur den Transforms folgen sondern auch den Trace spuren - aber immerhin nur upchain, es ist ja schließlich der Producer.
// Wir können aber davon ausgehen, dass uns ein angefragter partner alles liefert was er zum atom weiß - unsere rekursion bezeiht sich daher nur auf das nachverfolgen der Spuren.
// Defakto leifert uns der Partner die ergebnisse seiner Local search: Wir sprechen nämlich einfach dessen TraceController an.

module Search =  
    
    let private ExtractCreation = fun (entity:Entity) ->
        entity.Atoms
        |> List.fold (fun (state:EntityCreation option) atom -> // Find and return the Creation Atom
            match state with
            | Some (_) -> state
            | None -> 
                match atom.Data with
                | Creation ec -> Some(ec)
                | _ -> None // This Atom is not a Transformation atom and hence not a Creation.
            ) None
    
    // Tail-Recursive
    let rec private MergeEntities = fun (entitiesA:Entity list) (entitiesB:Entity list) ->
        match (entitiesA.IsEmpty, entitiesB.IsEmpty) with
        | (true,true) -> []
        | (true,false) -> entitiesB
        | (false,true) -> entitiesA
        | _ -> entitiesA
            |> List.fold (fun (state:(Entity option * Entity list)) (currentElement:Entity) -> 
                match (state, currentElement) with
                | ((None, result), e) -> (None, e::result) // Entity already merged
                | ((Some(e1), result), e2) -> // Entity not merged yet
                    match (e1.CompleteID = e2.CompleteID, result) with
                    | (true,result)  ->  // Merge
                        let mergedEntity = {
                            Atoms= List.distinctBy (fun (atom:Atom) -> atom.CompleteAtomID) (e1.Atoms@e2.Atoms) ; 
                            CompleteID=e1.CompleteID }
                        (None, mergedEntity::result)
                    | (false,result) -> (Some(e1), e2::result) // IDs do not equal, do not merge
                ) (Some(entitiesB.Head),[])
            |> function
                | (Some(entity), result) -> MergeEntities (entity::result) (entitiesB.Tail) // Entity with that ID was not present in A, so append now.
                | (None, result) -> MergeEntities result (entitiesB.Tail) // Merge was successful

    let GetEntityRemote = fun (producerID:string) (entityID:string) ->
        GetParticipantLocal producerID
        |> function
            | Some(producer) ->
            match producer.API with
                | Some(url) -> 
                    url + entityID 
                    |> (Http.RequestString >> JObject.Parse)
                    |> fun parsed -> 
                        parsed.ToObject<Entity list>()
                        |> List.filter( fun (elem:Entity) -> if (Types.VerifyEntity elem)=None then false else true)
                    // auf den eingegangenen Atomen sollten wir dann eventuell nochmal unsere Lokale Suche ausführen um eventuell unnötig erhaltene Atome Entities) rauszufiltern. (?)
                    // Wir machen nämlich sonst nochmal ein branching auf den erhaltenen entites und auf die art und weise kann man das ding defakto abschiessen.
                | None -> raise (System.ArgumentException ("Entity producer does not have a lookup API."))
            | None -> raise (System.ArgumentException ("Entity producer unknown."))

    // Collect all Atoms for this entity and entities consumed throughout the production process (recursively)
    // Tail-Recursive
    let rec private RecSearchLocal (result:Entity list) (entityIDs:string list) =
        if entityIDs.Length=0 then result else 
        entityIDs.Head
        |> GetEntityLocal
        |> function
            | None -> RecSearchLocal result entityIDs.Tail // No IDs left to search for. Return what we have so far / continue with other entities
            | Some entity ->
                ExtractCreation entity
                |> function
                    | None -> RecSearchLocal (entity::result) entityIDs.Tail // There is no Creation for this Atom. Thats appearently incomplete data - return what we have so far / continue with other entities
                    | Some ec -> RecSearchLocal (entity::result) (entityIDs.Tail @ ec.InEntities) // Add the in-entities of the creation to the search. List might be empty but that's fine.
    
    // Tail-Recursive
    let rec private RecSearchRemote (result:Entity list) (entityIDs:string list) =
        if entityIDs.Length=0 then result else 
        entityIDs.Head.Split [|'-'|]
        |> function
            | [|producerID; entityID|] -> 
                let entities = GetEntityRemote producerID entityID
                // Find the transformation Info Atoms of all Entities and Extract the In-Entity IDs
                entities
                |> List.fold (fun (state:string list) entity ->
                    ExtractCreation entity
                    |> function
                    | Some (ec) -> state @ ec.InEntities
                    | None -> state
                    ) []
                |> fun inEntityIDs -> RecSearchRemote (MergeEntities entities result) (entityIDs @ inEntityIDs) // Trigger next search
            | _ -> RecSearchRemote result entityIDs.Tail // Skip this ID (its invalid) and continue

    let SearchRemote = fun entityID -> RecSearchRemote [] [entityID]
    
    let SearchLocal = fun entityID -> RecSearchLocal [] [entityID]

    let Search = fun entityID ->
        let localEntities = SearchLocal entityID
        localEntities
        |> List.map (fun (entity:Entity) -> entity.CompleteID)
        |> RecSearchRemote []
        |> MergeEntities localEntities