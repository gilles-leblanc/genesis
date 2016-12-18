module GenesisTools

open CommandLine
open MapTools

open NameGenerator
open HeightMap  
open MidpointDisplacement

let generateMap =
    let map = newHeightMap 8
    generate map 0.3 0.5
    heightMapToPng map "out.png"
    heightMapToTxt map "out.txt"  

[<EntryPoint>]
let main argv =
    let toolOption = parseCommandLine (argv |> Array.toList)

    match toolOption.tool with
    | MapGenerator -> generateMap
    | NameGenerator -> printfn "Name Generator"
    | NoOptions -> ()

    0 
