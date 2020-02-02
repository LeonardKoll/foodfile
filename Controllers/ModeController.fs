namespace FoodFile

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Newtonsoft.Json

type ModeReturn = {
    Mode:string; // member, regular, combined
    MemberID:string;
    MemberName:string;
}

[<ApiController>]
[<Route("api/[controller]")>]
type ModeController (config:IConfiguration, ti:ThisInstance) =
    inherit ControllerBase()

    [<HttpGet()>] // Jetzt wo hier string als return steht müssen wir ContentType JSon evtl manuell setzen.
    member __.Get() : string =
        match ti.data with
            | None -> {Mode=config.GetValue<string>("mode"); MemberID=""; MemberName=""}
            | Some m -> {Mode=config.GetValue<string>("mode"); MemberID=m.ID; MemberName=m.Name}
        |> JsonConvert.SerializeObject