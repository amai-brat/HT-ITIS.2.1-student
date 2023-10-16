open System
open System.Net.Http
open Microsoft.FSharp.Control

let convertOperation operation =
    match operation with
    | "+" -> "Plus"
    | "-" -> "Minus"
    | "*" -> "Multiply"
    | "/" -> "Divide"
    | smth -> smth
    
let rec handleInput(input: string) =
    let client = new HttpClient()
    
    match input with
    | "" -> ()
    | _ ->
        let args = input.Split(' ')
        match args.Length with
        | 3 ->
            async {
                let url = "http://localhost:5000/calculate?" + 
                            $"value1={args[0]}&" +
                            $"operation={args[1] |> convertOperation}&" +
                            $"value2={args[2]}"
                let! response = client.GetAsync url |> Async.AwaitTask
                let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                printfn $"{content}"
            } |> Async.RunSynchronously
        | _ ->
            printfn "Неправильное количество аргументов"
            
        handleInput (Console.ReadLine())

        
[<EntryPoint>]
let main _ =
    handleInput (Console.ReadLine())
    0
