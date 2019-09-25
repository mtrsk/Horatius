// Learn more about F# at http://fsharp.org

open System
open System.IO
open Argu

open Verification

type CliError =
    | WrongArguments

// TODO: Add more arguments for the CLI
type CLIArguments =
    | All
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | All -> "Parses both files and creates new ones."

let getExitCode result =
    match result with
    | Ok () -> 0
    | Error err ->
        match err with
        | WrongArguments -> 1

let runAll (directory: string) =
    let filepath1 = Path.Combine(directory, "matriculasSemDV.txt")
    printfn "Loading File: %s" filepath1
    Validation.writeVDFile filepath1
    printfn "File written in the current directory"

    let filepath2 = Path.Combine(directory, "matriculasParaVerificar.txt")
    printfn "Loading File: %s" filepath2
    Validation.writeVerifiedFile filepath2
    printfn "File written in the current directory"

    Ok ()


// Directory containing the enrollments text files
[<Literal>]
let dir = "../../files"

[<EntryPoint>]
let main argv =
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)
    let parser = ArgumentParser.Create<CLIArguments>(programName = "horatius", errorHandler = errorHandler)
    match parser.ParseCommandLine argv with
    | arg when arg.Contains(All) -> runAll dir
    | _ ->
        printfn "%s" (parser.PrintUsage())
        Error WrongArguments
    |> getExitCode
