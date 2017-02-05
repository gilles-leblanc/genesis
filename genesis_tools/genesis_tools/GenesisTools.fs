module GenesisTools

open Newtonsoft.Json

open CommandLine
open MapTools

open ProbabilityTable
open NameGenerator
open HeightMap  
open MidpointDisplacement
open ValueNoise

let generateMap () =
    // let map = newHeightMap 10
    // midpointDisplacement map 0.3 0.5
    let map = generateNoise 1024 256.0
    heightMapToPng map "out.png"
    heightMapToTxt map "out.txt"  

let generateName () = 
    let table = buildProbabilityTableFromFile "media/greek_myth_sample" 3
    let json = JsonConvert.SerializeObject(table)    
    // printfn "%s" json
    printfn "%s" (generateRandomName table)

[<EntryPoint>]
let main argv =
    let toolOption = parseCommandLine (argv |> Array.toList)

    match toolOption.tool with
    | MapGenerator -> generateMap () |> ignore
    | NameGenerator -> generateName () |> ignore
    | NoOptions -> ()

    0 
