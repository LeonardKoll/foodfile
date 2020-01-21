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
    let MemberIndex = "members"
    [<Literal>]
    let SearchString = "{\"query\": {\"ids\" : {\"values\" : #IDs}}}"
    [<Literal>]
    let Host = "http://localhost:9200/" // For docker-compose execution

    // General purpose functions

    let InitIndices = fun () ->
        Http.RequestString ( Host + EntityIndex, httpMethod = "PUT") |> ignore
        Http.RequestString ( Host + MemberIndex, httpMethod = "PUT") |> ignore


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
   

    // Entity functions

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

    // Member functions

    (*
        Note:
        This stuff is only used if the instance serves as a membership provider.
        A regular instance can't lookup members from the local db and retrieves them from
        the membership service API instead.
    *)

    let GetMembersLocal = fun (memberIDs:string list) ->
        memberIDs
        |> GetByIDs MemberIndex
        |> function
            | None -> []
            | Some(jTokenEnum) ->
                jTokenEnum
                |> Seq.map (fun memberJson -> memberJson.ToObject<Member>())
                |> Seq.toList

    let GetMemberLocal = fun (memberID:string) ->
        GetMembersLocal [memberID]
        |> function
            | [] -> None
            | [m] -> Some(m)
            | _ -> None

    let WriteMember = fun (memb:Member) ->
        WriteById MemberIndex memb.ID memb

    let DeleteMember = DeleteByID MemberIndex
