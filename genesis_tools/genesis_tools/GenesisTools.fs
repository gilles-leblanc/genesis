module GenesisTools

open CommandLine
open MapTools

open ProbabilityTable
open NameGenerator
open HeightMap  
open MidpointDisplacement
open ValueNoise
open Terrain

let generateMap option =
    match option with
    | MidpointDisplacement ->         
        let map = newHeightMap 10
        midpointDisplacement map 0.3 0.5 
        map
    | ValueNoise ->
        generateNoise 1024 256.0

let generateName length fileName = 
    let table = buildProbabilityTableFromMediaFile fileName length
    printfn "%s" (generateRandomName table)

let serializeName length inFileName outFileName =
    let table = buildProbabilityTableFromMediaFile inFileName length
    serializeProbabilityTable outFileName table

let generateTerrain () =
    generateMap MidpointDisplacement |> makeTerrain gradientColors

[<EntryPoint>]
let main argv =
    let toolOption = parseCommandLine (argv |> Array.toList)

    match toolOption.tool with
    | MapGenerator opt -> generateMap opt |> heightMapToPng "out.png" |> ignore
    | NameGenerator opts -> generateName opts.Length opts.FileName |> ignore
    | NameSerializer opts -> serializeName opts.Length opts.InFileName opts.OutFileName |> ignore
    | TerrainGenerator -> generateTerrain () |> ignore
    | NoOptions -> ()

    0 
