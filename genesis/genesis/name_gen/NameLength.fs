module NameLength

open System
open MathNet.Numerics.Distributions

// A record type that contains the mean and standard deviation of the names world length from an 
// input file.
type NameLengthInfo = { mean:float; standardDeviation: float }

// Given an input string returns the NameLengthInfo record that can be later used to draw a random
// value from the normal distribution. 
let getNameLengthInfo (input:string) : NameLengthInfo =
    let names = input.Split [|' '|]
    let numberOfNames = float names.Length
    let namesLengths = names |> Array.map (fun name -> float name.Length)
    let mean = namesLengths |> Array.average        
    let standardDeviation = sqrt (namesLengths 
                                  |> Array.map (fun x -> (x - mean)**2.0) 
                                  |> Array.average)
    
    { mean = mean; standardDeviation = standardDeviation }

// Given a NameLengthInfo returns a random value drawn from a normal (gaussian) distribution
let getNameLength (nameLengthInfo:NameLengthInfo) : int =        
    let mean = nameLengthInfo.mean
    let standardDeviation = nameLengthInfo.standardDeviation
    let normalDistribution = new Normal(mean, standardDeviation)
    let length = normalDistribution.Sample() |> Math.Round |> int

    if length > 2 then length else 3