namespace FoodFile

open FSharp.Data
open Newtonsoft.Json.Linq

type Direction = Downchain | Upchain

type IEntityService =
    abstract member LocalDownchainSearch : (string list -> Entity list)
    abstract member LocalUpchainSearch : (string list -> Entity list)
    abstract member CompleteSearch : (Direction -> string option -> string list -> Entity list)

type CompletedRetreival = {
    MemberID:string;
    Member:Member option;
    Entities: string list;
} with
    
    member this.MergeInto = fun (basis:CompletedRetreival list) ->
        basis
        |> List.fold (fun (state:(bool * CompletedRetreival list)) cCR ->
            match state with
            | (true, result) -> (true, cCR::result)
            | (false, result) ->
                if this.MemberID=cCR.MemberID
                then 
                    let mergedCR = ( 
                        match this.Member with
                        | None ->       {   MemberID=this.MemberID; 
                                            Member=cCR.Member; 
                                            Entities= List.distinct(this.Entities@cCR.Entities)}
                        | Some(m) ->    {   MemberID=this.MemberID; 
                                            Member=Some(m); 
                                            Entities= List.distinct(this.Entities@cCR.Entities)}
                        )
                    (true, mergedCR::result)
                else
                    (false, cCR::result)
            ) (false,[]) // State: (isMerged, resultlist)
        |> function
            | (true, result) -> result          // CR existed already, information merged :)
            | (false, result) -> this::result   // CR did not exist in list. Add now.

