module MidpointDisplacement

open HeightMap

// set the four corners to random values
let initCorners (hm:HeightMap) (rnd:System.Random) =
    let size = hm.Size   
    
    hm.Set 0 0 (rnd.NextDouble())
    hm.Set 0 (size - 1) (rnd.NextDouble())
    hm.Set (size - 1) 0 (rnd.NextDouble())
    hm.Set (size - 1) (size - 1) (rnd.NextDouble())

// set the center value of the current matrix to the average of all middle values + variation function
let center (hm:HeightMap) (x1, y1) (x2, y2) (x3, y3) (x4, y4) (variation) =
    // average height of left and right middle points
    let avgHorizontal = avg (hm.Get x1 (avg y1 y3)) (hm.Get x2 (avg y2 y4))
    let avgVertical = avg (hm.Get (avg x1 x2) y1) (hm.Get (avg x3 x4) y3)
           
    // set center value
    hm.Set (avg x1 x4) (avg y1 y4) (avg avgHorizontal avgVertical |> variation) 

// set the middle values between each corner (c1 c2 c3 c4)
// variation is a function that is applied on each pixel to modify it's value
let middle (hm:HeightMap) (x1, y1) (x2, y2) (x3, y3) (x4, y4) (variation) =   
    // set left middle
    if hm.Get x1 (avg y1 y3) = 0.0 then 
        hm.Set x1 (avg y1 y3) (avg (hm.Get x1 y1) (hm.Get x3 y3) |> variation)      
    
    // set upper middle
    if hm.Get (avg x1 x2) y1 = 0.0 then
        hm.Set (avg x1 x2) y1 (avg (hm.Get x1 y1) (hm.Get x2 y2) |> variation)
    
    // set right middle
    if hm.Get x2 (avg y2 y4) = 0.0 then 
        hm.Set x2 (avg y2 y4) (avg (hm.Get x2 y2) (hm.Get x4 y4) |> variation)
    
    // set lower middle
    if hm.Get (avg x3 x4) y3 = 0.0 then
        hm.Set (avg x3 x4) y3 (avg (hm.Get x3 y3) (hm.Get x4 y4) |> variation)           

let rec displace (hm) (x1, y1) (x4, y4) (rnd) (spread) (spreadReduction) =
    let ulCorner = (x1, y1) 
    let urCorner = (x4, y1)
    let llCorner = (x1, y4)
    let lrCorner = (x4, y4)
    
    let variation = (fun x -> x + (randomize rnd spread)) >> normalizeValue
    let adjustedSpread = spread * spreadReduction
    
    // the lambda passed in as a parameter is temporary until a define a better function
    middle hm ulCorner urCorner llCorner lrCorner variation 
    center hm ulCorner urCorner llCorner lrCorner variation
    
    if x4 - x1 >= 2 then
        let xAvg = avg x1 x4
        let yAvg = avg y1 y4
        displace hm (x1, y1) (xAvg, yAvg) rnd adjustedSpread spreadReduction
        displace hm (xAvg, y1) (x4, yAvg) rnd adjustedSpread spreadReduction
        displace hm (x1, yAvg) (xAvg, y4) rnd adjustedSpread spreadReduction
        displace hm (xAvg, yAvg) (x4, y4) rnd adjustedSpread spreadReduction
    
let midpointDisplacement hm startingSpread spreadReduction =
    let rnd = System.Random()
    let size = hm.Size - 1    
    
    initCorners hm rnd
    displace hm (0, 0) (size, size) rnd startingSpread spreadReduction