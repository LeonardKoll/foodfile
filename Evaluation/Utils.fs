module Utils

open Microsoft.Extensions.Configuration
open FoodFile
open ScenarioLibrary


let getElasticService host entities members =
    ConfigurationBuilder()
        .AddInMemoryCollection(
            Map.empty
                .Add("elastic", host)
                .Add("entityindex", entities)
                .Add("memberindex", members)
        )
        .Build()
    |> ElasticService :> IElasticService

let assembleEntities (atoms: Atom List) =
    atoms
    |> List.groupBy (fun atom -> atom.EntityID)
    |> List.map (fun (entityID, entityAtoms) -> 
            {ID=entityID; Atoms=entityAtoms}
        )
    
let deployScenarioToMember (fffId:string) (ssId:string) (yjId:string) (es:IElasticService) (atomFuns:(string -> string -> string -> Atom) List) =
    atomFuns
    |> List.map (fun atomFun -> atomFun fffId ssId yjId)
    |> assembleEntities
    |> List.iter (fun entity -> es.WriteEntity entity)