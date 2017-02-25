module GenesisTools

open CommandLine
open MapTools

open ProbabilityTable
open NameGenerator
open HeightMap  
open MidpointDisplacement
open ValueNoise

let generateMap option =
    match option with
    | MidpointDisplacement ->         
        let map = newHeightMap 10
        midpointDisplacement map 0.3 0.5 
        heightMapToPng "out.png" map
    | ValueNoise ->
        generateNoise 1024 256.0
        |> heightMapToPng "out.png"

let generateName length fileName = 
    let table = buildProbabilityTableFromMediaFile fileName length
    printfn "%s" (generateRandomName table)

let serializeName length inFileName outFileName =
    let table = buildProbabilityTableFromMediaFile inFileName length
    serializeProbabilityTable outFileName table

[<EntryPoint>]
let main argv =
    let toolOption = parseCommandLine (argv |> Array.toList)

    match toolOption.tool with
    | MapGenerator opt -> generateMap opt |> ignore
    | NameGenerator opts -> generateName opts.Length opts.FileName |> ignore
    | NameSerializer opts -> serializeName opts.Length opts.InFileName opts.OutFileName |> ignore
    | NoOptions -> ()

    0 