type EntityService (ms:IMemberService,els:IElasticService) = 

    static member ExtractInEntities = fun (entities:Entity list) ->
        entities
        |> List.fold (fun (state:string list) entity ->
            entity.InEntities @ state
            |> List.distinct) []

    static member MergeEntities = fun (toMerge:Entity list) (basis:Entity list) ->
        if toMerge.IsEmpty then basis else
        toMerge.Head.MergeInto basis
        |> EntityService.MergeEntities toMerge.Tail

    static member private SearchEntitiesRemote = fun (direction:Direction) (memberAPI:string) (entityIDs:string list) ->
        async {
            
            let url = 
                (List.map ( fun id -> "id=" + id )
                >> String.concat "&"
                >> (fun arguments -> memberAPI + direction.ToString() + "/Multiple?" + arguments)) entityIDs
            
            printfn "%s" url

            let! result = Http.AsyncRequestString url 
            //ToDo: Error Handling. Log errors somewhere end return empty list.
            // Wenn eine url nicht stimmt crasht hier aktuell alles
            
            printfn "result %s" (result.ToString())

            return 
                (JArray.Parse // Errors may also occur here.
                >> (fun parsed -> parsed.ToObject<Entity list>())
                >> List.filter( fun entity -> entity.Verify )) result
        }

    static member private MergeCRs = fun (toMerge:CompletedRetreival list) (basis:CompletedRetreival list) ->
        if toMerge.IsEmpty then basis else
        toMerge.Head.MergeInto basis
        |> EntityService.MergeCRs toMerge.Tail 

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
            

    member this.FillCRsAPIs = fun (crs:CompletedRetreival list) ->
        crs
        |> List.filter ( fun cr -> cr.Member=None )
        |> List.map ( fun ct -> ct.MemberID )
        |> ms.GetMembersRemote
        |> List.map ( fun (m) -> {MemberID=m.ID; Member=Some(m); Entities=[]} )
        |> EntityService.MergeCRs crs

    member this.ExecuteCompleteSearch = fun (result:Entity list) (crs: CompletedRetreival list ) (direction:Direction) (allIDs:string list) ->
             
        printfn "allIDs: %A" allIDs
        printfn "doneIDs: %A" crs

        let cleanedCRS = 
            (this.FillCRsAPIs
            >> List.filter ( fun (cr:CompletedRetreival) -> not (cr.Member=None) )) crs
        
        cleanedCRS
        |> List.map ( fun (cr) ->
            async {
                
                // Compute ToDos for this Participant
                let todoIDs = 
                    List.filter (fun id -> not(List.exists (fun doneID -> id=doneID) cr.Entities) ) allIDs

                printfn "todoIDs: %A" todoIDs

                // Execute request Async
                let! entities = EntityService.SearchEntitiesRemote direction cr.Member.Value.API todoIDs
                // On method call, MemberURL will always be Some bc we cleaned the CRS list before this call.

                return (cr.MemberID, entities)
            })
        // Execute
        |> (Async.Parallel >> Async.RunSynchronously)
        // Merge Data
        |> Array.fold ( fun (searchAgain, mergedE, mergedCRS, mergedAll) (source, entities) ->

            let followupE =
                match direction with
                | Downchain ->
                    entities
                    |> EntityService.ExtractInEntities            
                | Upchain ->
                    entities
                    |> List.map (fun entity -> entity.ID)
                |> List.filter (fun id -> not (List.exists (fun elem -> id=elem) allIDs))

            let followupP = 
                (MemberService.ExtractMemberIDs  
                >> List.filter (fun foundP -> 
                    not (List.exists (fun (cr:CompletedRetreival) -> foundP=cr.MemberID) cleanedCRS)) ) entities
            
            let searchAgain_new =
                not (followupP.IsEmpty && followupE.IsEmpty)

            let mergedE_new =
                EntityService.MergeEntities entities mergedE

            let mergedCRS_new = 
                (List.map ( fun pID -> {MemberID=pID; Member=None; Entities=[]})
                >> EntityService.MergeCRs [{MemberID=source; Member=None; Entities=(allIDs@followupE)}]
                >> EntityService.MergeCRs mergedCRS) followupP
            (*
                In order to merge the Done-Lists we need to
                - extract the participants resulting from the new entities
                - add those participants to the list of CRs
                - store, that the current participant completed all ids of the current search
                    AND all IDs of the entities in the reply.
                    Note, that the current participant is already in the doneIDs
            *)

            let mergedAll_new =
                mergedAll @ followupE //already distinct

            (searchAgain_new, mergedE_new, mergedCRS_new, mergedAll_new)

            ) (false, result, crs, allIDs)
        |> function
            | (false, mergedE, _, _) -> mergedE
            | (true, mergedE, mergedDone, mergedAll) -> this.ExecuteCompleteSearch mergedE mergedDone direction mergedAll
        (*
            doneIDs has the same purpose as in ExecuteSearchLocal. However, it is by participant here: (participant-ID, participant-URL, [done Entities])
            We do not maintain openIDs when we go down recursively but allIDs.
            That's because participants might be added in subsequent searches and we want to ask them for all the entities.
        
            We do not have a public SearchRemote because ExecuteSearchRemote follows a local search
            whenever the Search function is called and it needs preperation since it needs at least one participant URL to start with.

            Note:A RemoteSearch always comprises a local search as long as we are among the participant URLs
            We do not exeplicitly execute a local search between the remote-searches: If the entities indicate that we might now something,
            we ourselves will be in the participantURLs and execute a request to our own URL. That's perfectly fine since this will never hold
            the process up - a localhost request can be expected to be the fastest.

            ToDo: As written right now here, (among other things) it will never bottom out if a malious participant keeps 
            adding entities or other participants.
        *)

    interface IEntityService with

        member this.LocalDownchainSearch = this.ExecuteLocalDownchainSearch [] []

        member this.LocalUpchainSearch = fun (entityIDs:string list) ->
            entityIDs
            |> els.GetEntitiesLocal
            |> fun initials -> this.ExecuteLocalUpchainSearch [] initials entityIDs
        

        member this.CompleteSearch = fun (direction:Direction) (memberID:string option) (entityIDs:string list) ->
            match (memberID, ms.ThisInstance) with
            | (None, None) -> []
            | (None, Some ti ) ->
                this.ExecuteCompleteSearch [] [
                    {MemberID=ti.ID; Member=Some(ti); Entities=[]}
                ] direction entityIDs
            | (Some pID, None) ->
                this.ExecuteCompleteSearch [] [
                    {MemberID=pID; Member=None; Entities=[]}
                ] direction entityIDs
            | (Some pID, Some ti) ->
                this.ExecuteCompleteSearch [] [
                    {MemberID=ti.ID; Member=Some(ti); Entities=[]};
                    {MemberID=pID; Member=None; Entities=[]}
                ] direction entityIDs