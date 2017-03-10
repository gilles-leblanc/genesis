module Terrain

open System.Drawing

open HeightMap

// type Pixel = { Red:int; Green:int; Blue:int }
type Image = { Size:int; Pixels:int * int * int array }
type Range = { Start:int; End:int; LowColor:int * int * int; HighColor:int * int * int }

// convert a heightmap value to rgb solid colors values
let solidColors p =
    let solidBlue = 10, 10, 200
    let solidBrown = 244, 209, 66
    let solidGrey = 119, 119, 119

    match convertFloatToInt p with
    | x when x < 75 -> solidBlue
    | x when x >= 75 && x < 175 -> solidBrown
    | x when x >= 175 -> solidGrey
    | x -> failwith "invalid solidColors operation"

// convert a heightmap value to rgb gradient colors values
let gradientColors p =
    let blueRange = { Start=0; End=74; LowColor=10, 10, 100; 
                                       HighColor=10, 10, 200 }    
    let brownRange = { Start=75; End=174; LowColor=122, 104, 33; 
                                          HighColor=244, 209, 66 }
    let greyRange = { Start=175; End=255; LowColor=69, 69, 69; 
                                          HighColor=129, 129, 129 }

    let inRange value range = 
        let { Start=start; End=``end`` } = range
        value >= start && value <= ``end``        

    let pct value range =
        float (value - range.Start) / 100.0

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

    match convertFloatToInt p with
    | x when inRange x blueRange -> gradient (pct x blueRange) blueRange
    | x when inRange x brownRange -> gradient (pct x brownRange) brownRange
    | x when inRange x greyRange -> gradient (pct x greyRange) greyRange
    | x -> failwith "invalid gradientColors operation"
    
let makeTerrain colorFunction (heightMap:HeightMap) = 
    // convert 1 float value to rgb
    // apply filters during conversion
    let png = new Bitmap(heightMap.Size, heightMap.Size)
    
    for x in [0..heightMap.Size-1] do
        for y in [0..heightMap.Size-1] do
            let red, green, blue = colorFunction (heightMap.Get x y) 
            png.SetPixel(x, y, Color.FromArgb(255, red, green, blue))
    
    png.Save("terrain.png", Imaging.ImageFormat.Png) |> ignore