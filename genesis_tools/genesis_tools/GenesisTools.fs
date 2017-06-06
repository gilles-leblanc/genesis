module GenesisTools

open CommandLine
open MapTools

open ProbabilityTable
open NameGenerator
open HeightMap  
open MidpointDisplacement
open ValueNoise
open Terrain
open Terraform

let private generateMap option =
    match option with
    | MidpointDisplacement ->         
        let map = newHeightMap 10
        midpointDisplacement map 
        map
    | ValueNoise ->
        generateNoise 1200 

let private generateName length fileName = 
    let table = buildProbabilityTableFromMediaFile fileName length
    printfn "%s" (generateRandomName table)

let private serializeName length inFileName outFileName =
    let table = buildProbabilityTableFromMediaFile inFileName length
    serializeProbabilityTable outFileName table

let private generateTerrain () =
    let map = generateMap MidpointDisplacement
    let rainMap = generateMap MidpointDisplacement
    let waterShedMap = newHeightMap 10
    map |> heightMapToPng "out.png"
    makeTerrain gradientColors map rainMap waterShedMap

let private terraform () =
    terraform ()

[<EntryPoint>]
let main argv =
    let toolOption = parseCommandLine (argv |> Array.toList)

    match toolOption.tool with
    | MapGenerator opt -> generateMap opt |> heightMapToPng "out.png" |> ignore
    | NameGenerator opts -> generateName opts.Length opts.FileName |> ignore
    | NameSerializer opts -> serializeName opts.Length opts.InFileName opts.OutFileName |> ignore
    | TerrainGenerator -> generateTerrain () |> ignore
    | Terraform -> terraform () |> ignore
    | NoOptions -> ()

    0 
