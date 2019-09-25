namespace Verification

module Validation =

    open System
    open System.IO
    open System.Text.RegularExpressions

    let inline charToInt c = int c - int '0'

    let validate (x : string) : seq<int> option =
        // Can this string be converted to a valid sequence of integers?
        // If so, then wrap that value into an sucess Option (Some),
        // othewise return None (which is a 'Failure'-like state), there's
        // no need to crash the program because of bad data.
        try
            x
            |> Seq.map(charToInt)
            |> Some
        with :? FormatException ->
            None

    let parseStrToDigits (enrollment : string) : seq<int> =
        // Tries to validate a string and unwraps it into two possible
        // values:
        //   - seq<int>, the string is parsed into a sequence of integers.
        //   - {}      , an empty sequence that is ignored later.
        validate enrollment
        |> (fun validated -> match validated with
            | Some value -> value
            | None       -> Seq.empty)

    let computeVerificationDigit (enrollment : seq<int>) : char =
        // Takes a seq<int> as an argument, which represents a single
        // enrollment number (e.g. seq {0; 8; 0; 4}) and calculates a
        // verification digit for it in hexadecimaml format.

        // Starts at 5 and keeps subtracting 1 until it gets 2, i.e.
        // generates the {5,4,3,2} weights.
        let weights = seq {5 .. -1 .. 2}

        // Seq comprehension, takes the i-th value from the enrollment container
        // and multiplies it to the i-th value of the weights container.
        let result = seq { for (x,y) in Seq.zip enrollment weights do yield x * y }

        // The computation of the verification digit is divided into 3 steps
        //   - First, all elements of the 'result' are summed.
        //   - Take the remainder of the sum modulus 16.
        //   - Then Return either a number from 0 - 9 (zero starts at 48 in ascii)
        //     or a character from 'A' to 'F' ('A' starts at 65).
        result
        |> Seq.sum
        |> (fun x -> x % 16)
        |> (fun x -> if x < 10 then char <| x + 48 else char <| x + 55)

    let appendDigit(enrollment : seq<int>) : seq<string> =
        // Takes a sequence of intergers, converts to a sequence of
        // strings and appends the verification digit to it.

        // Strings are easier to deal with, so convert char -> string.
        let digit = (computeVerificationDigit enrollment) |> string
        let newEnrollment = enrollment |> Seq.map(string)

        // Append the value '-[0-9A-F]' to the sequence. Seq.append returns
        // a new sequence, since this is the last expression of the function,
        // it's also it's return value.
        Seq.append newEnrollment ([| "-"; digit |] :> seq<string>)

    let reformat(enrollment : seq<int>) : string =
        // Takes a sequence of digits and:
        //   - Appends the verification digit
        //   - Converts the sequence to a list of strings
        //   - The list is then parsed as a string
        appendDigit enrollment
        |> Seq.toList
        |> String.concat ""

    let rename(filename: string) : string =
        // Takes a filename as a input, returns a new filename that it's either
        //   - {matriculas}ComDV{.txt}
        //   - {matriculas}Verificadas{.txt}

        // Matches filename with (\w*.txt$), which matches any sequences of
        // [a-zA-Z0-9_] that ends with '.txt'.
        let pattern = Regex.Match(filename, "(\w*.txt$)")

        // TODO: Since the files are hardcoded, there's no need to garantee
        // the pattern really matches something (it always does). If this
        // happens to change, then a refactor is necessary.

        // holds the values ['matriculas', '.txt']
        let newFilename = [pattern.Value.[0 .. 9]; ".txt"]

        match filename.Contains("ParaVerificar") with
        | true -> newFilename |> String.concat "Verificadas"
        | false -> newFilename |> String.concat "ComDV"

    let verificationStream(enrollments: seq<string>) : seq<string> =
        // Takes a sequence of strings and:
        //    - Converts to a sequence of digits.
        //    - Ignores invalid strings (empty sequences).
        //    - Applies the algorithm and apppends the
        //      verification digits.
        enrollments
        |> Seq.map(parseStrToDigits)
        |> Seq.filter(fun x -> x <> Seq.empty)
        |> Seq.map(reformat)

    let writeVDFile(filepath: string) : unit =
        // TODO: File.ReadLines is a simple solution, but has performance hits on
        // larger files, it works fine for now.
        let enrollmentList = (File.ReadLines filepath) |> verificationStream

        File.WriteAllLines(filepath |> rename, enrollmentList)

    let parseVerifiedString(vstring: string []) : string [] =
        // Appends a truth value to the input. For instance:
        //   [|"0902"; "8"|] -> [|"0902"; "8"; "verdadeiro"|]
        //   [|"1790"; "D"|] -> [|"1790"; "D"; "falso"|]
        let value = vstring.[0] |> parseStrToDigits
        let digit = (computeVerificationDigit value) |> string

        // If the computed digit does not match the given label,
        // then append 'false', otherwise 'true'.
        if digit <> vstring.[1] then
            Array.append vstring [| "falso" |]
        else
            Array.append vstring [| "verdadeiro" |]

    let checkStream(enrollments: seq<string>) : seq<string> =
        // Takes a sequence of strings and:
        //    - Splits the string into an array, using the '-' as delimiter.
        //    - Takes the array and appends the truth value.
        //    - Formats the output array as <num>-<[0-9A-F]> <truth value>
        enrollments
        |> Seq.map(fun x -> x.Split([| "-" |], System.StringSplitOptions.None))
        |> Seq.map(parseVerifiedString)
        |> Seq.map(fun (x: string []) -> x.[0] + "-" + x.[1] + " " + x.[2])

    let writeVerifiedFile(filepath: string) : unit =
        let enrollmentList = (File.ReadLines filepath) |> checkStream

        File.WriteAllLines(filepath |> rename, enrollmentList)
