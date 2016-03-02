module TerrainGen

open HeightMap  
open MidpointDisplacement
open TestFramework
open Tests

[<EntryPoint>]
let main argv =
    consoleTestRunner testsToRun   
    0 
