module ValueNoise

open System.Configuration
open HeightMap

let dimming = ConfigurationManager.AppSettings.Item("dimming") |> float
let minLight = ConfigurationManager.AppSettings.Item("minLight") |> float
let lightBoost = ConfigurationManager.AppSettings.Item("lightBoost") |> float

// use bilinear interpolation to smooth out the noise values
let bilinearInterpolation (origMap:HeightMap) x y zoomLevel : float = 
    let x' = float x / zoomLevel
    let y' = float y / zoomLevel
    
    let fractX = x' % 1.0
    let fractY = y' % 1.0

    let mapSize = origMap.Size

    let x1 = (int x' + mapSize) % mapSize
    let y1 = (int y' + mapSize) % mapSize

    let x2 = (x1 + mapSize - 1) % mapSize
    let y2 = (y1 + mapSize - 1) % mapSize

    (fractX * fractY * origMap.Get y1 x1) 
    + ((1.0 - fractX) * fractY * origMap.Get y1 x2) 
    + (fractX * (1.0 - fractY) * origMap.Get y2 x1) 
    + ((1.0 - fractX) * (1.0 - fractY) * origMap.Get y2 x2)

// The turbulence function creates many octaves, or values with different zoom levels.
// Each octave brightness decreases has it's zoom decreases to reduce it's effect on the whole.
// The values are meant to be average into a single value.
let rec turbulence heightMap x y zoom brightnessLevel values =    
    let newBrightness = match brightnessLevel with                       
                        | b when b - dimming > minLight -> b - dimming  // reduce brightness
                        | _ -> minLight                                 // do not reduce past this point
    
    match zoom with
    | z when z >= 2.0 -> let newValue = (bilinearInterpolation heightMap x y z) * brightnessLevel
                         turbulence heightMap x y (zoom / 2.0) newBrightness (newValue :: values)
    // do the interpolation one more time at a factor of 1.0
    | _ -> (bilinearInterpolation heightMap x y 1.0) * 0.025 :: values

// generate a new heigthMap of size * size, using Value Noise method
let generateNoise size zoomLevel : HeightMap =
    let rnd = System.Random()
    let boost x = x * lightBoost

    let map = 
        Array.zeroCreate (size * size) 
        |> Array.map (fun x -> rnd.NextDouble()) 
        |> newHeightMap' size 

    let zoomed = 
        Array.zeroCreate (size * size) 
        |> newHeightMap' size  

    [0..size - 1] |> List.iter (fun i -> 
        [0.. size - 1] |> List.iter (fun j -> zoomed.Set i j (turbulence map i j zoomLevel 1.0 List.empty 
                                                              |> List.average |> boost)))
    
    zoomed

