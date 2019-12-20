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

    

    let WriteDocument = fun (entity:Entity) ->
        match entity.ID with
        | None -> raise (System.ArgumentException("Entity empty or not consistent."))
        | Some(ID) -> 
            entity
            |> JsonConvert.SerializeObject
            |> fun toSend -> 
                Http.RequestString ( Host + SearchIndex + "/_create/" + ID, httpMethod = "POST", headers = [ "Content-Type","application/json" ], body = TextRequest toSend)


    let ReadDocument = fun id ->
        "{\"query\": {\"ids\" : {\"values\" : [\"" + id + "\"]}}}"
        |> fun body -> 
            Http.RequestString ( Host + SearchIndex + "/_search", httpMethod = "POST", headers = [ "Content-Type","application/json" ], body = TextRequest body)
        |> JObject.Parse
        |> fun (response:JObject) -> response.["hits"].["hits"].[0].["_source"].ToObject<Entity>()
   

   // Das sollte funktionierren, aber es retrievt ja aktuell nur in der eigenen Datenbank. Wir bräuchten dann als nächstes was, was übergreifend retreivt.
   // Und wir brauchen getAtoms entityID shortID version bzw getAtoms LongID und halt auch nur entityID shortID, sodass man alle Versionen des Atoms bekommt.
   // Und im nächsten Schritt haben wir dann vielleicht so tier funktionen:
        // Tier 1: Funktionen Read und Write Document. Modify?
        // Tier 2: Funktionen, die Atome beschaffen und schrieben
        //====== Bevor wir uns im Tier 3 kümmern muss dann Authentication / Authroization her. Tier 2 muss dann auch entsprechend erweitert werden.
        // Tier 3: Funktionen/Systeme die Caching einstellungen realasieren. 
        // Tier 4: Funkrionen/Systeme, die Konfliktresolvierung (automatisch oder manuell) realisieren
    let getAtoms = fun entityID ->
        entityID
        |> ReadDocument
        |> fun doc -> doc.Atoms

    // Just Testing
    let ExecuteRead = fun () ->
        startWith "E1"
        |> switch ReadDocument
        |> Convert.ToString
        |> printf "Result: %s"

    // Just Testing
    let ExecuteWrite = fun () ->
        startWith Data.TestEntity
        |> switch WriteDocument
        |> Convert.ToString
        |> printf "Result: %s"