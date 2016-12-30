module NameGenerator

open System
open System.Collections.Generic

// Open a file and read all lines into IEnumerable<string>
let readInputFile (filePath:string) = 
    System.IO.File.ReadAllText(filePath)

// Parses a string and count the total number of occurrences of substrings of size length
let rec countOccurrences (input:string) (occurrenceTable:Map<string, float>) length = 
    let adjLen = length - 1
    
    match input |> Seq.toList with
    | head :: tail when tail.Length >= adjLen -> 
        let other = Seq.take adjLen tail |> Seq.toList
        let occurrence = (head :: other |> Array.ofList |> String)

        // add current occurrence to the occurrence table
        let updatedMap = match occurrenceTable.ContainsKey (occurrence) with
                         | true -> occurrenceTable.Add(occurrence, occurrenceTable.[occurrence] + 1.0)
                         | false -> occurrenceTable.Add(occurrence, 1.0)

        // call the function recursively with the rest of the string
        countOccurrences (tail |> Array.ofList |> String) updatedMap length
    | _ -> occurrenceTable

// Given a map that contains the a collection of string and their respective counts, return a
// frequency map that is obtained by count / total count
let buildFrenquencyTable (occurrenceTable:Map<string, float>) =
    // fold occurence dict, count total
    let total = Map.fold (fun acc key value -> acc + value) 0.0 occurrenceTable
    
    // map over previous map and replace values with count / total count
    Map.map (fun key value -> Math.Round(value / total, 6)) occurrenceTable


// Given an input file create a probability table for the different letters in the file
let buildProbabilityTable (filePath:string) length =
    let input = readInputFile filePath
    let initialDictionary = Map.empty

    countOccurrences input initialDictionary length 
    |> buildFrenquencyTable
