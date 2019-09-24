namespace App

[<AutoOpen>]
module AvaloniaExtensions =
    open Avalonia.Markup.Xaml.Styling
    open System
    open Avalonia.Styling

    type Styles with
        member this.Load (source: string) =
            let style = new StyleInclude(baseUri = null)
            style.Source <- new Uri(source)
            this.Add(style)
