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

    [<HttpGet("{id}")>]
    member __.Get(id:string) : Atom list =
        Search.Search id
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

