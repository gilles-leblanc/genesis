module Terrain

open System.Drawing

open HeightMap
open Blur

// type Pixel = { Red:int; Green:int; Blue:int }
type Image = { Size:int; Pixels:int * int * int array }
type Range = { Start:int; End:int; LowColor:int * int * int; HighColor:int * int * int }

let private blueRange = { Start=0; End=74; LowColor=70, 150, 195; HighColor=120, 200, 245 }    
let private brownRange = { Start=75; End=180; LowColor=180, 139, 59; HighColor=224, 199, 90 }
let private greyRange = { Start=181; End=255; LowColor=69, 69, 69; HighColor=129, 129, 129 }
let private lightGreenRange = { Start=75; End=149; LowColor=156, 192, 83; HighColor=216, 245, 143 }
let private darkGreenRange = { Start=150; End=255; LowColor=59, 99, 30; HighColor=149, 199, 99 }

// get wether a value is in a range
let private isInRange range value = 
    let inRange range value = 
        let { Start=start; End=``end`` } = range
        value >= start && value <= ``end``    

    convertFloatToInt value |> inRange range

// return whether the value is a mountain
let isMountain value =
    isInRange greyRange value

// return whether the value is water
let isWater value =
    isInRange blueRange value

// return whether the value is a plain/hills
let isPlain value =
    isInRange brownRange value

// get a gradient colors using a range and percentage
let private gradient pct range =
    let mix low high pct =
        let fLow = float low
        let fHigh = float high
        fLow + ((fHigh - fLow) * pct)

    let { LowColor=lowColor; HighColor=highColor } = range
    let lr, lg, lb = lowColor
    let hr, hg, hb = highColor
    // changing to tuple, if successful rebase with previous commit
    (mix lr hr pct |> int), (mix lg hg pct |> int), (mix lb hb pct |> int)   

// Return 100% color value
let private fullColor value range = 1.0

// get a specific color for a specific point using a color function
let getColors mapPoint rainPoint pctFun =         
    match mapPoint, rainPoint with
    | x, y when isWater x -> gradient (pctFun x blueRange) blueRange
    | x, y when isPlain x &&                                                         // use brownRange pct with
                   isInRange lightGreenRange y -> gradient (pctFun x brownRange) lightGreenRange  // greenRange values to match  
    | x, y when isPlain x &&                                                         // use brownRange pct with
                   isInRange darkGreenRange y -> gradient (pctFun x brownRange) darkGreenRange  // greenRange values to match  
    | x, y when isPlain x -> gradient (pctFun x brownRange) brownRange               // underlying terrain
    | x, y when isMountain x -> gradient (pctFun x greyRange) greyRange
    | x, y -> failwith (sprintf "invalid colors operation mp:%f rp:%f" x y)

// convert a heightmap value to rgb solid colors values
let solidColors mapPoint rainPoint =
    getColors mapPoint rainPoint fullColor

// convert a heightmap value to rgb gradient colors values
let gradientColors mapPoint rainPoint =  
    getColors mapPoint rainPoint (fun value range -> float (convertFloatToInt value - range.Start) / 100.0)

let makeTerrain colorFunction (heightMap:HeightMap) (rainMap:HeightMap) = 
    // convert 1 float value to rgb
    // apply filters during conversion
    let png = new Bitmap(heightMap.Size, heightMap.Size)

    for x in [0..heightMap.Size-1] do
        for y in [0..heightMap.Size-1] do
            let red, green, blue = colorFunction (heightMap.Get x y |> normalizeValue)
                                                 (rainMap.Get x y |> normalizeValue) 
            png.SetPixel(x, y, Color.FromArgb(255, red, green, blue))
    
    png.Save("terrain.png", Imaging.ImageFormat.Png) |> ignore
    png
