namespace FoodFile

open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System

[<ApiController>]
[<Route("api/[controller]")>]
type TraceController () =
    inherit ControllerBase()


    // Wir müssen wissen ob UNSER frontend das ding aufruft oder eine andere instanz. Weil für UNSER frontend machen wir auch eine Remote Search,
    // Für eine andere instanz aber eher nich so...
    [<HttpGet("{id}")>] // Jetzt wo hier string als return steht müssen wir ContentType JSon evtl manuell setzen.
    member __.Get(id:string) : string =
        Search.SearchLocal id
        |> JsonConvert.SerializeObject
        // ToDo: Error Handling.
        // Note: Die Funktion kann evtl weg weil wir immer Atome aber nicht ganze Docs transferieren

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

