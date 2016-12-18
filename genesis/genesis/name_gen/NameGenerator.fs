module NameGenerator

open System.Collections.Generic

// Open a file and read all lines into IEnumerable<string>
let readInputFile (filePath:string) = 
    System.IO.File.ReadLines(filePath);

// Parses a collection of strings and count the total number of appearance of each character
let buildOccurenceTab (input:IEnumerable<string>) =
    input |> Seq.iter(printfn "%s")
    0;