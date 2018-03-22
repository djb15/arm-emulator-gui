(*
    High Level Programming @ Imperial College London # Spring 2018
    Project: A user-friendly ARM emulator in F# and Web Technologies ( Github Electron & Fable Compliler )
    Contributors: Angelos Filos, Dirk Brink
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

// System wide clipboard
let clipboard: obj = importMember "electron"

// JavaScript file handling module
let fs: obj = importDefault "fs"

/// Show error box in GUI
let errorBox (title: string) (text: string) = (importDefault "electron")?remote?dialog?showErrorBox(title, text)

// File filter options (not fully implemented but framework setup)
type FileFilter = {name: string; extensions: string list}
let fileRestriction = {name = "ARM"; extensions = ["s"]}

type OpenFileOptions = {title: string option; filters: FileFilter list}

// File open window
let openFileWindow (options: OpenFileOptions) callback = (importDefault "electron")?remote?dialog?showOpenDialog(options, callback)

// File save window
let saveFileWindow (options: OpenFileOptions) callback = (importDefault "electron")?remote?dialog?showSaveDialog(options, callback)

/// Take a string (hex/dec/bin) and converts into int32
let parseReg (s:string) =
    let rec parseBin' charLst n i =
        match charLst with
        | [] -> n
        | '1' :: rest ->
            parseBin' rest (n + i) (i*2)
        | '0' :: rest ->
            parseBin' rest n (i*2)
        | _ :: rest ->
            parseBin' rest n i
    match s with
    | x when x.StartsWith("0b") ->
        x.[2..]
        |> Seq.map System.Char.ToUpper
        |> Seq.toList
        |> List.rev
        |> fun chars -> parseBin' chars 0 1
    | _ -> int32 s


/// Adds event listener for every register format button
/// Inputs are register number as a string and the desired format
/// as a string.
let listMapper (reg,format) =
    (Ref.registerFormat reg format).addEventListener_click(fun _ ->
        let value = parseReg (((Ref.register reg).innerHTML).Trim())
        Update.registerFormat reg value format
    )


/// All format combinations for a register number as input int
let regFormatCombinations (reg:int) = 
    [reg,"dec"; reg,"hex"; reg,"bin"]


/// Add click event listener for each register as input int
let regClipboardAccess (reg:int) = 
    let register = Ref.register reg
    register.addEventListener_click(fun _ ->   
        clipboard?writeText(register.innerHTML) |> ignore
        Browser.window.alert(sprintf "Copied R%i" reg)
    )

/// Add click event listeners for the global register format update buttons.
/// Input is the desired format as a string.
/// On click all the register formats are updated.
let regsFormatAll format = 
    let updateReg reg = 
        let value = parseReg (((Ref.register reg).innerHTML).Trim())
        Update.registerFormat reg value format

    (Ref.registerFormatAll format).addEventListener_click(fun _ ->
        Update.registerFormatAll format
        List.map updateReg [0..15]
        |> List.iter id       
    )


/// Show/hide given register
/// Inputs are format of string -> "hidden" or "".
/// And regId of register as int.
let showHideRegs format regId = 
    let el = Ref.registerGroup regId
    el.setAttribute("class", format)

 
/// Click event listener for flag.
/// Input is flag as a string and on click the flag
/// is toggled
let flagListener flag = 
    (Ref.flag flag).addEventListener_click(fun _ ->
        Update.toggleFlag flag
    )

/// Initialization after `index.html` is loaded
let init () =
    // Change font size
    Ref.fontSize.addEventListener_change(fun _ ->
        let size: int =
            Ref.fontSize.value.[11..]
            |> int
        Browser.console.log "Font size updated" |> ignore
        Update.fontSize size
    )

    // Clear window after clicking New button
    Ref.newCode.addEventListener_click(fun _ ->
        Update.code("")
    )

    // Run emulator after clicking Run button
    Ref.run.addEventListener_click(fun _ ->
        // Get contents of monaco window as list of strings (for each line)
        let lines = 
            (Ref.code ()).Split('\n')
            |> Array.toList

        // Gets initial register values from GUI
        let initialRegs =
            let regVal regId = CommonData.register regId, uint32 (Ref.register regId).innerHTML
            List.map regVal [0..15]
            |> Map.ofList
            |> Some

        // Gets initial flag values from GUI
        let initialFlags = 
            let flagVal flagId = 
                match (Ref.flag flagId).innerHTML with
                | "0" -> false
                | _ -> true

            Some
                { 
                    Flags.N = flagVal "N"; 
                    Flags.Z = flagVal "Z"; 
                    Flags.C = flagVal "C"; 
                    Flags.V = flagVal "V"
                }          

        // Initialise the data path
        let initialise = 
            match Emulator.TopLevel.initDataPath initialFlags initialRegs None with
            | Ok x -> x
            | Error _ -> failwithf "Failed"

        // Run the emulator and handle the response
        let res = 
            match Emulator.TopLevel.parseThenExecLines lines initialise Map.empty with
            | Ok (returnData, returnSymbols) -> 
                Update.flags returnData.Fl
                Update.symbols returnSymbols returnData.MM
                Update.memory returnData.MM
                Update.clearError "holder" |> ignore // Clears any errors in GUI
                Update.changeRegisters returnData.Regs
                Update.changeEmulationStatus "Emulation Complete" false
           
                // Format registers properly
                let updateReg format reg =
                    let value = parseReg ((Ref.register reg).innerHTML)
                    Update.registerFormat reg value format
  
                Update.registerFormatAll "dec"
                List.map (updateReg "dec") [0..15]
                |> List.iter id  

            // Execution has failed
            | Error err -> 
                match err with
                | TopLevel.ERRLINE (errorType,lineNum) ->
                    match errorType with
                    | TopLevel.ERRIARITH err ->
                        Update.error err lineNum |> ignore
                        Update.changeEmulationStatus (sprintf "Error on line %i" (lineNum + 1u)) true

                    | TopLevel.ERRIBITARITH err ->
                        Update.error err lineNum |> ignore 
                        Update.changeEmulationStatus (sprintf "Error on line %i" (lineNum + 1u)) true

                    | TopLevel.ERRIMEM err ->
                        Update.error err lineNum |> ignore
                        Update.changeEmulationStatus (sprintf "Error on line %i" (lineNum + 1u)) true

                    | TopLevel.ERRIMULTMEM err ->
                        Update.error err lineNum |> ignore 
                        Update.changeEmulationStatus (sprintf "Error on line %i" (lineNum + 1u)) true
                    | TopLevel.ERRTOPLEVEL err ->
                        Update.error err lineNum |> ignore 
                        Update.changeEmulationStatus (sprintf "Error on line %i" (lineNum + 1u)) true
                    | _ ->
                        // This case should never happen
                        Browser.window.alert("Something went wrong")
                
                | TopLevel.ERRTOPLEVEL err ->
                    Browser.window.alert(err)
                    Update.changeEmulationStatus "Emulation failed" true                  
                | _ -> 
                    // This case should never happen
                    Browser.window.alert("Something went wrong")

        // Run emulator
        res
    )

    // Change panel in right half of window to memory
    Ref.memoryPanel.addEventListener_click(fun _ ->
        
        Ref.registerTop.setAttribute("class", "hidden")
        Ref.labelsTable.setAttribute("class", "hidden")
        Ref.memoryTable.setAttribute("class", "")
        
        Ref.memoryPanel.setAttribute("class", "btn target")
        Ref.registerPanel.setAttribute("class", "btn btn-default")
        Ref.labelPanel.setAttribute("class", "btn btn-default")

        List.map (showHideRegs "hidden") [0..15]
        |> List.iter id
    )

    // Change panel in right half of window to registers
    Ref.registerPanel.addEventListener_click(fun _ ->
        
        Ref.registerTop.setAttribute("class", "")
        Ref.labelsTable.setAttribute("class", "hidden")
        Ref.memoryTable.setAttribute("class", "hidden")

        Ref.memoryPanel.setAttribute("class", "btn btn-default")
        Ref.registerPanel.setAttribute("class", "btn target")
        Ref.labelPanel.setAttribute("class", "btn btn-default")

        List.map (showHideRegs "") [0..15]
        |> List.iter id
    )

    // Change panel in right half of window to labels
    Ref.labelPanel.addEventListener_click(fun _ ->
        
        Ref.registerTop.setAttribute("class", "hidden")
        Ref.labelsTable.setAttribute("class", "")
        Ref.memoryTable.setAttribute("class", "hidden")

        Ref.memoryPanel.setAttribute("class", "btn btn-default")
        Ref.registerPanel.setAttribute("class", "btn btn-default")
        Ref.labelPanel.setAttribute("class", "btn target")

        List.map (showHideRegs "hidden") [0..15]
        |> List.iter id
    )

    // Reset registers and flags
    Ref.reset.addEventListener_click(fun _ ->
        let setTo0 reg = 
            Update.registerFormat reg 0 "dec"
        
        Update.registerFormatAll "dec"

        Ref.emulator.innerHTML <- "Emulator Off"
        Ref.emulator.setAttribute("style", "")

        let resetFlags = {C = false; Z = false; N = false; V = false}
        Update.flags resetFlags

        List.map setTo0 [0..15]
        |> List.iter id
    )

    // Open file from computer
    Ref.openFile.addEventListener_click(fun _ ->
        let options = {title = None; filters = [fileRestriction]}
        let callback (filePath: string array) =
            // Callback not handled for error case (throws error in js console) but does
            // not crash the gui -> Should be improved
            let formatText err data = 
                Update.code data
            fs?readFile(filePath.[0], "utf8", formatText)

        openFileWindow options callback
    )

    // Save contents of monaco window to file
    Ref.save.addEventListener_click(fun _ ->
        let options = {title = None; filters = [fileRestriction]}
        let callback (fileName: string) =
            let errorCallback err = 
                Browser.console.log (err)          
            fs?writeFile(fileName, Ref.code (), errorCallback)

        saveFileWindow options callback

    )

    // Reset and run the emulator
    Ref.resetRun.addEventListener_click(fun _ ->
        Update.resetAndRun ""
    )

    // All register formating (dec, bin, hex)
    List.collect regFormatCombinations [0..15]
    |> List.map listMapper 
    |> List.iter id

    // Clipboard access for all registers
    List.map regClipboardAccess [0..15]
    |> List.iter id

    // Format all registers in dec/hex/bin format
    List.map regsFormatAll ["dec";"bin";"hex"]
    |> List.iter id

    // Generate random numbers up to 30,000 for all registers (excluding SP, LR, PC)
    Ref.registerRandomiseAll.addEventListener_click(fun _ ->
        let randomUpdate reg = 
            Update.registerFormat reg (System.Random().Next 30000) "hex"

        let setTo0 reg = 
            Update.registerFormat reg 0 "hex"
        
        List.map randomUpdate [0..12]
        |> List.iter id

        List.map setTo0 [13..15]
        |> List.iter id

        Update.registerFormatAll "hex"
    )

    List.map flagListener ["C";"N";"V";"Z"]
    |> List.iter id

init()
