// Learn more about F# at http://fsharp.org

open System
open Verification

[<EntryPoint>]
let main argv =
    let filepath = "matriculas.txt"

    Validation.parseFile filepath
    |> Seq.iter(fun x -> printfn "%A" x)

    0 // return an integer exit code
