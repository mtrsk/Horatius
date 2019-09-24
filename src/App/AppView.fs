namespace App

open Avalonia.Controls
open Avalonia.Media
open Avalonia.FuncUI.Types
open Avalonia.FuncUI
open Avalonia.Layout

open System.IO
open Verification


module FileGeneratorView =

    // The model holds data that you want to keep track of while the application is running
    type ListState = {
        filepath : string
        list : string list
    }

    //The initial state of of the application
    let initialState = {
        filepath = ""
        list = []
    }

    // The Msg type defines what events/actions can occur while the application is running
    // the state of the application changes *only* in reaction to these events
    type Msg =
    | GenerateVDFile
    | VerifyFile

    // The update function computes the next state of the application based on the current state and the incoming messages
    let update (msg: Msg) (state: ListState) : ListState =
        match msg with
        | GenerateVDFile ->
            let path = "matriculasSemDV.txt"
            { filepath = path
              list = (File.ReadLines path)
                |> Validation.verificationStream
                |> Seq.toList
            }
        | VerifyFile     ->
            let path = "matriculasParaVerificar.txt"
            { filepath = path
              list = (File.ReadLines path)
                |> Validation.checkStream
                |> Seq.toList
            }

    // The view function returns the view of the application depending on its current state. Messages can be passed to the dispatch function.
    let viewElement (element : string) : View =
        Views.textBlock [
            Attrs.verticalAlignment VerticalAlignment.Center
            Attrs.horizontalAlignment HorizontalAlignment.Center
            Attrs.text element
            Attrs.fontSize 12.0
        ]

    let view (state: ListState) (dispatch): View =
        Views.dockpanel [
            Attrs.children [
                Views.button [
                    Attrs.dockPanel_dock Dock.Bottom
                    Attrs.onClick (fun sender args -> dispatch GenerateVDFile)
                    Attrs.content "Generate Verified Digits"
                ]
                Views.button [
                    Attrs.dockPanel_dock Dock.Bottom
                    Attrs.onClick (fun sender args -> dispatch VerifyFile)
                    Attrs.content "Verify File"
                ]
                Views.viewLazy state.list dispatch (fun elements dispatch ->
                    Attrs.dockPanel_dock Dock.Top
                    Views.stackPanel [
                        Attrs.children [
                            for element in elements do
                                yield viewElement element
                        ]
                    ]
                )
            ]
        ]
