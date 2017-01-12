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
// Given letter X, a probability table gives a percentage for letter Y to appear following letter X.
let addProbability (key:string) value (probabilityTable:Map<string, Map<string, float>>) length =
    let mainKey = Seq.take length key |> String.Concat
    let subKey = Seq.skip length key |> String.Concat
    
    match probabilityTable.ContainsKey(mainKey) with
    | true -> let subMap = Map.find mainKey probabilityTable
              match subMap.ContainsKey(subKey) with
              | true -> failwith "subkey already added in probabilityTable"
              | false -> let newSubMap = subMap.Add(subKey, value)
                         probabilityTable.Add(mainKey, newSubMap)
    | false -> let subMap = Map.empty.Add(subKey, value)
               probabilityTable.Add(mainKey, subMap)

// Cumulate the submap to transform the probabilities from 0.25 0.25 0.5 to 0.25 0.75 1.0.
// Probabilities -> Cumulative probabilities
let cumulate map =
    let total = Map.fold (fun acc key value -> acc + value) 0.0 map
        
    let _, cumulativeSubMap = 
        Map.map (fun key value -> Math.Round(value / total, 6)) map
        // fold into a cumulative result
        |> Map.fold (fun (t, (m:Map<string, float>)) key value -> 
                        (t + value, m.Add(key, t + value))
                    ) (0.0, Map.empty)
    
    cumulativeSubMap

// Given an input string creates a probability table for the different letters in the string.
let buildProbabilityTable (input:string) length  = // : Map<string, Map<string, float>>
    let occurrencesTable = countOccurrences input Map.empty length 
    let adjLen = length - 1

    Map.fold (fun acc key value -> addProbability key value acc adjLen) Map.empty occurrencesTable
    |> Map.map (fun key value -> cumulate value)

// Given an input file path, creates a probability table calling buildProbabilityTable
let buildProbabilityTableFromFile (filePath:string) length = 
    let input = System.IO.File.ReadAllText(filePath)
    buildProbabilityTable input length