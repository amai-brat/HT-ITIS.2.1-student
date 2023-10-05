open Hw4
    
[<EntryPoint>]
let main args = 
    let calcOptions = Parser.parseCalcArguments args
    printfn $"{Calculator.calculate calcOptions.arg1 calcOptions.operation calcOptions.arg2}"
    0
    
