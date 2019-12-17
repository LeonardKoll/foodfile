namespace FoodFile

open System

type Result<'TSuccess,'TFailure> = 
    | Success of 'TSuccess
    | Failure of Exception


module Railway =

    let switch = fun simpleFun input -> 
        match input with
        | Success s -> 
            try
                simpleFun s
                |> Success
            with e ->
                Failure e               // When the wrappeeFunction raises an exception
        | Failure e -> Failure e        // When the input was a Failure already.
   
       (*
    let railwayFun = fun (railwayFun:_->Result<'TSuccess,'TFailure>) input -> 
        match input with
        | Success s -> 
            try
                railwayFun s
            with e ->
                Failure e               // When the wrappeeFunction raises an exception
        | Failure e -> Failure e        // When the input was a Failure already.
        *)

    let startWith = fun value -> Success value