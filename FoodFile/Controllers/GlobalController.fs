namespace FoodFile

open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json
open Microsoft.Extensions.Configuration
open System
open Newtonsoft.Json.Linq

type GcReturn = {
    Entities:Entity list;
    Members:Member list;
}

type VerifyRequest = {
    CompleteID: string;
    Members: string list
}

[<ApiController>]
[<Route("api/entities/[controller]")>]
[<EnableInMode([|"regular";"combined"|])>]
type GlobalController (ms:IMemberService,els:IElasticService, ens:IEntityService, config:IConfiguration) =
    inherit ControllerBase()

    (*
        ID-Format:
        <member-ID>-<entity-ID>-<token>
        where the member-ID and token are optional. A token is only allowed if a member-ID is provided.
    *)

    [<HttpGet("downchain/{id}")>] // Jetzt wo hier string als return steht müssen wir ContentType JSon evtl manuell setzen.
    member __.GetDownchain (id:string) : string = // Member ID optional

        let entities =
            (function
            | [|entityID|] -> ens.CompleteSearch Downchain None entityID None
            | [|memberID; entityID|] -> ens.CompleteSearch Downchain (Some memberID) entityID None
            | [|memberID; entityID; entityToken|] -> ens.CompleteSearch Downchain (Some memberID) entityID (Some entityToken)
            | _ -> []) (id.Split('-'))
        
        let members = ms.ExtractMembers entities
        {Entities=entities; Members=members}
        |> JsonConvert.SerializeObject
        // ToDo: Error Handling.
        // Note: Die Funktion kann evtl weg weil wir immer Atome aber nicht ganze Docs transferieren

    [<HttpGet("upchain/{id}")>] // Jetzt wo hier string als return steht müssen wir ContentType JSon evtl manuell setzen.
    member __.GetUpchain (id:string) : string = // Member ID optional  

        let entities =
            (function
            | [|entityID|] -> ens.CompleteSearch Upchain None entityID None
            | [|memberID; entityID|] -> ens.CompleteSearch Upchain (Some memberID) entityID None
            | [|memberID; entityID; entityToken|] -> ens.CompleteSearch Upchain (Some memberID) entityID (Some entityToken)
            | _ -> []) (id.Split('-'))

        let members = ms.ExtractMembers entities
        {Entities=entities; Members=members}
        |> JsonConvert.SerializeObject
        // ToDo: Error Handling.
        // Note: Die Funktion kann evtl weg weil wir immer Atome aber nicht ganze Docs transferieren


    [<HttpPost("atom/verify")>]
    member __.VerifyAtom ([<FromBody>] body:Object) : AtomHashSupportCount list =
           body.ToString()
           |> JObject.Parse
           |> fun parseResult -> parseResult.ToObject<VerifyRequest>()
           |> fun request ->
                request.Members
                |> ens.VerifyAtom request.CompleteID 

    [<HttpPost("fetch")>]
    member __.Fetch([<FromBody>] body:Object) =
        
        body.ToString()
        |> JArray.Parse
        |> fun parseResult -> parseResult.ToObject<string list>()
        |> ens.FetchAll

    interface IConfigurableController with
        member this.config = config