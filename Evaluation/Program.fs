// Assemble Entities

open System
open FoodFile
open Microsoft.Extensions.Configuration

[<EntryPoint>]
let main args =
    
    // Read Configuration
    let config = 
        ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .AddCommandLine(args)
            .Build()
    let fffElastic = config.GetValue<string>("fffElastic")
    let ssElastic = config.GetValue<string>("ssElastic")
    let yjElastic = config.GetValue<string>("yjElastic")
    let fffId = config.GetValue<string>("fffId")
    let ssId = config.GetValue<string>("ssId")
    let yjId = config.GetValue<string>("yjId")
    let entities = config.GetValue<string>("entityindex")
    let members = config.GetValue<string>("memberindex")
    let creationcmd = config.GetValue<string>("indexcreationcmd")
   
    // Re-Create Indices
    printfn "Resetting Elasticsearch..."
    ElasticService.RemoveIndices fffElastic entities members
    ElasticService.InitIndices fffElastic entities members creationcmd
    printfn "[ok] FreshFruitFarmers"
    ElasticService.RemoveIndices ssElastic entities members
    ElasticService.InitIndices ssElastic entities members creationcmd
    printfn "[ok] SugarSilo"
    ElasticService.RemoveIndices yjElastic entities members
    ElasticService.InitIndices yjElastic entities members creationcmd
    printfn "[ok] YummyJam"
    
    // Retreive Scenario
    let scenario = (
        typeof<ScenarioLibrary.Scenario>.DeclaringType.GetProperty 
        >> fun propInfo -> 
            propInfo.GetValue(propInfo) 
            :?> ScenarioLibrary.Scenario)(config.GetValue<string>("apply"))

    printfn "Deploying Scenario..."

    // Deploy to FFF
    Utils.getElasticService fffElastic entities members
    |> Utils.deployScenarioToMember fffId ssId yjId
    |> fun deployer -> deployer scenario.FreshFruitFarmers
    printfn "[ok] FreshFruitFarmers"

    // Deploy to SS
    Utils.getElasticService ssElastic entities members
    |> Utils.deployScenarioToMember fffId ssId yjId
    |> fun deployer -> deployer scenario.SugarSilo
    printfn "[ok] SugarSilo"

    // Deploy to YJ
    Utils.getElasticService yjElastic entities members
    |> Utils.deployScenarioToMember fffId ssId yjId
    |> fun deployer -> deployer scenario.YummyJam
    printfn "[ok] YummyJam"
    
    0