namespace FoodFile.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Microsoft.Extensions.Configuration
open FoodFile

[<TestClass>]
type TestClass () =

    [<ClassInitialize>]
    static member public PrepareElastic (context:TestContext) =
        
        let config = (new ConfigurationBuilder()).AddJsonFile("appSettings.test.json").Build()

        let host = config.GetValue<string>("elastic")
        let entityindex = config.GetValue<string>("entityindex")
        let memberindex = config.GetValue<string>("memberindex")
        let creationcmd = config.GetValue<string>("indexcreationcmd")
        ElasticService.InitIndices host entityindex memberindex creationcmd
        
        let es = new ElasticService(config) :> IElasticService

        es
        |> fun service -> List.iter (fun memb ->
                service.WriteMember memb) FoodFile.TestMembers.all

        es
        |> fun service -> List.iter (fun entity ->
                service.WriteEntity entity) FoodFile.TestEntities.all

    [<TestMethod>]
    member this.TestMethodPassing () =
        Assert.IsTrue(true);
