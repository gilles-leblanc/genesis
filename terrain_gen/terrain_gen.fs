module terrain_gen

open HeightMap  
open MidpointDisplacement

[<EntryPoint>]
let main argv =
    printfn "%A" argv
    let mn = newHeightMap 10    
    printfn "%f" (mn.Get 5 5) 
    0 // return an integer exit code
