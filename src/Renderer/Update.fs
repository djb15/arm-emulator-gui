(*
    High Level Programming @ Imperial College London # Spring 2018
    Project: A user-friendly ARM emulator in F# and Web Technologies ( Github Electron & Fable Compliler )
    Contributors: Angelos Filos, Dirk Brink
    Module: Renderer.Update
    Description: Event helper functions for `HTML` elements in `index.html`.
*)

module Update

open Fable.Core
open Fable.Import
open Fable.Core.JsInterop
open Fable.Import.Browser
open FSharp.Core

open Ref
open Fable.Import.Electron
open CommonData

/// Change font size
/// Input is font size as an integer
let fontSize (size: int) =
    let options = createObj ["fontSize" ==> size]
    window?code?updateOptions options


/// Change register to selected format.
/// Inputs are register number as regId, the register value as int
/// and register format is a string.
let registerFormat (regId: int) (value: int) (format: string) =
    let el = Ref.register regId
    
    let hexButton = Ref.registerFormat regId "hex"
    let decButton = Ref.registerFormat regId "dec"
    let binButton = Ref.registerFormat regId "bin"
    
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
        let rec intToBinary i n =
            let space = 
                match n % 4 = 0 with
                | true -> " "
                | false -> ""
            match n >= 0, ((i >>> n) &&& 1u) with
            | true, 0u -> "0" + space :: intToBinary i (n-1)
            | true, _ -> "1" + space :: intToBinary i (n-1)
            | false, _ -> []
        
        hexButton.setAttribute("class", "btn btn-enc")
        decButton.setAttribute("class", "btn btn-enc")
        binButton.setAttribute("class", "btn btn-enc target")
        el.innerHTML <- sprintf "0b%s" (String.concat "" (intToBinary (uint32 value) 31))


/// Format all registers to the desired format which is input as a string
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
    | "bin" ->
        hexButton.setAttribute("class", "btn btn-enc btn-enc-top")
        decButton.setAttribute("class", "btn btn-enc btn-enc-top")
        binButton.setAttribute("class", "btn btn-enc btn-enc-top target")


/// Change the register values in the GUI after execution
/// Input is the registers as a map
let changeRegisters (regs: Map<CommonData.RName,uint32>) =
    let frontEndReg (key: CommonData.RName) (value: uint32) =
        let regNum = key.RegNum
        let el = Ref.register regNum
        el.innerHTML <- sprintf "%i" value
    Map.iter frontEndReg regs


/// Set symbols on execution
/// Inputs are SymbolTable and MachineMemory
let symbols (sym: CommonLex.SymbolTable) (mem: CommonData.MachineMemory<'INS>) = 
    let el = Ref.labelsTableBody

    // The HTML used for symbol rendering
    let symbolHTML (labelType:string) (labelName:string) (labelValue:uint32) (labelContent:string) = 
        sprintf """<tr>
            <td>%s</td>
            <td>%s</td>
            <td>0x%X</td>
            <td>%s</td>
        </tr>""" labelType labelName labelValue labelContent

    // The HTML used for memory rendering
    let memoryHTML (memLocation:uint32) (memContents:string) = 
        sprintf """<tr>
            <td>0x%X</td>
            <td>%s</td>
        </tr>""" memLocation memContents

    // Find symbol label in memory and return HTML with symbol details
    let findInMemory (label,value) =
        let convertValue = WA value 
        match mem.TryFind convertValue with
        | Some memory ->
            match memory with
            | CommonData.DataLoc x -> (symbolHTML "Data" label convertValue.Val (sprintf "0x%X" x))
            | CommonData.Code _ -> (symbolHTML "Code" label convertValue.Val "Code")
        | Microsoft.FSharp.Core.Option.None -> (symbolHTML "EQU" label convertValue.Val "N/A")

   // Concatenated list of symbols (in HTML form)
    let finalSymbolTable = 
        Map.toList sym
        |> List.map findInMemory
        |> String.concat "\n"
    
    // Render symbols
    el.innerHTML <- finalSymbolTable


/// Set memory on execution
/// Input is MachineMemory
let memory (mem: CommonData.MachineMemory<'INS>) = 
    let el = Ref.memoryTableBody

    // HTML for rendering memory in GUI
    let memoryHTML (memLocation:uint32) (memContents:string) = 
        sprintf """<tr>
            <td>0x%X</td>
            <td>%s</td>
        </tr>""" memLocation memContents

    // Find address in memory and set value depending on MemLoc type
    let findInMemory (loc:WAddr,value) =
        let location = loc

        match value with
        | CommonData.DataLoc x -> (memoryHTML location.Val (sprintf "0x%X" x))
        | CommonData.Code _ -> (memoryHTML location.Val "Code")

    // Concatenate list of memory (in HTML form)
    let finalMemoryTable = 
        Map.toList mem
        |> List.map findInMemory
        |> String.concat "\n"
    
    // Render in GUI
    el.innerHTML <- finalMemoryTable


/// Set flags after execution
/// Input is a Flags object containing the flag
/// results after running emulator
let flags (values: CommonData.Flags) =
    let setFlag id value = 
        let el = Ref.flag id
        match value with
            | false ->
                el.setAttribute("style", "background: #fcfcfc")
                el.innerHTML <- sprintf "%i" 0
            | true ->
                el.setAttribute("style", "background: #4285f4")
                el.innerHTML <- sprintf "%i" 1

    setFlag "N" values.N
    setFlag "Z" values.Z
    setFlag "C" values.C
    setFlag "V" values.V

/// Toggle a flag between set and unset
/// Input is the id of the flag as a string
let toggleFlag (flagId: string) =
    let el = Ref.flag flagId
    let currentVal = el.innerHTML
    match currentVal with
        | "1" ->
            el.setAttribute("style", "background: #fcfcfc")
            el.innerHTML <- sprintf "%i" 0
        | _ ->
            el.setAttribute("style", "background: #4285f4")
            el.innerHTML <- sprintf "%i" 1


let code (text: string) =
    window?code?setValue(text)


/// Set error in the monaco editor
/// Inputs are error as a string and line number 
/// as an integer (which has to be increased by 1
/// as top level counts from 0)
let error err lineNum =
    window?setError(err, lineNum + 1u)

/// Clear monaco window of all errors
let clearError _ =
    window?clearError("")

/// Change the status of emulation at the top of the GUI window
/// Inputs are the status string and whether or not there
/// has been an error in emulation (err: bool)
let changeEmulationStatus status err = 
    let el = Ref.emulator
    el.innerHTML <- (sprintf "%s" status)
    match err with
    | false ->
        el.setAttribute("style", "background: #ffe290")
    | true ->
        el.setAttribute("style", "background: #e57c8e")


/// Reset the GUI and run the emulator
/// by emulating clicks in javascript
let resetAndRun _ =
    window?resetRun("")

    
