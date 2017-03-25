module Terrain

open System.Drawing

open HeightMap

// type Pixel = { Red:int; Green:int; Blue:int }
type Image = { Size:int; Pixels:int * int * int array }
type Range = { Start:int; End:int; LowColor:int * int * int; HighColor:int * int * int }

let blueRange = { Start=0; End=74; LowColor=10, 10, 100; HighColor=10, 10, 200 }    
let brownRange = { Start=75; End=180; LowColor=122, 104, 33; HighColor=244, 209, 66 }
let greyRange = { Start=181; End=255; LowColor=69, 69, 69; HighColor=129, 129, 129 }
let greenRange = { Start=150; End=200; LowColor=19, 115, 58; HighColor=19, 200, 58 }

// get wether a value is in a range
let inRange value range = 
    let { Start=start; End=``end`` } = range
    value >= start && value <= ``end``    

// get a gradient colors using a range and percentage
let gradient pct range =
    let mix low high pct =
        let fLow = float low
        let fHigh = float high
        fLow + ((fHigh - fLow) * pct)

    let { LowColor=lowColor; HighColor=highColor } = range
    let lr, lg, lb = lowColor
    let hr, hg, hb = highColor
    // changing to tuple, if successful rebase with previous commit
    (mix lr hr pct |> int), (mix lg hg pct |> int), (mix lb hb pct |> int)   

// get a specific color for a specific point using a color function
let getColors mapPoint rainPoint pctFun =         
    match convertFloatToInt mapPoint, convertFloatToInt rainPoint with
    | x, y when inRange x blueRange -> gradient (pctFun x blueRange) blueRange
    | x, y when inRange x brownRange && 
                inRange y greenRange -> gradient (pctFun y greenRange) greenRange
    | x, y when inRange x brownRange -> gradient (pctFun x brownRange) brownRange
    | x, y when inRange x greyRange -> gradient (pctFun x greyRange) greyRange
    | x, y -> failwith "invalid colors operation"

// convert a heightmap value to rgb solid colors values
let solidColors mapPoint rainPoint =
    getColors mapPoint rainPoint (fun value range -> 100.0)

// convert a heightmap value to rgb gradient colors values
let gradientColors mapPoint rainPoint =  
    getColors mapPoint rainPoint (fun value range -> float (value - range.Start) / 100.0)

let makeTerrain colorFunction (heightMap:HeightMap) (rainMap:HeightMap) = 
    // convert 1 float value to rgb
    // apply filters during conversion
    let png = new Bitmap(heightMap.Size, heightMap.Size)
    
    for x in [0..heightMap.Size-1] do
        for y in [0..heightMap.Size-1] do
            let red, green, blue = colorFunction (heightMap.Get x y) (rainMap.Get x y) 
            png.SetPixel(x, y, Color.FromArgb(255, red, green, blue))
        
    png.Save("terrain.png", Imaging.ImageFormat.Png) |> ignore