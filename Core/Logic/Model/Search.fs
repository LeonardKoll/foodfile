namespace FoodFile

open Elastic

(*
    The Serch Module coordinates all entity-searches
    accross the local Elastic-Instance as well as
    accross the FoodFile Network.
    It relies on the Elastic-Module for the local search
    and on the Network-Module for distributed search
*)
module Search = 
    
    // Collect all Atoms for this entity and entities consumed throughout the production process (recursively)
    // Tail-Recursive
    // TODO Aktuell nur Lokal...
    let rec private RecursiveSearch (result:Atom list) (entityIDs:string list) =
        if entityIDs.Length=0 then result else 
        entityIDs.Head
        |> GetEntity
        |> function
            | None -> RecursiveSearch result entityIDs.Tail // No IDs left to search for. Return what we have so far / continue with other entities
            | Some entity -> 
                entity.Atoms
                |> List.fold (fun (state:TransformationInfo option) atom -> // Find and return the Creation Atom
                    match state with
                    | Some ti -> state
                    | None -> 
                        match atom.Data with
                        | Transformation ti -> Some(ti)
                        | _ -> None // This Atom is not a Transformation atom and hence not a Creation.
                    ) None
                |> function
                    | None -> RecursiveSearch (result @ entity.Atoms) entityIDs.Tail // There is no Creation for this Atom. Thats appearently incomplete data - return what we have so far / continue with other entities
                    | Some ti -> RecursiveSearch (result @ entity.Atoms) (entityIDs.Tail @ ti.InEntities) // Add the in-entities of the creation to the search. List might be empty but that's fine.


    let Search = fun entityID -> RecursiveSearch [] [entityID]