module Hw6.Parser

open System
open System.Globalization
open Hw6.MaybeBuilder
open Microsoft.AspNetCore.Http

let isArgLengthSupported (request: HttpRequest): Result<HttpRequest, (Message * string)> =
    match request.Query.Count with
    | 3 -> Ok request
    | _ -> Error (Message.WrongArgLength, request.Query.Count.ToString())

let areQueryParametersCorrect (request: HttpRequest): Result<(string * string * string), (Message * string)> =
    match request.Query.TryGetValue "value1" with
    | false, _ -> Error (Message.NotFoundQueryParameter, "value1")
    | true, arg1 ->
        match request.Query.TryGetValue "operation" with
        | false, _ -> Error (Message.NotFoundQueryParameter, "operation")
        | true, operation ->
            match request.Query.TryGetValue "value2" with
            | false, _ -> Error (Message.NotFoundQueryParameter, "value2")
            | true, arg2 -> Ok (arg1.ToString(), operation.ToString(), arg2.ToString())
            
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), (Message * string)> =
    match operation with
    | "Plus" -> Ok (arg1, CalculatorOperation.Plus, arg2)
    | "Minus" -> Ok (arg1, CalculatorOperation.Minus, arg2)
    | "Multiply" -> Ok (arg1, CalculatorOperation.Multiply, arg2)
    | "Divide" -> Ok (arg1, CalculatorOperation.Divide, arg2)
    | _ -> Error (Message.WrongArgFormatOperation, operation)

let parseArgs (arg1: string, operation: string, arg2: string): Result<(double * CalculatorOperation * double), (Message * string)> =
    match Double.TryParse(arg1, NumberStyles.Number, CultureInfo.InvariantCulture) with
    | false, _ -> Error (Message.WrongArgFormat, arg1)
    | true, val1 ->
        match Double.TryParse(arg2, NumberStyles.Number, CultureInfo.InvariantCulture) with
        | false, _ -> Error (Message.WrongArgFormat, arg2)
        | true, val2 -> isOperationSupported (val1, operation, val2)
    
let parseCalcArguments (request: HttpRequest): Result<('a * CalculatorOperation * 'b), (Message * string)> =
    maybe {
        let! rightArgsLength = isArgLengthSupported request
        let! rightQueryParameters = areQueryParametersCorrect rightArgsLength
        let! parsed = parseArgs rightQueryParameters
        return parsed
    }