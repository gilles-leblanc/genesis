module NameGenerator

open System
open System.Collections.Generic   

open NameLength
open ProbabilityTable

let rnd = System.Random()

// Randomly returns a string from values based on it's probability
let pickString (values:Map<string, float>) : string =
    let randomValue = rnd.NextDouble()
    let pick = values
               |> Map.tryPick (fun key value -> if randomValue >= value then Some(key) else None)
    
    match pick with
    | Some v -> v
    | None -> failwith "Can't pick letter"

// Recursively creates a new name.
let rec buildName (nameSoFar:string) (charLeft:int) (probabilityTable:ProbabilityTable) : string =
    let lastChar = Char.ToString nameSoFar.[nameSoFar.Length - 1]

    let addition = match Map.containsKey lastChar probabilityTable.probabilities with
                   // if our character exists pick one of it's subkeys
                   | true -> pickString probabilityTable.probabilities.[lastChar]              
                   // otherwise start a new sequence of character with a name starting character
                   | false -> pickString probabilityTable.probabilities.[" "]   

    let newName = nameSoFar + addition
    let newCharLeft = charLeft - addition.Length
    
    match newCharLeft with
    | ln when ln > 0 -> buildName newName newCharLeft probabilityTable // we need more
    | ln when ln < 0 -> newName.[0..newName.Length - 1]   // we went one char to long
    | _ -> newName    // we are exactly where we want to be             

// Given a pre-built probability table generates a random name.
let generateRandomName (probabilityTable:ProbabilityTable) : string = 
    let nameLength = int (getNameLength probabilityTable.nameLengthInfo)
    // We pass in the whitespace char to start the name as this will allow us to find letters after
    // spaces in our probability table. These are the letters that start name. 
    // We must remember to take this whitespace into account in our nameLength and later when 
    // returning the name
    let lowerCaseName = buildName " " nameLength probabilityTable
    (Char.ToUpper lowerCaseName.[1] |> Char.ToString) + lowerCaseName.[2..]