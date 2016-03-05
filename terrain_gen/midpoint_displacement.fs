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
    
// returns the average between two values    
let avg (a:float) (b:float) =
    (a + b) / 2.0
    
// find the middle between two points in our map
let middle a b =
    (a + b) / 2              
    
// set the "diamond" corners (see diamond-square algo.) which are the middle
// values between each corner (c1 c2 c3 c4)   
let diamond (hm:HeightMap) c1 c2 c3 c4 =
    let x1, y1 = c1
    let x2, y2 = c2
    let x3, y3 = c3
    let x4, y4 = c4
    
    // set upper middle
    hm.Set (middle x1 x2) y1 (avg (hm.Get x1 y1) (hm.Get x2 y2))     
    
    // set lower middle
    hm.Set (middle x3 x4) y3 (avg (hm.Get x3 y3) (hm.Get x4 y4))
    
    // set left middle
    hm.Set (middle y1 y3) x1 (avg (hm.Get x1 y1) (hm.Get x3 y3))
    
    // set right middle
    hm.Set (middle y2 y4) x2 (avg (hm.Get x2 y2) (hm.Get x4 y4))   
    
let generate hm =
    initCorners hm
    let size = hm.Size
    diamond hm (0, 0) (0, size) (size, 0) (size, size)