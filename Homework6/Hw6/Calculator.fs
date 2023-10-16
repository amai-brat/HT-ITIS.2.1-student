module Hw6.Calculator

open Microsoft.AspNetCore.Http

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let convertMessageToString (message: (Message * string)): string =
    match message with
    | Message.WrongArgLength, amount -> $"Wrong amount of arguments: {amount}. Needed: 3"
    | Message.WrongArgFormat, arg -> $"Could not parse value '{arg}'"
    | Message.WrongArgFormatOperation, operation -> $"Could not parse value '{operation}'"
    | Message.NotFoundQueryParameter, param -> $"Could not find parameter '{param}'"
    | _ -> ""

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline calculate (request: HttpRequest): Result<string, string> =
    let parsed = Parser.parseCalcArguments request
    match parsed with
    | Error error -> Error (error |> convertMessageToString)
    | Ok (value1, operation, value2) -> 
        match operation with
        | CalculatorOperation.Plus -> Ok $"{value1 + value2}"
        | CalculatorOperation.Minus -> Ok $"{value1 - value2}"
        | CalculatorOperation.Multiply -> Ok $"{value1 * value2}"
        | CalculatorOperation.Divide ->
            match value2 with
            | 0.0 -> Ok "DivideByZero"
            | _ -> Ok $"{value1 / value2}"
        | _ -> Error "Something went wrong"
