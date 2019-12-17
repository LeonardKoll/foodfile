namespace FoodFile

open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System

[<ApiController>]
[<Route("api/[controller]")>]
type ElasticController () =
    inherit ControllerBase()

    [<HttpGet("{id}")>]
    member __.Get(id:string) : Entity =
        Elastic.ReadDocument id

    [<HttpPost>]
    member __.Create([<FromBody>] body:Object ) =
        body
        |> Convert.ToString
        |> JObject.Parse
        |> fun parseResult -> parseResult.ToObject<Entity>()
        |> Elastic.WriteDocument 