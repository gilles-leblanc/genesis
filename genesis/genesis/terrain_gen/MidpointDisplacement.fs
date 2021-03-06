module MidpointDisplacement

open System.Configuration
open HeightMap

let private startingSpread = ConfigurationManager.AppSettings.Item("startingSpread") |> float
let private spreadReduction = ConfigurationManager.AppSettings.Item("spreadReduction") |> float
let private landCornerInitValue = ConfigurationManager.AppSettings.Item("landCornerInitValue") |> float
let private seaCornerInitValue = ConfigurationManager.AppSettings.Item("seaCornerInitValue") |> float

// set the four corners to random values
let initCorners (hm:HeightMap) (rnd:System.Random) highCornerValue lowCornerValue =
    let size = hm.Size          
   
    // init a single corner value
    let initCorner (hm:HeightMap) corner value =        
        match corner with 
        | 0 -> hm.Set 0 0 value
        | 1 -> hm.Set 0 (size - 1) value
        | 2 -> hm.Set (size - 1) 0 value
        | 3 -> hm.Set (size - 1) (size - 1) value
        | _ -> failwith "Invalid corner value"

    // first init all corners with random values
    [0..3] |> List.iter (fun c -> initCorner hm c (rnd.NextDouble())) 

    // then we choose a random value to pick a corner which we will overwrite 
    // to contain our first KeyValue
    let firstCorner = rnd.Next(0, 3)
    initCorner hm firstCorner highCornerValue

    // next we choose the next corner to receive a predetermined value by selecting the corner,
    // x places from this None
    let secondCorner = (firstCorner + rnd.Next(1, 3)) % 4
    initCorner hm secondCorner lowCornerValue   

// Call initCorners with predefined highCorner and lowCorner values from the config file
let initCornersWithConfigValues (hm:HeightMap) (rnd:System.Random) =
    initCorners hm rnd landCornerInitValue seaCornerInitValue

let initCornersWithHighCornerValues (hm:HeightMap) (rnd:System.Random) =
    initCorners hm rnd landCornerInitValue landCornerInitValue

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
    
let midpointDisplacement hm initCornersFunction =
    let rec displace (hm) (x1, y1) (x4, y4) (rnd) (spread) (spreadReduction) =
        let ulCorner = (x1, y1) 
        let urCorner = (x4, y1)
        let llCorner = (x1, y4)
        let lrCorner = (x4, y4)
        
        let variation = (fun x -> x + (randomize rnd spread)) >> normalizeValue
        let adjustedSpread = spread * spreadReduction
        
        middle hm ulCorner urCorner llCorner lrCorner variation 
        center hm ulCorner urCorner llCorner lrCorner variation

        if x4 - x1 >= 2 then
            let xAvg = avg x1 x4
            let yAvg = avg y1 y4
            displace hm (x1, y1) (xAvg, yAvg) rnd adjustedSpread spreadReduction
            displace hm (xAvg, y1) (x4, yAvg) rnd adjustedSpread spreadReduction
            displace hm (x1, yAvg) (xAvg, y4) rnd adjustedSpread spreadReduction
            displace hm (xAvg, yAvg) (x4, y4) rnd adjustedSpread spreadReduction

    let rnd = System.Random()
    let size = hm.Size - 1    
    
    initCornersFunction hm rnd
    displace hm (0, 0) (size, size) rnd startingSpread spreadReduction