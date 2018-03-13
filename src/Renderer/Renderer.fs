(*
    High Level Programming @ Imperial College London # Spring 2018
    Project: A user-friendly ARM emulator in F# and Web Technologies ( Github Electron & Fable Compliler )
    Contributors: Angelos Filos
    Module: Main
    Description: Electron Renderer Process - Script to executed after `index.html` is loaded.
*)

module Renderer

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Electron
open Node.Exports
open Fable.PowerPack

open Fable.Import.Browser

// open DevTools to see the message
// Menu -> View -> Toggle Developer Tools
Browser.console.log "Hi from the renderer.js" |> ignore

open Ref
open Update
open ArmEmulator

/// Access to `Emulator` project
/// ------------- CHANGE THIS --------------
///let dummyVariable = Emulator.Common.A


// Adds event listener for every register format button
let listMapper (reg,format) =
    (Ref.registerFormat reg format).addEventListener_click(fun _ ->
        Update.registerFormat reg 123 format
    )

// Dec, hex and bin for input register number
let regFormatCombinations (reg:int) = 
    [reg,"dec"; reg,"hex"; reg,"bin"]


/// Initialization after `index.html` is loaded
let init () =
    Ref.fontSize.addEventListener_change(fun _ ->
        let size: int =
            // TODO: error-prone, hardcoded index
            // of word "Font Size: xx" to slice
            Ref.fontSize.value.[11..]
            |> int
        Browser.console.log "Font size updated" |> ignore
        Update.fontSize size
    )
    // TODO: Implement actions for the buttons
    Ref.newCode.addEventListener_click(fun _ ->
        Update.code("")
    )
    Ref.save.addEventListener_click(fun _ ->
        Browser.window.alert (sprintf "%A" (Ref.code ()))
    )
    Ref.run.addEventListener_click(fun _ ->
        Browser.window.alert "NotImplemented :|"
    )
    // just for fun!
    (Ref.register 0).addEventListener_click(fun _ ->
        Browser.console.log "register R0 changed!" |> ignore
        Update.register 0 (System.Random().Next 1000)
    )
    (Ref.flag "N").addEventListener_click(fun _ ->
        Browser.console.log "flag N changed!" |> ignore
        Update.flag "N" true
    )

    // List.map for all register formating (dec, bin, hex)
    List.collect regFormatCombinations [0..15]
    |> List.map listMapper 
    |> List.iter id



init()
