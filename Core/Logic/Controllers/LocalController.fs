namespace FoodFile

open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System

[<ApiController>]
[<Route("api/[controller]")>]
type LocalController () =
    inherit ControllerBase()

    [<HttpGet("{id}")>] // Jetzt wo hier string als return steht müssen wir ContentType JSon evtl manuell setzen.
    member __.Get([<FromQuery>] id:string array) : string = //Multiple
        Search.LocalSearch (Array.toList id)
        |> JsonConvert.SerializeObject
        // ToDo: Error Handling.

    [<HttpPost>]
    member __.Create([<FromBody>] body:Object ) =
        body
        |> Convert.ToString
        |> JObject.Parse
        |> fun parseResult -> parseResult.ToObject<Entity>()
        |> Elastic.WriteEntity 
        // ToDo: Nicht so nice hier ist, dass es nicht schon in der Signatur zu JToken oder JObject geparst wird.
        // ToDO: Error Handling.
        // Note: Die Funktion kann evtl weg weil wir immer Atome aber nicht ganze Docs transferieren

