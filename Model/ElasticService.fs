namespace FoodFile

open System
open Newtonsoft.Json
open FSharp.Data
open Newtonsoft.Json.Linq
open System.Collections.Generic
open Microsoft.Extensions.Configuration

// The Elastic Module handels all interaction with the local Elastic instance.
type IElasticService =
    abstract member GetEntitiesLocal : (string list -> Entity list)
    abstract member GetEntityLocal : (string -> Entity option)
    abstract member WriteEntity : (Entity -> unit)
    abstract member GetByInEntities : (string list -> Entity list)
    abstract member GetMembersLocal : (string list -> Member list)
    abstract member GetMemberLocal : (string -> Member option)
    abstract member WriteMember : (Member -> unit)
    abstract member DeleteMember : (string -> unit)


type ElasticService(config:IConfiguration) =

    let Host = config.GetValue<string>("elastic")

    // ToDo: Time sliced indexing.
    static let EntityIndex = "entities"
    static let MemberIndex = "members"
    static let ByIDsQuery = "{\"from\":0,\"size\":10000,\"query\": {\"ids\" : {\"values\" : #IDs }}}"
    static let ByInEntitiesQuery = "{\"from\":0,\"size\":10000,\"query\": {\"bool\": {\"filter\": {\"terms\": {\"InEntities\": #IDs }}}}}"

    static member InitIndices = fun (host:string) (creationCmd:string) ->
        try
            Http.RequestString (    host + EntityIndex, 
                                    httpMethod = "PUT", 
                                    headers = [ "Content-Type","application/json" ],
                                    body = TextRequest creationCmd) |> ignore
        with _ -> ()
        try
            Http.RequestString (    host + MemberIndex, 
                                    httpMethod = "PUT", 
                                    headers = [ "Content-Type","application/json" ],
                                    body = TextRequest creationCmd) |> ignore
        with _ -> ()

    member private this.GetDocuments = fun (query:string) (index:string) (ids:string list) ->
        ids
        |> JsonConvert.SerializeObject
        |> fun toInsert -> query.Replace ("#IDs", toInsert)
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

    member private this.GetByIDs = this.GetDocuments ByIDsQuery
    
    member private this.DeleteByID = fun (index:string) (id:string) ->
        Http.RequestString (    Host + index + "/_doc/" + id, 
                                httpMethod = "DELETE")
        |> ignore
    
    member private this.WriteById = fun (index:string) (id:string) (toWrite:obj) ->
        toWrite
        |> JsonConvert.SerializeObject
        |> fun toSend -> 
            Http.RequestString ( Host + index + "/_doc/" + id, httpMethod = "Put", headers = [ "Content-Type","application/json" ], body = TextRequest toSend)
        |> ignore
    
    member private this.ConverToEntities = fun (toConvert:IEnumerable<JToken> option) ->
        toConvert
        |> function
        | None -> []
        | Some(jTokenEnum) -> 
            jTokenEnum
            |> Seq.fold ( fun (state:Entity list) entityJson ->
                    let entity = entityJson.ToObject<Entity>()
                    if entity.Verify then entity::state else state
                ) []

    interface IElasticService with     

        member this.GetEntitiesLocal = fun (entityIDs:string list) ->
            entityIDs
            |> this.GetByIDs EntityIndex
            |> this.ConverToEntities

        member this.GetEntityLocal = fun (entityID:string) ->
            [entityID]
            |> this.GetByIDs EntityIndex
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
            
        member this.WriteEntity = fun (entity:Entity) ->
            match entity.Verify with
            | false -> raise (System.ArgumentException("Entity empty or not consistent."))
            | true -> this.WriteById EntityIndex entity.ID (entity:>obj)

        member this.GetByInEntities = fun (entityIDs:string list) ->
            entityIDs
            // For some reason ES requires the ids to be lowercase in the searchstring.
            |> List.map (fun (id:string) -> id.ToLower())
            |> this.GetDocuments ByInEntitiesQuery EntityIndex
            |> this.ConverToEntities
        
        // Member functions

        (*
            Note:
            This stuff is only used if the instance serves as a membership provider.
            A regular instance can't lookup members from the local db and retrieves them from
            the membership service API instead.
        *)

        member this.GetMembersLocal = fun (memberIDs:string list) ->
            memberIDs
            |> this.GetByIDs MemberIndex
            |> function
                | None -> []
                | Some(jTokenEnum) ->
                    jTokenEnum
                    |> Seq.map (fun memberJson -> memberJson.ToObject<Member>())
                    |> Seq.toList

        member this.GetMemberLocal = fun (memberID:string) ->
            (this:>IElasticService).GetMembersLocal [memberID]
            |> function
                | [] -> None
                | [m] -> Some(m)
                | _ -> None

        member this.WriteMember = fun (memb:Member) ->
            this.WriteById MemberIndex memb.ID (memb:>obj)

        member this.DeleteMember = this.DeleteByID MemberIndex
