(*
    High Level Programming @ Imperial College London # Spring 2018
    Project: A user-friendly ARM emulator in F# and Web Technologies ( Github Electron & Fable Compliler )
    Contributors: Angelos Filos
    Module: Main
    Description: Electron Renderer Process - Script to executed after `index.html` is loaded.
*)

module Renderer

open Fable.Core.JsInterop
open Fable.Import
open FSharp.Core
open CommonData

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
        let value = int32 (Ref.register reg).innerHTML
        Update.registerFormat reg value format
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
        let value = int32 (Ref.register reg).innerHTML
        Update.registerFormat reg value format

    (Ref.registerFormatAll format).addEventListener_click(fun _ ->
        Update.registerFormatAll format
        List.map updateReg [0..15]
        |> List.iter id       
    )

// Show or hide all the registers
let showHideRegs format id = 
    let el = Ref.registerGroup id
    el.setAttribute("class", format)

 
let flagListener flag = 
    (Ref.flag flag).addEventListener_click(fun _ ->
        Update.toggleFlag flag
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
        let lines = 
            (Ref.code ()).Split('\n')
            |> Array.toList

        // Gets initial register values from GUI
        let initialRegs =
            let regVal id = CommonData.register id, uint32 (Ref.register id).innerHTML
            List.map regVal [0..15]
            |> Map.ofList
            |> Some

        let initialFlags = 
            let flagVal id = 
                match (Ref.flag id).innerHTML with
                | "0" -> false
                | _ -> true

            Some
                { 
                    Flags.N = flagVal "N"; 
                    Flags.Z = flagVal "Z"; 
                    Flags.C = flagVal "C"; 
                    Flags.V = flagVal "V"
                }          

        let initialise = 
            match Emulator.TopLevel.initDataPath initialFlags initialRegs None with
            | Ok x -> x
            | Error _ -> failwithf "Failed"

        let res = 
            match Emulator.TopLevel.parseThenExecLines lines initialise None with
            | Ok (returnData, returnSymbols) -> 
                Update.flags returnData.Fl
                Update.changeRegisters returnData.Regs

            | Error _ -> failwithf "Failed"

        res
    )

    Ref.memoryPanel.addEventListener_click(fun _ ->
        
        Ref.regsiterTop.setAttribute("class", "hidden")
        Ref.labelsTable.setAttribute("class", "hidden")
        
        Ref.memoryPanel.setAttribute("class", "btn target")
        Ref.registerPanel.setAttribute("class", "btn btn-default")
        Ref.labelPanel.setAttribute("class", "btn btn-default")

        List.map (showHideRegs "hidden") [0..15]
        |> List.iter id
    )

    Ref.registerPanel.addEventListener_click(fun _ ->
        
        Ref.regsiterTop.setAttribute("class", "")
        Ref.labelsTable.setAttribute("class", "hidden")

        Ref.memoryPanel.setAttribute("class", "btn btn-default")
        Ref.registerPanel.setAttribute("class", "btn target")
        Ref.labelPanel.setAttribute("class", "btn btn-default")

        List.map (showHideRegs "") [0..15]
        |> List.iter id
    )

    Ref.labelPanel.addEventListener_click(fun _ ->
        
        Ref.regsiterTop.setAttribute("class", "hidden")
        Ref.labelsTable.setAttribute("class", "")

        Ref.memoryPanel.setAttribute("class", "btn btn-default")
        Ref.registerPanel.setAttribute("class", "btn btn-default")
        Ref.labelPanel.setAttribute("class", "btn target")

        List.map (showHideRegs "hidden") [0..15]
        |> List.iter id
    )


    // Reset button click event listener
    Ref.reset.addEventListener_click(fun _ ->
        let setTo0 reg = 
            Update.registerFormat reg 0 "dec"
        
        Update.registerFormatAll "dec"

        List.map setTo0 [0..15]
        |> List.iter id
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

    List.map flagListener ["C";"N";"V";"Z"]
    |> List.iter id

init()
