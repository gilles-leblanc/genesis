module NameGenerator

open System
open System.Collections.Generic   

// Parses a string and count the total number of occurrences of substrings of size length
let rec countOccurrences (input:string) (occurrenceTable:Map<string, float>) length = 
    let adjLen = length - 1
    
    match input |> Seq.toList with
    | head :: tail when tail.Length >= adjLen -> 
        let other = Seq.take adjLen tail |> Seq.toList
        let occurrence = head :: other |> Array.ofList |> String

        // add current occurrence to the occurrence table
        let updatedMap = match occurrenceTable.ContainsKey (occurrence) with
                         | true -> occurrenceTable.Add(occurrence, occurrenceTable.[occurrence] + 1.0)
                         | false -> occurrenceTable.Add(occurrence, 1.0)

        // call the function recursively with the rest of the string
        countOccurrences (tail |> Array.ofList |> String) updatedMap length
    | _ -> occurrenceTable

// Return a new probability table with the key value pair added.
let addProbability (key:string) value (probabilityTable:Map<string, Map<string, float>>) length =
    let mainKey = Seq.take length key |> String.Concat
    let subKey = Seq.skip length key |> String.Concat
    
    match probabilityTable.ContainsKey(mainKey) with
    | true -> let subMap = Map.find mainKey probabilityTable
              match subMap.ContainsKey(subKey) with
              | true -> failwith "subkey already added in probabilityTable"
              | false -> let newSubMap = subMap.Add(subKey, value)
                         probabilityTable.Add(mainKey, subMap)
    | false -> let subMap = Map.empty.Add(subKey, value)
               probabilityTable.Add(mainKey, subMap)

// Given an input string create a frequency table for the different letters in the string
let buildFrequencyTableTable (input:string) length =
    let initialDictionary = Map.empty
    let total = input.Length - length + 1 |> float

    let frequencyTable = countOccurrences input initialDictionary length 
                         |> Map.map (fun key value -> Math.Round(value / total, 6))

    frequencyTable
    
// Given an input file create a probability table for the different letters in the file
let buildProbabilityTable (filePath:string) length = // : Map<string, Map<string, float>>
    let input = System.IO.File.ReadAllText(filePath)
    buildFrequencyTableTable input length

    // let adjLen = length - 1
    // let probabilityTable = Map.empty

    // Map.fold (fun state key value -> addProbability key value state adjLen) probabilityTable frequencyTable
    //Map.map (fun key value -> addProbability key value probabilityTable adjLen) frequencyTable