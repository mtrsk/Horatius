// Learn more about F# at http://fsharp.org

open System

open Verification

[<EntryPoint>]
let main argv =
    let filepath1 = "matriculasSemDV.txt"
    Validation.writeVDFile filepath1

    let filepath2 = "matriculasParaVerificar.txt"
    Validation.writeVerifiedFile filepath2
    0 // return an integer exit code
