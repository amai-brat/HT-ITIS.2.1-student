module Hw5.Parser

open System
open System.Globalization
open Hw5.Calculator
open Hw5.MaybeBuilder

let isArgLengthSupported (args:string[]): Result<string[], Message> =
    match args.Length with
    | 3 -> Ok args
    | _ -> Error Message.WrongArgLength
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | PLUS -> Ok (arg1, CalculatorOperation.Plus, arg2)
    | MINUS -> Ok (arg1, CalculatorOperation.Minus, arg2)
    | MULTIPLY -> Ok (arg1, CalculatorOperation.Multiply, arg2)
    | DIVIDE -> Ok (arg1, CalculatorOperation.Divide, arg2)
    | _ -> Error Message.WrongArgFormatOperation

let parseArgs (args: string[]): Result<(double * CalculatorOperation * double), Message> =
    // Без CultureInfo парсятся числа только с десятичной запятой, а в тестах точка
    match Double.TryParse(args[0], NumberStyles.Number, CultureInfo.InvariantCulture) with
    | false, _ -> Error Message.WrongArgFormat
    | true, val1 ->
        match Double.TryParse(args[2], NumberStyles.Number, CultureInfo.InvariantCulture) with
        | false, _ -> Error Message.WrongArgFormat
        | true, val2 -> isOperationSupported (val1, args[1], val2)

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match (operation, arg2) with
    | CalculatorOperation.Divide, 0.0 -> Error Message.DivideByZero
    | _ -> Ok (arg1, operation, arg2)
    
let parseCalcArguments (args: string[]): Result<('a * CalculatorOperation * 'b), Message> =
    maybe {
        let! rightArgsLength = isArgLengthSupported args
        let! parsed = parseArgs rightArgsLength
        let! noDividingByZero = isDividingByZero parsed
        return noDividingByZero
    } 