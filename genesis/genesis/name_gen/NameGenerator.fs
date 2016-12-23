module NameGenerator

open System.Collections.Generic

// Open a file and read all lines into IEnumerable<string>
let readInputFile (filePath:string) = 
    System.IO.File.ReadLines(filePath)

// Parses a collection of strings and count the total number of appearance of each character
let buildOccurenceTable (input:IEnumerable<string>) =
    let occurenceTable = new Dictionary<string, int>()
    
    // todo: investigate using fold with a map as the accumulator
    for s in input do 
        String.iter (fun x -> let occurence = x.ToString()
                              match occurenceTable.ContainsKey (occurence) with
                              | true -> occurenceTable.[occurence] <- occurenceTable.[occurence] + 1
                              | false -> occurenceTable.Add(occurence, 1)) s

    occurenceTable

// Given an input file create a probability table for the different letters in the file
let buildProbabilityTables (filePath:string) =
    let inputData = readInputFile filePath
    buildOccurenceTable inputData