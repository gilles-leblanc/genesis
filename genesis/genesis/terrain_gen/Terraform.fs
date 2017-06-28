module Terraform

open System.Configuration
open System.Drawing

open HeightMap  
open ValueNoise
open MidpointDisplacement
open Terrain
open Blur

let private absorptionFactor = ConfigurationManager.AppSettings.Item("absorptionFactor") |> float
let private landErosionFactor = ConfigurationManager.AppSettings.Item("landErosionFactor") |> float
let private stoneErosionFactor = ConfigurationManager.AppSettings.Item("stoneErosionFactor") |> float
let private minimumErodedLandHeight = ConfigurationManager.AppSettings.Item("minimumErodedLandHeight") |> float
let private numberRunOffSteps = ConfigurationManager.AppSettings.Item("numberRunOffSteps") |> int

// Given a height map and a pair of x and y coordinates, find the lowest neighboring point.
let findLowestNeighbors (heightMap:HeightMap) (x, y) : (int * int) list =
    // collect a list of all neighbors
    let possibleNeighbors = [(x - 1, y - 1); (x, y - 1); (x + 1, y - 1);
                             (x - 1, y);                 (x + 1, y);
                             (x - 1, y + 1); (x, y + 1); (x + 1, y + 1);]
    
    // filter out the spots where there aren't any neighbors (invalid coordinates)
    let neighbors = possibleNeighbors |> List.filter (fun (x, y) ->  heightMap.CoordValid x y)

    // find lowest by value
    let minValue = neighbors |> List.map (fun (nx, ny) -> heightMap.Get nx ny) |> List.min        

    // get all neighbors with this minimum value
    neighbors |> List.filter (fun (nx, ny) -> heightMap.Get nx ny = minValue)

// erode the landmass following the action of water
let private erode x y (landmassMap:HeightMap) =
    let v = (landmassMap.Get x y)
    
    let erosionFactor = match isMountain v with
                        | true -> stoneErosionFactor
                        | false -> landErosionFactor

    match v with 
    | currentHeight when currentHeight > minimumErodedLandHeight -> landmassMap.Substract x y erosionFactor
    | _ -> ignore() // do not erode past this point

// simulate water run off on the height map
let runoff (landmassMap:HeightMap) (rainMap:HeightMap) step =   
    let watershedStep = newHeightMap 10

    // calculate water runoff for each coord
    for x in [0..landmassMap.Size-1] do
        for y in [0..landmassMap.Size-1] do
            // get water at current coordinate
            match rainMap.Get x y with
            | waterAt when waterAt > 0.0 -> 
                // find lowests neighbors
                let lowests = findLowestNeighbors landmassMap (x, y)
                let dividedWater = waterAt / (lowests |> List.length |> float) 

                // for each lowest neighbor
                lowests |> List.iter (fun (lx, ly) -> 
                    match isWater (landmassMap.Get lx ly) with
                    // if the water runoff has reached a body of water, drain it to stop further runoff
                    | true -> rainMap.Substract x y dividedWater
                    // if we haven't reached a body of water, continue runoff
                    | false -> // get height and calculate spill            
                        let currentHeight = landmassMap.Get x y
                        let lowestNeighborHeight = landmassMap.Get lx ly                
                        
                        match currentHeight with
                        // if the current height is equal or higher to the lowest neighbor height, 
                        // some of the water will drain to the lowest neighbor (water divided by number of lowests)
                        | current when current >= lowestNeighborHeight -> // take into account an "absorption factor" 
                                                                        // that accounts for absorption into the ground
                                                                        // and evaporaion into the air, this also prevents 
                                                                        // water from pooling too much
                                                                        let remaining = dividedWater - absorptionFactor
                                                                        watershedStep.Add lx ly remaining
                                                                        rainMap.Substract x y dividedWater
                        // otherwise current is smaller than the lowest neighbor, 
                        // we need to calculate the amount that will spill over
                        | current -> let spillOff = (currentHeight + dividedWater) - lowestNeighborHeight
                                     let remaining = spillOff - absorptionFactor
                                     watershedStep.Add lx ly remaining
                                     rainMap.Substract x y spillOff

                        // erode landmass under passage of water
                        erode x y landmassMap
                )

            | _ -> ignore()     // there is no water at this point, do nothing
    
    watershedStep

let terraform () =       
    let landmassMap = newHeightMap 10
    midpointDisplacement landmassMap initCornersWithConfigValues

    let humidityMap = newHeightMap 10
    midpointDisplacement humidityMap initCornersWithConfigValues

    let rainMap = newHeightMap 10
    midpointDisplacement rainMap initCornersWithHighCornerValues

    // watershed generation
    let watershedMap = List.fold (fun acc elem -> runoff landmassMap acc elem) rainMap [1..numberRunOffSteps]    
 

    makeTerrain gradientColors landmassMap humidityMap watershedMap