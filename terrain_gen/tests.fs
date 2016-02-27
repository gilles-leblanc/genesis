module Tests

open HeightMap
open MidpointDisplacement

// assert functions
let assertAreEqual expected actual =
    if expected <> actual then printfn "%s" ("Test failed, expected " + expected.ToString() + ", actual " + actual.ToString())  
    else ()

// tests
let ``test newHeightMap will return a 0 initialized height map`` = 
    let hm = newHeightMap 5
    let result = hm.Map |> Array.sum
    assertAreEqual 1.0 result

// test runner
let runTests =
    let testsToRun = [``test newHeightMap will return a 0 initialized height map``]    
    testsToRun |> List.map (fun f -> f)
    printfn "%s" "Ran all tests."
    