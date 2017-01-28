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
    let lastChar = nameSoFar.[nameSoFar.Length - 1].ToString() 

    match probabilityTable.probabilities.ContainsKey(lastChar) with
    | true -> let addition = pickString probabilityTable.probabilities.[lastChar]
              let newName = nameSoFar + addition
              match (addition, charLeft) with
              | (add, left) when left > add.Length -> buildName newName 0 probabilityTable
              | _ -> newName
    | false -> failwith "lastChar not found in probability table"

// Given a pre-built probability table generates a random name.
let generateRandomName (probabilityTable:ProbabilityTable) : string = 
    let nameLength = int (getNameLength probabilityTable.nameLengthInfo)
    buildName " " nameLength probabilityTable