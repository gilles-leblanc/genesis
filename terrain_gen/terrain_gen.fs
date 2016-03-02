module TerrainGen

open HeightMap  
open MidpointDisplacement
open Tests

[<EntryPoint>]
let main argv =
    consoleTestRunner testsToRun   
    0 
