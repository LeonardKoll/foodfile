namespace FoodFile

open Microsoft.AspNetCore.Mvc.Filters
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration

// https://stackoverflow.com/questions/4348071/how-to-pass-parameters-to-a-custom-actionfilter-in-asp-net-mvc-2/4348097
// https://stackoverflow.com/questions/38991603/how-can-i-disable-some-apis-of-my-asp-net-application

type IConfigurableController =
    abstract member config:IConfiguration

type EnableInMode(modes:string array) =
    inherit ActionFilterAttribute()

    override this.OnActionExecuting(filterContext:ActionExecutingContext) =
        let mode = (filterContext.Controller:?>IConfigurableController).config.GetValue<string>("mode")
        modes
        |> Array.exists (fun elem -> elem=mode)
        |> function
            | true -> this.proceed(filterContext)
            | false -> (filterContext.Result <- NotFoundResult())

    member this.proceed (filterContext) =
        base.OnActionExecuting(filterContext)