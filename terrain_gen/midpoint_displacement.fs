module MidpointDisplacement

open HeightMap

// set the four corners to random values
let initCorners (hm:heightMap) =
    hm.Set 0 0 1.0
    hm.Set 0 (hm.Size - 1) 1.0
    hm.Set (hm.Size - 1) 0 1.0
    hm.Set (hm.Size - 1) (hm.Size - 1) 1.0