module Terraform

open System.Configuration
open System.Drawing

open HeightMap  
open ValueNoise
open MidpointDisplacement
open Terrain
open Blur

let private absorptionFactor = ConfigurationManager.AppSettings.Item("absorptionFactor") |> float

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

    // calculate water runoff at this step
    for x in [0..landmassMap.Size-1] do
        for y in [0..landmassMap.Size-1] do
            // find lowest neighbor
            let (lx, ly) = findLowestNeighbor landmassMap (x, y)

            // todo: only move if we go over the lower neighbor height when including the water
            // todo: only move the amount of water that "spills" over
            // todo: erode land by water action

            // move water to this neighbor
            watershedStep.Set x y (rainMap.Get lx ly)
    
    // temp png of this step
    let png = new Bitmap(watershedStep.Size, watershedStep.Size) 

    for x in [0..watershedStep.Size-1] do
        for y in [0..watershedStep.Size-1] do
            let red, green, blue = gradientColors (watershedStep.Get x y) (0.0) 
            png.SetPixel(x, y, Color.FromArgb(255, red, green, blue))
        
    png.Save(sprintf "terraform%i.png" step, Imaging.ImageFormat.Png) |> ignore   
    watershedStep

let terraform () =       
    let landmassMap = newHeightMap 10
    midpointDisplacement landmassMap

    let rainMap = newHeightMap 10
    midpointDisplacement rainMap

    // watershed generation
    runoff landmassMap rainMap 1      
