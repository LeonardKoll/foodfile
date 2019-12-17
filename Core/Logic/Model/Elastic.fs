namespace FoodFile

open System
open Newtonsoft.Json
open FSharp.Data
open Newtonsoft.Json.Linq

open Railway

module Elastic =

    /// elasticsearch index name
    [<Literal>]
    let SearchIndex = "test"

    [<Literal>]
    let Host = "http://localhost:9200/"

    type Result<'TSuccess,'TFailure> = 
        | Success of 'TSuccess
        | Failure of 'TFailure

    let WriteDocument = fun (entity:Entity) ->
        entity
        |> JsonConvert.SerializeObject
        |> fun toSend -> 
            Http.RequestString ( Host + SearchIndex + "/_create/" + entity.ID, httpMethod = "POST", headers = [ "Content-Type","application/json" ], body = TextRequest toSend)


    let ReadDocument = fun id ->
        "{\"query\": {\"ids\" : {\"values\" : [\"" + id + "\"]}}}"
        |> fun body -> 
            Http.RequestString ( Host + SearchIndex + "/_search", httpMethod = "POST", headers = [ "Content-Type","application/json" ], body = TextRequest body)
        |> JObject.Parse
        |> fun (response:JObject) -> response.["hits"].["hits"].[0].["_source"].ToObject<Entity>()
   



    let ExecuteRead = fun args ->
        startWith "QuickTest"
        |> switch ReadDocument
        |> Convert.ToString
        |> printf "Result: %s"
        0

    let ExecuteWrite = fun args ->
        startWith Data.TestEntity
        |> switch WriteDocument
        |> Convert.ToString
        |> printf "Result: %s"
        0