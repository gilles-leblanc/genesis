module MidpointDisplacement

open HeightMap

// set the four corners to random values
let initCorners (hm:HeightMap) =
    let rnd = System.Random()    
    
    hm.Set 0 0 (rnd.NextDouble())
    hm.Set 0 (hm.Size - 1) (rnd.NextDouble())
    hm.Set (hm.Size - 1) 0 (rnd.NextDouble())
    hm.Set (hm.Size - 1) (hm.Size - 1) (rnd.NextDouble())