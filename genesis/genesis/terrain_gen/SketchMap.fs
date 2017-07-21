module SketchMap

open System.Configuration
open System.Drawing

open HeightMap 
open ValueNoise
open MidpointDisplacement
open Terrain
open Blur

let private brushTreshholdLevel = ConfigurationManager.AppSettings.Item("brushTreshholdLevel") |> float

// Generate a non realistic map in a "sketch" style
let sketch () = 
    let landmassMap = newHeightMap 10
    midpointDisplacement landmassMap initCornersWithConfigValues

    let humidityMap = newHeightMap 10
    midpointDisplacement humidityMap initCornersWithConfigValues

    let brushMap = generateNoise 1025 

    gaussianBlur landmassMap

    let bitmap = makeTerrain solidColors landmassMap humidityMap

    let newPng = new Bitmap(landmassMap.Size, landmassMap.Size)

    for x in [0..landmassMap.Size-1] do
        for y in [0..landmassMap.Size-1] do            
            newPng.SetPixel(x, y, bitmap.GetPixel(x, y))

    let mutable i = 0
    let mutable j = 0

    while i < brushMap.Size do        
        while j < brushMap.Size do
        //     let value = brushMap.Get i j            
        //     if value > brushTreshholdLevel then 
        //         newPng.SetPixel(i, j, Color.FromArgb(255, 255, 0, 0))
            j <- j + 1
            printfn "j: %i" j
        i <- i + 1        
        printfn "i: %i" i

    // for x in [0..brushMap.Size-1] do
    //     for y in [0..brushMap.Size-1] do            
    //         let value = brushMap.Get x y            
    //         // if we are over a certain value, check to fit a tree brush
    //         if value > brushTreshholdLevel then 
    //             // draw brush

    //             // increment x y

    //             newPng.SetPixel(x, y, Color.FromArgb(255, 255, 0, 0))
    //             // if it fits overlay it on original image
                

    newPng.Save("sketch.png", Imaging.ImageFormat.Png)
    ignore