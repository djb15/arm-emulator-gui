(*
    High Level Programming @ Imperial College London # Spring 2018
    Project: A user-friendly ARM emulator in F# and Web Technologies ( Github Electron & Fable Compliler )
    Contributors: Angelos Filos
    Module: Ref
    Description: References to `HTML` elements from `index.html`.
*)

module Ref

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser

let fontSize: HTMLSelectElement =
    Browser.document.getElementById("font-size") :?> HTMLSelectElement
let register (id: int): HTMLElement =
    Browser.document.getElementById(sprintf "R%i" id)

// Register formatting
let registerFormat (id: int) (format: string): HTMLButtonElement =
    Browser.document.getElementById(sprintf "R%i-%s" id format) :?> HTMLButtonElement

// Format all registers button
let registerFormatAll (format: string): HTMLButtonElement = 
    Browser.document.getElementById(sprintf "registers-%s" format) :?> HTMLButtonElement

// New file button
let newCode: HTMLButtonElement =
    Browser.document.getElementById("newCode") :?> HTMLButtonElement

let explore: HTMLButtonElement =
    Browser.document.getElementById("explore") :?> HTMLButtonElement
let save: HTMLButtonElement =
    Browser.document.getElementById("save") :?> HTMLButtonElement
let run: HTMLButtonElement =
    Browser.document.getElementById("run") :?> HTMLButtonElement
let flag (id: string): HTMLElement =
    Browser.document.getElementById(sprintf "flag_%s" id)
let code: unit -> string = fun _ ->
    window?code?getValue() :?> string
