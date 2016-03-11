module HeightMap

// contains the height map types and common functions that can be re-used for 
// different generation algorithms

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
    
// returns the average between two values    
let avgf (a:float) (b:float) =
    (a + b) / 2.0
    
// find the middle between two points in our map
let avgi (a:int) (b:int) =
    (a + b) / 2
    
// returns a floating number which is generated using bounds as a control of the range of possible values
let randomize (rnd:System.Random) (bound:int) : float = 
    float (rnd.Next(-bound, bound) / bound)
    