namespace FoodFile

open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System
open Types
open Microsoft.Extensions.Configuration

type DeleteMember = {
    ID: string; // 6 Zeichen
    Password: string; //ToDo
    // Future: Signature Key, Expires (when we need to discard memory cahce)
}

[<ApiController>]
[<Route("api/[controller]")>]
type MembersController (es:IElasticService, config:IConfiguration) =
    inherit ControllerBase()

    let disabled = config.GetValue<string>("mode") = "regular"

    [<HttpGet("{id}")>] // Jetzt wo hier string als return steht müssen wir ContentType JSon evtl manuell setzen.
    member __.Get([<FromQuery>] id:string array) : string = //Multiple
        if disabled then (raise (Exception "Disabled")) else
        id
        |> Array.toList
        |> es.GetMembersLocal
        |> List.map (fun memb -> memb.Publish)
        |> JsonConvert.SerializeObject
        // ToDo: Error Handling.

    [<HttpPost>]
    member __.Create([<FromBody>] body:Object ) : string =

        if disabled then (raise (Exception "Disabled")) else

        let memb = 
            (JObject.Parse
            >> fun parseResult -> parseResult.ToObject<Member>()
            ) (body.ToString())

        memb.ID
        |> function
            | "" -> 
                let memberID = newMemberID
                {memb with ID=memberID} |> es.WriteMember |> ignore
                Result ("Created new member with ID " + memberID.ToString())
            | id -> 
                es.GetMemberLocal id
                |> function
                    | None -> Error "ID provided but member not found. Provide empty string for creation of new members."
                    | Some (result) -> 
                        match result.Password=memb.Password with
                        | true -> 
                            es.WriteMember memb |> ignore
                            Result ("Edited member with ID " + id.ToString())
                        | false -> 
                            Error "Passwort does not match."
        
        // In any case, we will eventually output the id / error.
        |> JsonConvert.SerializeObject
        
        // ToDo: Error Handling.
        // ToDo: What if PW Change desired? Brute Force?

    [<HttpDelete>]
    member __.Delete([<FromBody>] toDelete:Object) : string =

        if disabled then (raise (Exception "Disabled")) else

        let memb = 
            (JObject.Parse
            >> (fun result -> result.ToObject<DeleteMember>())
            ) (toDelete.ToString())


        memb.ID
        |> es.GetMemberLocal
        |> function
            | None -> Error "Member not found."
            | Some result ->
                match result.Password=memb.Password with
                    | true -> 
                        es.DeleteMember memb.ID |> ignore
                        Result ("Deleted member with ID " + memb.ID.ToString())
                    | false -> 
                        Error "Passwort does not match."      
        // In any case, we will eventually output the result.
        |> JsonConvert.SerializeObject