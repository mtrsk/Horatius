// Learn more about F# at http://fsharp.org

open System
open System.IO

open Verification

// Directory containing the enrollments text files
[<Literal>]
    let dir = "../../files"

[<EntryPoint>]
let main argv =
    let filepath1 = Path.Combine(dir, "matriculasSemDV.txt")
    Validation.writeVDFile filepath1

    let filepath2 = Path.Combine(dir, "matriculasParaVerificar.txt")
    Validation.writeVerifiedFile filepath2
    0 // return an integer exit code
