namespace FoodFile

open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System
open Members


type ReturnInfo = {
    Entities:Entity list;
    Members:Member list;
}

[<ApiController>]
[<Route("api/entities/[controller]")>]
type GlobalController () =
    inherit ControllerBase()

    [<HttpGet("{id}")>] // Jetzt wo hier string als return steht müssen wir ContentType JSon evtl manuell setzen.
    member __.Get (id:string) : string = // Marticipant ID optional  
        let entities = Entities.CompleteSearch None [id]
        let members = ExtractMembers entities

        {Entities=entities; Members=members}
        |> JsonConvert.SerializeObject
        // ToDo: Error Handling.
        // Note: Die Funktion kann evtl weg weil wir immer Atome aber nicht ganze Docs transferieren