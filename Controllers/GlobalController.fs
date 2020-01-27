namespace FoodFile

open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json
open Members


type ReturnInfo = {
    Entities:Entity list;
    Members:Member list;
}

[<ApiController>]
[<Route("api/entities/[controller]")>]
type GlobalController () =
    inherit ControllerBase()

    [<HttpGet("downchain/{id}")>] // Jetzt wo hier string als return steht müssen wir ContentType JSon evtl manuell setzen.
    member __.GetDownchain (id:string) : string = // Member ID optional
        
        let entities =
            (function
            | [|entityID|] -> Entities.CompleteSearch Downchain None [entityID]
            | [|memberID; entityID|] -> Entities.CompleteSearch Downchain (Some memberID) [entityID]
            | _ -> []) (id.Split('-'))
        
        let members = ExtractMembers entities
        {Entities=entities; Members=members}
        |> JsonConvert.SerializeObject
        // ToDo: Error Handling.
        // Note: Die Funktion kann evtl weg weil wir immer Atome aber nicht ganze Docs transferieren

    [<HttpGet("upchain/{id}")>] // Jetzt wo hier string als return steht müssen wir ContentType JSon evtl manuell setzen.
    member __.GetUpchain (id:string) : string = // Member ID optional  
        
        let entities =
            (function
            | [|entityID|] -> Entities.CompleteSearch Upchain None [entityID]
            | [|memberID; entityID|] -> Entities.CompleteSearch Upchain (Some memberID) [entityID]
            | _ -> []) (id.Split('-'))

        let members = ExtractMembers entities
        {Entities=entities; Members=members}
        |> JsonConvert.SerializeObject
        // ToDo: Error Handling.
        // Note: Die Funktion kann evtl weg weil wir immer Atome aber nicht ganze Docs transferieren