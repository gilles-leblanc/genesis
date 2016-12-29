module NameGenerator

open System
open System.Collections.Generic

// Open a file and read all lines into IEnumerable<string>
let readInputFile (filePath:string) = 
    System.IO.File.ReadAllText(filePath)

// Parses a string and count the total number of occurrences of substrings of size length
let rec countOccurrences (input:string) (occurrenceTable:Dictionary<string, int>) length = 
    let adjLen = length - 1
    
    match input |> Seq.toList with
    | head :: tail when tail.Length >= adjLen -> 
        let other = Seq.take adjLen tail |> Seq.toList
        let occurrence = (head :: other |> Array.ofList |> String)

        // add current occurrence to the occurrence table
        match occurrenceTable.ContainsKey (occurrence) with
        | true -> occurrenceTable.[occurrence] <- occurrenceTable.[occurrence] + 1
        | false -> occurrenceTable.Add(occurrence, 1)

        // call the function recursively with the rest of the string
        countOccurrences (tail |> Array.ofList |> String) occurrenceTable length
    | _ -> occurrenceTable

// build frequency table    
    // map again with actual / total
let buildFrenquencyTable (occurrenceTable:Dictionary<string, int>) =
    // fold occurence dict, count total
    let total = occurrenceTable |> Seq.fold (fun acc (KeyValue(k, v)) -> acc + v) 0    
    total


// Given an input file create a probability table for the different letters in the file
let buildProbabilityTable (filePath:string) length =
    let input = readInputFile filePath
    let initialDictionary = new Dictionary<string, int>()

    countOccurrences input initialDictionary length