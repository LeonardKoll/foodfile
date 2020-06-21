namespace FoodFile.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Microsoft.Extensions.Configuration
open FoodFile

[<TestClass>]
type TestClass () =

    [<ClassInitialize>]
    static member public PrepareElastic (context:TestContext) =
    
        let esYJ = TestClass.prepareIndices "appSettings.yj.json"
        esYJ
        |> fun service -> List.iter (fun memb ->
            service.WriteMember memb) FoodFile.TestMembers.all
        esYJ
        |> fun service -> List.iter (fun entity ->
                service.WriteEntity entity) FoodFile.TestEntities.yummyjamEntities

        TestClass.prepareIndices "appSettings.fff.json"
        |> fun service -> List.iter (fun entity ->
            service.WriteEntity entity) FoodFile.TestEntities.freshfruitfarmerEntities

        TestClass.prepareIndices "appSettings.ss.json"
        |> fun service -> List.iter (fun entity ->
            service.WriteEntity entity) FoodFile.TestEntities.sugarsiloEntities
    
    static member private prepareIndices (appSettings:string) : IElasticService =
        let config = (new ConfigurationBuilder()).AddJsonFile(appSettings).Build()
        let host = config.GetValue<string>("elastic")
        let entityindex = config.GetValue<string>("entityindex")
        let memberindex = config.GetValue<string>("memberindex")
        let creationcmd = config.GetValue<string>("indexcreationcmd")
        ElasticService.RemoveIndices host entityindex memberindex
        ElasticService.InitIndices host entityindex memberindex creationcmd
        new ElasticService(config) :> IElasticService
     

    [<TestMethod>]
    member this.TestMethodPassing () =
        Assert.IsTrue(true);
