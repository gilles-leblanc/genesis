module GenesisTools

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
    let table = buildProbabilityTableFromMediaFile "media/tolkien_sample" 4
    serializeProbabilityTable "media/tolkien_serialized" table

    let deserializedTable = buildProbabilityTableFromSerializationFile "media/tolkien_serialized" 4
    printfn "%s" (generateRandomName deserializedTable)

[<EntryPoint>]
let main argv =
    let toolOption = parseCommandLine (argv |> Array.toList)

    match toolOption.tool with
    | MapGenerator -> generateMap () |> ignore
    | NameGenerator -> generateName () |> ignore
    | NoOptions -> ()

    0 
