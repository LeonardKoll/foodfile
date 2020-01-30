namespace FoodFile

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration

[<ApiController>]
[<Route("api/[controller]")>]
type ModeController (config:IConfiguration) =
    inherit ControllerBase()

    [<HttpGet()>] // Jetzt wo hier string als return steht müssen wir ContentType JSon evtl manuell setzen.
    member __.Get() : string =
        config.GetValue<string>("mode") // member, regular, combined