module GenesisTools

open Newtonsoft.Json

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

let generateName = 
    // buildProbabilityTable "media/greek_myth_sample"
    let table = buildProbabilityTable " James John Max Gary Jess Gilles Mary " 2
    let json = JsonConvert.SerializeObject(table)
    printfn "%s" json

[<EntryPoint>]
let main argv =
    let toolOption = parseCommandLine (argv |> Array.toList)

    match toolOption.tool with
    | MapGenerator -> generateMap
    | NameGenerator -> generateName |> ignore
    | NoOptions -> ()

    0 
