namespace FoodFile

open System
open Newtonsoft.Json
open FSharp.Data
open Newtonsoft.Json.Linq
open Types

// The Elastic Module handels all interaction with the local Elastic instance.

module Elastic =

    /// elasticsearch index name
    [<Literal>]
    let EntityIndex = "entities"
    [<Literal>]
    let ParticipantIndex = "participants"
    [<Literal>]
    let SearchString = "{\"query\": {\"ids\" : {\"values\" : [\"#ID\"]}}}"
    [<Literal>]
    let Host = "http://localhost:9200/"

    let private GetByID = fun (index:string) (id:string) ->
        SearchString.Replace("#ID", id)
        |> fun body -> 
            Http.RequestString ( Host + index + "/_search", httpMethod = "POST", headers = [ "Content-Type","application/json" ], body = TextRequest body)
        |> JObject.Parse
        |> fun (response:JObject) -> 
            if response.["hits"].["total"].["value"].ToObject<int>() = 0 
            then None
            else Some (response.["hits"].["hits"].[0].["_source"])

    let private WriteById = fun (index:string) (id:string) (toWrite:'a) ->
        toWrite
        |> JsonConvert.SerializeObject
        |> fun toSend -> 
            Http.RequestString ( Host + index + "/_create/" + id, httpMethod = "POST", headers = [ "Content-Type","application/json" ], body = TextRequest toSend)
   
    let GetEntityLocal = fun id ->
        id
        |> GetByID EntityIndex
        |> function
            | Some(entityJson) -> Some(entityJson.ToObject<Entity>())
            | None -> None

    // ToDo: Das sollte die einzige variante sein. Wenn es ihn Local nicht gibt wird er anderweitig gefetcht und local dazugeschrieben (wir cashen quasi immer)
    // Oder wir gehen halt einfach als gegeben davon aus dass dieser ES-Index durch einen separaten mecahnismus gesynct wird und ebtrachten das für den moment als Out of Scope.
    let GetParticipantLocal = fun id ->
        id
        |> GetByID ParticipantIndex
        |> function
            | Some(participantJson) -> Some(participantJson.ToObject<Participant>())
            | None -> None
    
    let WriteEntity = fun (entity:Entity) ->
        match GetVerifiedEntityID entity with
        | None -> raise (System.ArgumentException("Entity empty or not consistent."))
        | Some(ID) -> WriteById EntityIndex ID entity

    let WriteParticipant = fun (participant:Participant) ->
        WriteById ParticipantIndex participant.ID participant
            

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