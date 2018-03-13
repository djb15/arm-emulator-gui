(*
    High Level Programming @ Imperial College London # Spring 2018
    Project: A user-friendly ARM emulator in F# and Web Technologies ( Github Electron & Fable Compliler )
    Contributors: Angelos Filos
    Module: Renderer.Update
    Description: Event helper functions for `HTML` elements in `index.html`.
*)

module Update

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

open Ref

let fontSize (size: int) =
    let options = createObj ["fontSize" ==> size]
    window?code?updateOptions options

// Change register value to selected format
let registerFormat (id: int) (value: int) (format: string) =
    let el = Ref.register id
    
    let hexButton = Ref.registerFormat id "hex"
    let decButton = Ref.registerFormat id "dec"
    let binButton = Ref.registerFormat id "bin"
    
    match format with
    | "dec" ->
        hexButton.setAttribute("class", "btn btn-enc")
        decButton.setAttribute("class", "btn btn-enc target")
        binButton.setAttribute("class", "btn btn-enc")
        el.innerHTML <- sprintf "%i" value
    | "hex" ->
        hexButton.setAttribute("class", "btn btn-enc target")
        decButton.setAttribute("class", "btn btn-enc")
        binButton.setAttribute("class", "btn btn-enc")
        el.innerHTML <- sprintf "0x%X" value
    | _ ->
        let rec intToBinary i =
            match i with
            | 0 | 1 -> string i
            | _ ->
                let bit = string (i % 2)
                (intToBinary (i / 2)) + bit
        
        hexButton.setAttribute("class", "btn btn-enc")
        decButton.setAttribute("class", "btn btn-enc")
        binButton.setAttribute("class", "btn btn-enc target")
        el.innerHTML <- sprintf "0b%s" (intToBinary value)


let registerFormatAll (format: string) = 
    let hexButton = Ref.registerFormatAll "hex"
    let decButton = Ref.registerFormatAll "dec"
    let binButton = Ref.registerFormatAll "bin"

    match format with
    | "dec" ->
        hexButton.setAttribute("class", "btn btn-enc btn-enc-top")
        decButton.setAttribute("class", "btn btn-enc btn-enc-top target")
        binButton.setAttribute("class", "btn btn-enc btn-enc-top")
    | "hex" ->
        hexButton.setAttribute("class", "btn btn-enc btn-enc-top target")
        decButton.setAttribute("class", "btn btn-enc btn-enc-top")
        binButton.setAttribute("class", "btn btn-enc btn-enc-top")
    | _ ->
        hexButton.setAttribute("class", "btn btn-enc btn-enc-top")
        decButton.setAttribute("class", "btn btn-enc btn-enc-top")
        binButton.setAttribute("class", "btn btn-enc btn-enc-top target")


let flag (id: string) (value: bool) =
    let el = Ref.flag id
    match value with
        | false ->
            el.setAttribute("style", "background: #fcfcfc")
            el.innerHTML <- sprintf "%i" 0
        | true ->
            el.setAttribute("style", "background: #4285f4")
            el.innerHTML <- sprintf "%i" 1
let code (text: string) =
    window?code?setValue(text)
