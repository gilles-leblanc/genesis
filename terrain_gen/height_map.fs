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

// normalize a height map to constrain all of it's values between 0.0 and 1.0
let normalize matrix =
    matrix |> Array.map (fun a ->
                            if a < 0.0 then
                                0.0
                            else if a > 1.0 then
                                1.0
                            else
                                a)
