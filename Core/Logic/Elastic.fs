module Elastic

#if INTERACTIVE
    #I @"C:\Users\leona\.nuget\packages\";;
    #r @"elasticsearch.net\7.4.1\lib\net461\Elasticsearch.Net.dll";;
    #r @"nest\7.4.1\lib\net461\Nest.dll";;
#endif

open System
open Newtonsoft.Json
open FSharp.Data
open Newtonsoft.Json.Linq


/// elasticsearch index name
[<Literal>]
let SearchIndex = "test"

[<Literal>]
let Host = "http://localhost:9200/"


let WriteDocument = fun (entity:Data.Entity) ->
     entity
     |> JsonConvert.SerializeObject
     |> fun toSend -> 
            printf "To Send: %s: " toSend
            Http.RequestString ( Host + SearchIndex + "/_create/" + entity.ID, httpMethod = "POST", headers = [ "Content-Type","application/json" ], body = TextRequest toSend)
            |> printf "Write Result %s"


let ReadDocument = fun id ->
    "{\"query\": {\"ids\" : {\"values\" : [\"" + id + "\"]}}}"
    |> fun body -> 
        Http.RequestString ( Host + SearchIndex + "/_search", httpMethod = "POST", headers = [ "Content-Type","application/json" ], body = TextRequest body)
    |> JObject.Parse
    |> fun (response:JObject) -> response.["hits"].["hits"].[0].["_source"].ToObject<Data.Entity>()
   



//[<EntryPoint>]
let ExecuteWrite = fun args ->
    Data.TestEntity
    |> WriteDocument
    0

[<EntryPoint>]
let ExecuteRead = fun args ->
    "SecondOfficialEntity"
    |> ReadDocument
    |> Convert.ToString
    |> printf "Result: %s"
    0
