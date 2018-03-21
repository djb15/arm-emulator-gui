(*
    High Level Programming @ Imperial College London # Spring 2018
    Project: A user-friendly ARM emulator in F# and Web Technologies ( Github Electron & Fable Compliler )
    Contributors: Angelos Filos
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

// Change the register values in the GUI after execution
let changeRegisters (regs: Map<CommonData.RName,uint32>) =
    let frontEndReg (key: CommonData.RName) (value: uint32) =
        let regNum = key.RegNum
        let el = Ref.register regNum
        el.innerHTML <- sprintf "%i" value
    Map.iter frontEndReg regs


let symbols (sym: CommonLex.SymbolTable) (mem: CommonData.MachineMemory<'INS>) = 
    let el = Ref.labelsTableBody

    let symbolHTML (labelType:string) (labelName:string) (labelValue:uint32) (labelContent:string) = 
        sprintf """<tr>
            <td>%s</td>
            <td>%s</td>
            <td>0x%X</td>
            <td>%s</td>
        </tr>""" labelType labelName labelValue labelContent

    let memoryHTML (memLocation:uint32) (memContents:string) = 
        sprintf """<tr>
            <td>0x%X</td>
            <td>%s</td>
        </tr>""" memLocation memContents


    let findInMemory (label,value) =
        let convertValue = WA value 
        match mem.TryFind convertValue with
        | Some memory ->
            match memory with
            | CommonData.DataLoc x -> (symbolHTML "Data" label convertValue.Val (sprintf "0x%X" x))
            | CommonData.Code _ -> (symbolHTML "Code" label convertValue.Val "Code")
        | Microsoft.FSharp.Core.Option.None -> (symbolHTML "EQU" label convertValue.Val "N/A")


    let finalSymbolTable = 
        Map.toList sym
        |> List.map findInMemory
        |> String.concat "\n"
    
    el.innerHTML <- finalSymbolTable


let memory (mem: CommonData.MachineMemory<'INS>) = 
    let el = Ref.memoryTableBody

    let memoryHTML (memLocation:uint32) (memContents:string) = 
        sprintf """<tr>
            <td>0x%X</td>
            <td>%s</td>
        </tr>""" memLocation memContents


    let findInMemory (loc:WAddr,value) =
        let location = loc

        match value with
        | CommonData.DataLoc x -> (memoryHTML location.Val (sprintf "0x%X" x))
        | CommonData.Code _ -> (memoryHTML location.Val "Code")


    let finalSymbolTable = 
        Map.toList mem
        |> List.map findInMemory
        |> String.concat "\n"
    
    el.innerHTML <- finalSymbolTable


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

let toggleFlag (id: string) =
    let el = Ref.flag id
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

let error err lineNum =
    window?setError(err, lineNum + 1u)

let clearError _ =
    window?clearError("")


let changeEmulationStatus status err = 
    let el = Ref.emulator
    el.innerHTML <- (sprintf "%s" status)
    match err with
    | false ->
        el.setAttribute("style", "background: #ffe290")
    | true ->
        el.setAttribute("style", "background: #e57c8e")



let resetAndRun _ =
    window?resetRun("")

    
