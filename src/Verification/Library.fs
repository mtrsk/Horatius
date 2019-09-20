namespace Verification

module Validation =

  open System
  open System.IO

  let inline charToInt c = int c - int '0'

  let validate (x : string) =
    try
      x
      |> Seq.map(charToInt)
      |> Some
    with :? FormatException ->
      None

  let parseStrToDigits (x : string) =
    validate x
    |> (fun validated -> match validated with
        | Some value -> value
        | None       -> Seq.empty)

  let computeVerificationDigit (enrollment : seq<int>) =
    (* Takes a seq<int> as an argument, which represents a single
      enrollment number (e.g. {0; 8; 0; 4}) and calculates a veri-
      fication digit for it in hexadecimaml format.*)

    // Starts at 5 and keeps subtracting 1 until it gets 2, i.e.
    // generates the {5,4,3,2} weights.
    let weights = seq {5 .. -1 .. 2}
    // Seq comprehension, takes the i-th value from the enrollment container
    // and multiplies to the i-th value of the weights container.
    let result = seq { for (x,y) in Seq.zip enrollment weights do yield x * y }

    result
    |> Seq.sum
    |> (fun x -> x % 16)
    |> (fun x -> if x < 10 then char <| x + 48 else char <| x + 55)

  let appendDigit(enrollment : seq<int>) =
    let digit = (computeVerificationDigit enrollment) |> string
    let newEnrollment = enrollment |> Seq.map(string)

    Seq.append newEnrollment ([| "-"; digit |] :> seq<string>)

  let reformat(enrollment : seq<int>) =
    appendDigit enrollment
    |> Seq.toList
    |> String.concat ""

  let parseFile(filepath : string) =
    (File.ReadLines filepath)
    |> Seq.map(parseStrToDigits)
    |> Seq.filter(fun x -> x <> Seq.empty)
    |> Seq.map(reformat)
