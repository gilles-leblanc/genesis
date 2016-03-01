module TerrainGen

open HeightMap  
open MidpointDisplacement
open Tests

[<EntryPoint>]
let main argv =
    runTests testsToRun   
    0 
