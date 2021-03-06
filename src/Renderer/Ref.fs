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

// Register format button
let registerFormat (id: int) (format: string): HTMLButtonElement =
    Browser.document.getElementById(sprintf "R%i-%s" id format) :?> HTMLButtonElement

// Format all registers button
let registerFormatAll (format: string): HTMLButtonElement =
    Browser.document.getElementById(sprintf "registers-%s" format) :?> HTMLButtonElement

// Randomise all registers button
let registerRandomiseAll: HTMLButtonElement =
    Browser.document.getElementById("registers-randomise") :?> HTMLButtonElement

// Register group containing all fields for a register
let registerGroup (id: int): HTMLElement =
    Browser.document.getElementById(sprintf "R%i-group" id)

// Top level buttons on register panel
let registerTop : HTMLElement = 
    Browser.document.getElementById("register-top")

// ------ Begin panel buttons -----
let memoryPanel: HTMLButtonElement = 
    Browser.document.getElementById("memory") :?> HTMLButtonElement

let registerPanel: HTMLButtonElement = 
    Browser.document.getElementById("registers") :?> HTMLButtonElement

let labelPanel: HTMLButtonElement = 
    Browser.document.getElementById("labels") :?> HTMLButtonElement
// ----- End panel buttons ------

// The table containing labels
let labelsTable: HTMLElement = 
    Browser.document.getElementById("labels-table")

let labelsTableBody: HTMLElement = 
    Browser.document.getElementById("labels-table-body")

// The table containing memory contents
let memoryTable: HTMLElement = 
    Browser.document.getElementById("memory-table")

let memoryTableBody: HTMLElement = 
    Browser.document.getElementById("memory-table-body")

let newCode: HTMLButtonElement =
    Browser.document.getElementById("newCode") :?> HTMLButtonElement

let openFile: HTMLButtonElement =
    Browser.document.getElementById("open") :?> HTMLButtonElement

let save: HTMLButtonElement =
    Browser.document.getElementById("save") :?> HTMLButtonElement

let run: HTMLButtonElement =
    Browser.document.getElementById("run") :?> HTMLButtonElement

let emulator: HTMLElement = 
    Browser.document.getElementById("emulator-details")

let resetRun: HTMLButtonElement = 
    Browser.document.getElementById("reset-run") :?> HTMLButtonElement

let reset: HTMLButtonElement = 
    Browser.document.getElementById("reset") :?> HTMLButtonElement

let flag (id: string): HTMLElement =
    Browser.document.getElementById(sprintf "flag_%s" id)

let code: unit -> string = fun _ ->
    window?code?getValue() :?> string
