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
open SketchMap

let private generateMap option =
    match option with
    | MidpointDisplacement ->         
        let map = newHeightMap 10
        midpointDisplacement map initCornersWithConfigValues
        map
    | ValueNoise ->
        generateNoise 1025 

let private generateName length fileName = 
    let table = buildProbabilityTableFromMediaFile fileName length
    printfn "%s" (generateRandomName table)

let private serializeName length inFileName outFileName =
    let table = buildProbabilityTableFromMediaFile inFileName length
    serializeProbabilityTable outFileName table

let private generateTerrain () =
    let map = generateMap MidpointDisplacement
    let rainMap = generateMap MidpointDisplacement
    map |> heightMapToPng "out.png"
    makeTerrain gradientColors map rainMap

let private terraform () =
    terraform ()

let private sketch () =
    sketch ()

[<EntryPoint>]
let main argv =
    let toolOption = parseCommandLine (argv |> Array.toList)

    match toolOption.tool with
    | MapGenerator opt -> generateMap opt |> heightMapToPng "out.png" |> ignore
    | NameGenerator opts -> generateName opts.Length opts.FileName |> ignore
    | NameSerializer opts -> serializeName opts.Length opts.InFileName opts.OutFileName |> ignore
    | TerrainGenerator -> generateTerrain () |> ignore
    | Terraform -> terraform () |> ignore
    | Sketch -> sketch () |> ignore
    | NoOptions -> ()

    0 
