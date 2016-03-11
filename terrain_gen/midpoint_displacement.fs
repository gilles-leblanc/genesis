module MidpointDisplacement

open HeightMap

// set the four corners to random values
let initCorners (hm:HeightMap) =
    let rnd = System.Random()    
    let size = hm.Size   
    
    hm.Set 0 0 (rnd.NextDouble())
    hm.Set 0 (size - 1) (rnd.NextDouble())
    hm.Set (size - 1) 0 (rnd.NextDouble())
    hm.Set (size - 1) (size - 1) (rnd.NextDouble())
    
// set the middle values between each corner (c1 c2 c3 c4)
// variation is a function that is applied on each pixel to modify it's value
let middle (hm:HeightMap) (c1) (c2) (c3) (c4) (variation) =
    let x1, y1 = c1
    let x2, y2 = c2
    let x3, y3 = c3
    let x4, y4 = c4   
        
    // set left middle
    hm.Set x1 (avgi y1 y3) (avgf (hm.Get x1 y1) (hm.Get x3 y3) |> variation |> normalizeValue)      
    
    // set upper middle
    hm.Set (avgi x1 x2) y1 (avgf (hm.Get x1 y1) (hm.Get x2 y2) |> variation |> normalizeValue)
    
    // set right middle 
    hm.Set x2 (avgi y2 y4) (avgf (hm.Get x2 y2) (hm.Get x4 y4) |> variation |> normalizeValue)
    
    // set lower middle
    hm.Set (avgi x3 x4) y3 (avgf (hm.Get x3 y3) (hm.Get x4 y4) |> variation |> normalizeValue)           

// set the center value of the current matrix to the average of all middle values + variation function
// let center (hm:HeightMap) (c1) (c4) (variation) =
//     
    
let generate hm =
    initCorners hm
    let size = hm.Size - 1
    let rnd = System.Random()
    middle hm (0, 0) (size, 0) (0, size) (size, size) (fun x -> x + (randomize rnd 100))
    