namespace FoodFile

// DUMMY

(*
    This module retrieves participant data from FoodFile.org
    and handles in-Memory Caching (later)
*)

module Members =

    let freshfruitfarmers = {ID="IK7TEO"; Name=Some("FreshFruitFarmers"); API=Some("https://localhost:5001/api/local/")}
    let sugarsilo = {ID="UC2NRQ"; Name=Some("SugarSilo"); API=Some("https://localhost:5001/api/local/")}
    let yummyjam = {ID="8X55N4"; Name=Some("YummyJam"); API=Some("https://localhost:5001/api/local/")}
    let members = [freshfruitfarmers;sugarsilo;yummyjam]

    let GetMembers = fun (memberIDs:string list) ->
        members
        |> List.filter ( fun elem1 -> List.exists (fun elem2 -> elem1.ID=elem2 ) memberIDs )

    let GetMemberAPIs = fun (memberIDs:string list) ->
        memberIDs
        |> GetMembers
        |> List.map ( fun p -> p.API )
        |> List.zip memberIDs

    // This instance can also be pure Trace. Then we do not have a participant ID.
    let thisInstance = Some({ID="2VIJP2"; Name=Some("RealRetail"); API=Some("https://localhost:5001/api/local/")})

    let ExtractMembers = fun (entities:Entity list) ->
        entities
        |> List.fold ( fun (stateA:string list) entity -> 
            entity.Atoms
            |> List.fold (fun (stateB:string list) atom ->
            
                let mA = match atom.Information with
                    | Creation x -> Some(x.Responsible)
                    | Transfer x -> Some(x.Responsible)
                    | _ -> None
                
                let mB = 
                    List.map ( fun c -> c.Signer) atom.Signatures

                match mA with
                | Some (Some m) -> m::(mB@stateB)
                | _             -> mB@stateB

                ) stateA
            ) []
        |> List.distinct
        |> GetMembers