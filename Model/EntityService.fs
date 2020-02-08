namespace FoodFile

open FSharp.Data
open Newtonsoft.Json.Linq

type MemberActivity = {
    Member:Member
    Entities:string list;
}

type IEntityService =
    abstract member LocalDownchainSearch : (string list -> Entity list)
    abstract member LocalUpchainSearch : (string list -> Entity list)
    abstract member CompleteSearch : (SearchDirection -> string option -> string list -> Entity list)

type EntityService (ms:IMemberService,els:IElasticService,ti:ThisInstance) = 

    static member ExtractInEntities = fun (entities:Entity list) ->
        entities
        |> List.fold (fun (state:string list) entity ->
            entity.InEntities @ state
            |> List.distinct) []

    static member private SearchEntitiesRemote = fun (direction:SearchDirection) (memberAPI:string) (entityIDs:string list) ->
        async {

            if entityIDs.IsEmpty
            then return []
            else
            
            let url = 
                (List.map ( fun id -> "id=" + id )
                >> String.concat "&"
                >> (fun arguments -> memberAPI + direction.ToUrlString() + "/Multiple?" + arguments)) entityIDs

            try
                let! result = Http.AsyncRequestString url 
                return 
                    (JArray.Parse // Errors may also occur here.
                    >> (fun parsed -> parsed.ToObject<Entity list>())
                    >> List.filter( fun entity -> entity.Verify )) result
            with
            _ -> return []
        }

    static member private ComputeQueryTasks = fun (queryHistory:MemberActivity list) (ids:string list) ->
        queryHistory
        |> List.fold (fun (state:MemberActivity list) (current:MemberActivity) ->
                ids
                |> List.filter ( fun id -> not( List.exists ( fun doneID -> id=doneID ) current.Entities ) )
                |> fun todo -> {current with Entities=todo}
                |> fun todo -> todo::state
            ) []

    static member private ExecuteQueryTasks = fun (direction:SearchDirection) (tasks:MemberActivity list) ->
        tasks
        |> List.map ( fun (task) ->
            async {
                // Create Async Requests
                let! entities = EntityService.SearchEntitiesRemote direction task.Member.API task.Entities
                return (task.Member, entities)
            })
        // Execute
        |> (Async.Parallel >> Async.RunSynchronously)

    static member private MergeResults = fun (lastResult:Entity list) (iterationResult:(Member*Entity list)[]) ->
        iterationResult
        // Extract the Entity Lists
        |> Array.map (fun current ->
                match current with
                | (_,entities) -> entities
            )
        // Concatenate all result Lists
        |> Array.fold (fun (state:Entity list) (current:Entity list) ->  
                current@state
            ) []
        // Merge them into the last Result list
        |> List.fold (fun (state:Entity list) (current:Entity) ->
                current.MergeInto state
            ) lastResult

    static member private UpdateDoneLists = fun (iterationResult:(Member*Entity list)[]) (ids:string list) (history:MemberActivity list)  -> 
        history
        |> List.map (fun ma -> 
            iterationResult
            |> Array.tryFind ( fun (m, _) -> m=ma.Member )
            |> function
            | Some (_, entities) -> 
                entities
                |> List.map (fun entity -> entity.ID)
                |> fun responseIDs -> ids@responseIDs
                |> List.distinct
                |> fun doneIDs -> {ma with Entities=doneIDs}
            | None -> {ma with Entities=ids}
            )

    static member private MergeIDs = fun (direction:SearchDirection) (entities:Entity list) (entityIDs: string list) ->
        match direction with
        | Up ->
            entities
            |> List.map (fun entity -> entity.ID)
        | Down ->
            entities
            |> List.collect (fun entity -> entity.InEntities)
        |> fun result -> entityIDs @ result
        |> List.distinct

    member private this.AppendResultMembers = fun (mergedResult:Entity list) (lastHistory:MemberActivity list) ->
           mergedResult
           |> MemberService.ExtractMemberIDs
           |> List.filter (fun foundMember -> // Keep only those which need to be added.
                   not (List.exists (fun (lhEntry:MemberActivity) -> lhEntry.Member.ID = foundMember ) lastHistory )
               )
           |> ms.GetMembersRemote
           |> List.map (fun m -> {Member=m; Entities=[]})
           |> fun newMAs -> lastHistory@newMAs

    member this.ExecuteLocalDownchainSearch (doneIDs:string list) (result:Entity list) (openIDs:string list) = 
        if openIDs.IsEmpty then result else
        let doneAfterIDs = doneIDs @ openIDs
        openIDs
        |> els.GetEntitiesLocal
        |> function
            | [] -> result
            | entities -> 
                EntityService.ExtractInEntities entities
                // Remove those we already did and those already on our ToDo
                |> List.filter (fun id -> not(List.exists (fun elem -> id=elem) doneAfterIDs))
                |> this.ExecuteLocalDownchainSearch doneAfterIDs (result @ entities)
        (*
            The openIDs is necessary because in some cases, the search might be triggered
            with multiple openIDs elements. During execution, we might find entities which 
            are also on the open list, but we want to prevent to search twice.
        *)

    member this.ExecuteLocalUpchainSearch (doneIDs:string list) (result:Entity list) (openIDs:string list) =
        if openIDs.IsEmpty then result else
        let doneAfterIDs = doneIDs @ openIDs
        openIDs
        |> els.GetByInEntities
        |> function
            | [] -> result
            | entities ->
                entities
                |> List.map (fun entity -> entity.ID)
                // Remove those we already did and those already on our ToDo
                |> List.filter (fun id -> not(List.exists (fun elem -> id=elem) doneAfterIDs))
                |> this.ExecuteLocalUpchainSearch doneAfterIDs (result @ entities) 
            
    member this.ExecuteCompleteSearch = fun (lastResult:Entity list) (direction:SearchDirection) (lastHistory:MemberActivity list) (ids:string list) -> 
        
        let iterationResult =
            (EntityService.ComputeQueryTasks lastHistory 
            >> EntityService.ExecuteQueryTasks direction) (ids)

        // Merge Results (also with results so far)
        let mergedResult = EntityService.MergeResults lastResult iterationResult
            
        // We only proceed if there was something new in our search
        if (Set.ofList lastResult) = (Set.ofList mergedResult)
        then lastResult
        else

        // Update Member-Pool (ID / API)
        let mergedHistory = 
            (EntityService.UpdateDoneLists iterationResult ids
            >> this.AppendResultMembers mergedResult) (lastHistory)

        
        // Update ids (AFTER Member-Pool update)
        let mergedIDs = EntityService.MergeIDs direction mergedResult ids

        this.ExecuteCompleteSearch mergedResult direction mergedHistory mergedIDs


    interface IEntityService with

        member this.LocalDownchainSearch = this.ExecuteLocalDownchainSearch [] []

        member this.LocalUpchainSearch = fun (entityIDs:string list) ->
            entityIDs
            |> els.GetEntitiesLocal
            |> fun initials -> this.ExecuteLocalUpchainSearch [] initials entityIDs
        

        member this.CompleteSearch = fun (direction:SearchDirection) (memberID:string option) (entityIDs:string list) ->
            match memberID with
            | None -> None
            | Some id -> ms.GetMemberRemote id
            |> function
            | Some remoteMember ->
                match ti.data with
                | Some thisInstance -> [{Member=thisInstance; Entities=[]};{Member=remoteMember; Entities=[]}]
                | None -> [{Member=remoteMember; Entities=[]}]
            | None ->
                match ti.data with
                | Some thisInstance -> [{Member=thisInstance; Entities=[]}]
                | None -> []
            |> function
            | [] -> []
            | prefill -> this.ExecuteCompleteSearch [] direction prefill entityIDs