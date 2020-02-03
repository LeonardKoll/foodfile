namespace FoodFile

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer
open System.IO
open System.Threading

// https://stackoverflow.com/questions/44785729/how-do-i-access-command-line-parameters-in-an-asp-net-core-mvc-app
// https://stackoverflow.com/questions/40917669/why-startups-iconfigurationroot-is-null/40950300#40950300
// https://stackoverflow.com/questions/46940710/getting-value-from-appsettings-json-in-net-core
// https://weblog.west-wind.com/posts/2016/may/23/strongly-typed-configuration-settings-in-aspnet-core

module Startup =

    type Startup private () =
        new (configuration:IConfiguration) as this =
            Startup() then
            this.Configuration <- configuration

        // This method gets called by the runtime. Use this method to add services to the container.
        member this.ConfigureServices(services: IServiceCollection) =

            let host = this.Configuration.GetValue<string>("elastic")
            let entityindex = this.Configuration.GetValue<string>("entityindex")
            let memberindex = this.Configuration.GetValue<string>("memberindex")
            let creationcmd = this.Configuration.GetValue<string>("indexcreationcmd")
            ElasticService.InitIndices host entityindex memberindex creationcmd

            services.AddScoped<IMemberService, MemberService>() |> ignore
            services.AddScoped<IElasticService, ElasticService>() |> ignore
            services.AddScoped<IEntityService, EntityService>() |> ignore

            this.Configuration.GetValue<string>("this")
            |> (MemberService(this.Configuration):>IMemberService).GetMemberRemote 
            |> fun result -> services.AddSingleton<ThisInstance>(new ThisInstance(result))
            |> ignore

            services.AddControllers() |> ignore

            services.AddSpaStaticFiles (fun configuration -> 
                configuration.RootPath <- "ClientApp/build"
                )
            
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
            if (env.IsDevelopment()) then
                app.UseDeveloperExceptionPage() |> ignore

            app.UseHttpsRedirection() |> ignore
            app.UseStaticFiles() |> ignore
            app.UseSpaStaticFiles() |> ignore
            app.UseRouting() |> ignore

            app.UseAuthorization() |> ignore

            app.UseEndpoints(fun endpoints ->
                //endpoints.MapControllers() |> ignore
                endpoints.MapControllerRoute ("default", "{controller}/{action=Index}/{id?}") |> ignore
                ) |> ignore

            app.UseSpa (fun spa ->
                spa.Options.SourcePath <- "ClientApp"
                if env.IsDevelopment() then spa.UseReactDevelopmentServer("start")
                )

        member val Configuration : IConfiguration = null with get, set



    [<EntryPoint>]
    let main args =
        
        // AddSingelton = Wird einmal erstellt
        // AddScoped = Wird einmal pro Verbindung erstellt
        // AddTransient = Wird einmal pro Anforderung erstellt

        let config = // Eventuell gehts auch ohne, komplett automatisch
            ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .AddCommandLine(args)
                .Build()

        //Interactions with Elasticsearch will fail right after startup of the ES container.
        Thread.Sleep (1000 * config.GetValue<int>("delay"))

        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(fun webBuilder ->
                webBuilder.UseConfiguration(config) |> ignore
                webBuilder.UseStartup<Startup>() |> ignore
            )
            .Build().Run()

        0