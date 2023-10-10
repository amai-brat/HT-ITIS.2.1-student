open System
open Hw5

[<EntryPoint>]
let main (args:string[]) =
    let parsedArgs = Parser.parseCalcArguments args
    match parsedArgs with
    | Ok (val1, operation, val2) ->
        printfn $"{Calculator.calculate val1 operation val2}"
    | Error errorValue ->
        printfn $"{errorValue}"
    0
