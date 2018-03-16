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
open Emulator

/// Access to `Emulator` project
/// ------------- CHANGE THIS --------------
///let dummyVariable = Emulator.Common.A

// System wide clipboard
let clipboard : obj = importMember "electron"

// Adds event listener for every register format button
let listMapper (reg,format) =
    (Ref.registerFormat reg format).addEventListener_click(fun _ ->
        Update.registerFormat reg 123 format
    )

// Dec, hex and bin for input register number
let regFormatCombinations (reg:int) = 
    [reg,"dec"; reg,"hex"; reg,"bin"]


// Clipboard for all registers mapping function
let regClipboardAccess (reg:int) = 
    let register = Ref.register reg
    register.addEventListener_click(fun _ ->   
        clipboard?writeText(register.innerHTML) |> ignore
        Browser.window.alert(sprintf "Copied R%i" reg)
    )

let regsFormatAll format = 
    let updateReg reg = 
        Update.registerFormat reg 123 format

    (Ref.registerFormatAll format).addEventListener_click(fun _ ->
        Update.registerFormatAll format
        List.map updateReg [0..15]
        |> List.iter id       
    )


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

    (Ref.flag "N").addEventListener_click(fun _ ->
        Browser.console.log "flag N changed!" |> ignore
        Update.flag "N" true
    )

    // List.map for all register formating (dec, bin, hex)
    List.collect regFormatCombinations [0..15]
    |> List.map listMapper 
    |> List.iter id

    // Clipboard access for all registers
    List.map regClipboardAccess [0..15]
    |> List.iter id

    // Format all registers in dec/hex/bin format
    List.map regsFormatAll ["dec";"bin";"hex"]
    |> List.iter id

    // Generate random numbers up to 30,000 just for demonstration (excluding SP, LR, PC)
    Ref.registerRandomiseAll.addEventListener_click(fun _ ->
        let randomUpdate reg = 
            Update.registerFormat reg (System.Random().Next 30000) "hex"
        
        List.map randomUpdate [0..12]
        |> List.iter id
    )

init()
