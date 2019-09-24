module Tests

open System
open Xunit

open Verification

[<Fact>]
let ``rename correctly changes the correct file`` () =
    let input1 = "matriculasSemDV.txt"
    let expected1 = "matriculasComDV.txt"
    let actual1 = Validation.rename input1
    Assert.Equal<string>(expected1, actual1)

    let input2 = "matriculasParaVerificar.txt"
    let expected2 = "matriculasVerificadas.txt"
    let actual2 = Validation.rename input2
    Assert.Equal<string>(expected2, actual2)

[<Fact>]
let ``computeVerification correctly computes a digit given a sequence`` () =
    let input = Validation.parseStrToDigits "9369"
    let expected : char = 'D'
    let actual : char = Validation.computeVerificationDigit input

    Assert.Equal<char>(expected, actual)

[<Fact>]
let ``verificationStream correctly computes the digits of sequences`` () =
    let input : seq<string> = seq ["0822"; "1988"; "3186"; "4573"]
    let expected : seq<string> = seq ["0822-A"; "1988-1"; "3186-7"; "4573-3"]
    let actual : seq<string> = Validation.verificationStream input

    Assert.Equal<Collections.Generic.IEnumerable<string>>(expected, actual)

[<Fact>]
let ``parseVerifiedString correctly applies the algorithm to generate digits`` () =
    // Also, this verifies if the example from the functions comment
    // is still relevant.

    let input1 = [|"0902"; "8"|]
    let expected1 = [|"0902"; "8"; "verdadeiro"|]
    let actual1 = Validation.parseVerifiedString input1
    Assert.Equal<Collections.Generic.IEnumerable<string>>(expected1, actual1)

    let input2 = [|"1790"; "D"|]
    let expected2 = [|"1790"; "D"; "falso"|]
    let actual2 = Validation.parseVerifiedString input2
    Assert.Equal<Collections.Generic.IEnumerable<string>>(expected2, actual2)

[<Fact>]
let ``checkStream correctly verifies the sequences`` () =
    let input : seq<string> = seq ["0902-8"; "0924-2"; "1790-D"; "5289-C"; "5339-0"]
    let expected : seq<string> = seq [
        "0902-8 verdadeiro";
        "0924-2 verdadeiro";
        "1790-D falso";
        "5289-C falso";
        "5339-0 verdadeiro"
    ]

    let actual : seq<string> = Validation.checkStream input

    Assert.Equal<Collections.Generic.IEnumerable<string>>(expected, actual)
