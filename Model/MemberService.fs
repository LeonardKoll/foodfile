﻿namespace FoodFile

open FSharp.Data
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open Microsoft.Extensions.Configuration

(*
    This module retrieves participant data from FoodFile.org
    and handles in-Memory Caching (later)
*)

type IMemberService =
    abstract member GetMembersRemote : (string list -> Member list)
    abstract member GetMemberRemote : (string -> Member option)
    abstract member GetMemberAPIs : (string list -> (string*string) list)
    abstract member ExtractMembers : (Entity list -> Member list)
    abstract member ThisInstance : Member option

type MemberService(config:IConfiguration) =

    let MembershipProvider = config.GetValue<string>("members")
   
    static member ExtractMemberIDs = fun (entities:Entity list) ->
        entities
        |> List.fold (fun (state:string list) entity ->
            entity.InvolvedMembers @ state
            |> List.distinct) []

    interface IMemberService with

        // This instance can also be pure Trace. Then we do not have a participant ID.
        member this.ThisInstance = (this:>IMemberService).GetMemberRemote (config.GetValue<string>("this"))


        member this.GetMembersRemote = fun (memberIDs:string list) ->   
            memberIDs
            |> function
                | [] -> []
                | _ ->
                    memberIDs
                    |> List.map ( fun id -> "id=" + id )
                    |> String.concat "&"
                    |> fun arguments -> MembershipProvider + "Multiple?" + arguments
                    |> Http.RequestString //ToDo: Error Handling. Log errors somewhere end return empty list.
                    |> JArray.Parse // Errors may also occur here.
                    |> fun parsed -> parsed.ToObject<Member list>()
    
        member this.GetMemberRemote = fun (memberID:string) ->
            [memberID]
            |> (this:>IMemberService).GetMembersRemote
            |> List.tryExactlyOne

        member this.GetMemberAPIs = fun (memberIDs:string list) ->
            memberIDs
            |> (this:>IMemberService).GetMembersRemote
            |> List.map ( fun p -> p.API )
            |> List.zip memberIDs

        member this.ExtractMembers = 
            MemberService.ExtractMemberIDs >> (this:>IMemberService).GetMembersRemote