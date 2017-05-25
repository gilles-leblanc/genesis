module Terraform

open System.Configuration
open System.Drawing

open HeightMap  
open ValueNoise
open MidpointDisplacement
open Terrain
open Blur

let private absorptionFactor = ConfigurationManager.AppSettings.Item("absorptionFactor") |> float
let private erosionFactor = ConfigurationManager.AppSettings.Item("erosionFactor") |> float
let private numberRunOffSteps = ConfigurationManager.AppSettings.Item("numberRunOffSteps") |> int

// Given a height map and a pair of x and y coordinates, find the lowest neighboring point.
let findLowestNeighbor (heightMap:HeightMap) (x, y) : int * int =
    // collect a list of all neighbors
    let possibleNeighbors = [(x - 1, y - 1); (x, y - 1); (x + 1, y - 1);
                             (x - 1, y);                 (x + 1, y);
                             (x - 1, y + 1); (x, y + 1); (x + 1, y + 1);]
    
    // filter out the spots where there aren't any neighbors (invalid coordinates)
    let neighbors = possibleNeighbors |> List.filter (fun (x, y) ->  heightMap.CoordValid x y)

    // find lowest by value
    neighbors |> List.minBy (fun (nx, ny) -> heightMap.Get nx ny)

// simulate water run off on the height map
let runoff (landmassMap:HeightMap) (rainMap:HeightMap) step =   
    // make a copy of the map not to modify the original as we work on it
    let watershedStep = newHeightMap 10

    // calculate water runoff for each coord
    for x in [0..landmassMap.Size-1] do
        for y in [0..landmassMap.Size-1] do
            // get water at current coordinate
            match rainMap.Get x y with
            | waterAt when waterAt > 0.0 -> 
                // find lowest neighbor
                let (lx, ly) = findLowestNeighbor landmassMap (x, y)
                
                match isWater (landmassMap.Get lx ly) with
                // if the water runoff has reached a body of water, drain it to stop further runoff
                | true -> rainMap.Substract x y waterAt
                // if we haven't reached a body of water, continue runoff
                | false -> // get height and calculate spill            
                    let currentHeight = landmassMap.Get x y
                    let lowestNeighborHeight = landmassMap.Get lx ly                
                    
                    match currentHeight with
                    // if the current height is equal or higher to the lowest neighbor height, 
                    // all water will drain to the lowest neighbor
                    | current when current >= lowestNeighborHeight -> // take into account an "absorption factor" 
                                                                      // that accounts for absorption into the ground
                                                                      // and evaporaion into the air, this also prevents 
                                                                      // water from pooling too much
                                                                      let remaining = waterAt - absorptionFactor
                                                                      watershedStep.Add lx ly remaining
                                                                      rainMap.Substract x y waterAt
                    // otherwise current is smaller than the lowest neighbor, 
                    // we need to calculate the amount that will spill over
                    | current -> let spillOff = (currentHeight + waterAt) - lowestNeighborHeight
                                 watershedStep.Add lx ly spillOff
                                 rainMap.Substract x y spillOff

                    // erode landmass under passage of water
                    landmassMap.Substract x y erosionFactor

            | _ -> ignore()     // there is no water at this point, do nothing
    
    watershedStep

let terraform () =       
    let landmassMap = newHeightMap 10
    midpointDisplacement landmassMap

    let rainMap = newHeightMap 10
    midpointDisplacement rainMap

    // watershed generation
    let watershedMap = List.fold (fun acc elem -> runoff landmassMap acc elem) rainMap [1..numberRunOffSteps]    

    let png = new Bitmap(landmassMap.Size, landmassMap.Size) 

    for x in [0..landmassMap.Size-1] do
        for y in [0..landmassMap.Size-1] do
            let red, green, blue = gradientColors (landmassMap.Get x y) 0.0 0.0
            png.SetPixel(x, y, Color.FromArgb(255, red, green, blue))

    png.Save("terraform.png", Imaging.ImageFormat.Png) |> ignore   

    makeTerrain gradientColors landmassMap rainMap watershedMap