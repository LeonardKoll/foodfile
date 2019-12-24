namespace FoodFile

open System

type Result<'TSuccess,'TFailure> = 
    | Success of 'TSuccess
    | Failure of 'TFailure


module ResultPipeline =

    // value Wrapper
    let inline startWith value = Success value

    // Function Wrapper
    let wrap = fun simpleFunction (input:Result<'TSuccess, 'TFailure>) ->
        match input with
        | Success s -> 
            try
                simpleFunction s
                |> Success
            with e ->
                Failure e               // When the wrappeeFunction raises an exception
        | Failure f -> Failure f        // When the input was a Failure already.
    
    // Regular Executeor
    let execute = fun (pipelineFunction:_->Result<'TSuccess,'TFailure>) (input:Result<'TSuccess, 'TFailure>) -> 
        match input with
        | Success s -> pipelineFunction s
        | Failure f -> Failure f

    // Infix
    let (>=>) = execute