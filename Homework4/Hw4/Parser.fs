module Hw4.Parser

open System
open Hw4.Calculator


type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let isArgLengthSupported (args : string[]) =
    args.Length = 3

let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> ArgumentException() |> raise
    
let parseValue (s:string) =
    match Double.TryParse s with
    | true, num -> num
    | _ -> ArgumentException() |> raise
    
let parseCalcArguments(args : string[]) =
    match isArgLengthSupported args with
    | false -> ArgumentException() |> raise
    | true ->
        {
            arg1 = parseValue args[0]
            arg2 = parseValue args[2]
            operation = parseOperation args[1] 
        }


    