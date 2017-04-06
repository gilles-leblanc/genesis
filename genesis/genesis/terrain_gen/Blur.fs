module Blur

open HeightMap

type Filter = { Positions:int list; Kernel: float array }

// Applies a filter to a heightmap at position X Y
let private filter x y (heightMap:HeightMap) (filter:Filter) fposx fposy =
    let filtered = filter.Positions 
                   |> List.fold (fun acc p -> let posx = fposx x p
                                              let posy = fposy y p
                                              if posx >= 0 && posx < heightMap.Size &&
                                                 posy >= 0 && posy < heightMap.Size
                                              then acc + 
                                                   filter.Kernel.[p + 3] * heightMap.Get posx posy 
                                              else acc) 0.0

    heightMap.Set x y filtered


// Take a height map and blur it using a Gaussian blur
let gaussianBlur (heightMap:HeightMap) =  
     let gaussianFilter = { Positions = [-3..3]; 
                            Kernel = [| 0.006; 0.061; 0.242; 0.383; 0.242; 0.061; 0.006 |]}
     let size = heightMap.Size

     // do the filter on one dimension
     [2..size - 3] |> List.iter (fun i -> 
        [2.. size - 3] |> List.iter (fun j -> filter i j heightMap gaussianFilter (( + )) (fun y p -> y)))

    // do the filter on the other dimension
     [2..size - 3] |> List.iter (fun i -> 
        [2.. size - 3] |> List.iter (fun j -> filter i j heightMap gaussianFilter (fun x p -> x) (( + ))))


