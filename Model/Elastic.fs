namespace FoodFile

open System
open Newtonsoft.Json
open FSharp.Data
open Newtonsoft.Json.Linq

// The Elastic Module handels all interaction with the local Elastic instance.

module Elastic =

    // elasticsearch index name
    // Future: Time sliced indexing.
    [<Literal>]
    let EntityIndex = "entities"
    [<Literal>]
    let SearchString = "{\"query\": {\"ids\" : {\"values\" : #IDs}}}"
    [<Literal>]
    let Host = "http://localhost:9200/"

    let private GetByIDs = fun (index:string) (ids:string list) ->
        ids
        |> JsonConvert.SerializeObject
        |> fun toInsert -> SearchString.Replace ("#IDs", toInsert)
        // ToDo: On Exception: Log "Request to ElasticSearch Failed" somewher eand return an empty list
        |> fun body -> 
            Http.RequestString (    Host + index + "/_search", 
                                    httpMethod = "POST", 
                                    headers = [ "Content-Type","application/json" ], 
                                    body = TextRequest body)
        |> JObject.Parse
        |> fun (response:JObject) -> 
            match response.["hits"].["total"].["value"].ToObject<int>() with
            | 0 -> None
            | _ -> Some(response.SelectTokens "hits.hits.._source")

    let private DeleteByID = fun (index:string) (id:string) ->
        Http.RequestString (    Host + index + "/_doc/" + id, 
                                httpMethod = "DELETE")

    let private WriteById = fun (index:string) (id:string) (toWrite:'a) ->
        toWrite
        |> JsonConvert.SerializeObject
        |> fun toSend -> 
            Http.RequestString ( Host + index + "/_doc/" + id, httpMethod = "Put", headers = [ "Content-Type","application/json" ], body = TextRequest toSend)
   
    let GetEntitiesLocal = fun (entityIDs:string list) ->
        entityIDs
        |> GetByIDs EntityIndex
        |> function
            | None -> []
            | Some(jTokenEnum) -> 
                jTokenEnum
                |> Seq.fold ( fun (state:Entity list) entityJson ->
                        let entity = entityJson.ToObject<Entity>()
                        if entity.Verify then entity::state else state
                    ) []

    let GetEntityLocal = fun (entityID:string) ->
        [entityID]
        |> GetByIDs EntityIndex
        |> function
            | None -> None
            | Some(jTokenEnum) ->
                jTokenEnum
                |> Seq.tryExactlyOne
                |> function
                    | None -> None
                    | Some (entityJson) -> 
                        let entity = entityJson.ToObject<Entity>()
                        if entity.Verify then Some(entity) else None
            
    let WriteEntity = fun (entity:Entity) ->
        match entity.Verify with
        | false -> raise (System.ArgumentException("Entity empty or not consistent."))
        | true -> WriteById EntityIndex entity.ID entity
        

   // Das sollte funktionierren, aber es retrievt ja aktuell nur in der eigenen Datenbank. Wir bräuchten dann als nächstes was, was übergreifend retreivt.
   // Und wir brauchen getAtoms entityID shortID version bzw getAtoms LongID und halt auch nur entityID shortID, sodass man alle Versionen des Atoms bekommt.
   // Und im nächsten Schritt haben wir dann vielleicht so tier funktionen:
        // Tier 1: Funktionen Read und Write Document. Modify?
        // Tier 2: Funktionen, die Atome beschaffen und schrieben
        //====== Bevor wir uns im Tier 3 kümmern muss dann Authentication / Authroization her. Tier 2 muss dann auch entsprechend erweitert werden.
        // Tier 3: Funktionen/Systeme die Caching einstellungen realasieren. 
        // Tier 4: Funkrionen/Systeme, die Konfliktresolvierung (automatisch oder manuell) realisieren
    (* let GetAtoms = fun entityID ->
        entityID
        |> ReadDocument
        |> function
            | None -> None
            | Some entity -> Some entity.Atoms
            *)