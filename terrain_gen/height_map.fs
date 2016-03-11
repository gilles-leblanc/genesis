module HeightMap

type HeightMap = {Size:int; Map:float array} with     
    member this.Get x y =
        this.Map.[x * this.Size + y]      
        
    member this.Set x y value =
        this.Map.[x * this.Size + y] <- value

// returns a square matrix of size 2^n + 1
let newHeightMap n : HeightMap =
    let size = ( pown 2 n ) + 1
    {Size = size; Map = Array.zeroCreate (size * size)}  

// normalize a single value to constrain it's value between 0.0 and 1.0
let normalizeValue v =
    match v with
    | v when v < 0.0 -> 0.0
    | v when v > 1.0 -> 1.0
    | _ -> v

// converts a float point ranging from 0.0 to 1.0 to a rgb value
// 0.0 represents black and 1.0 white. The conversion is in greyscale 
let convertFloatToRgb (pct:float) : int * int * int =
    let greyscale = int (float 255 * pct)
    (greyscale, greyscale, greyscale)