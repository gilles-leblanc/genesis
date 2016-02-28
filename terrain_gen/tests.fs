module Tests

open HeightMap
open MidpointDisplacement

// assert functions
let assertAreEqual expected actual =
    printf "... "
    if expected <> actual then printfn "%s" ("Test failed, expected " + expected.ToString() + ", actual " + actual.ToString())  
    else printfn "Test passed"

// tests
let testNewHeightMapReturnZeroInitializedHm () = 
    let hm = newHeightMap 5
    let result = hm.Map |> Array.sum
    assertAreEqual 0.0 result

// tests included in run
let testsToRun = 
    [
        ("test newHeightMap will return a 0 initialized height map",
        testNewHeightMapReturnZeroInitializedHm)
    ]

// test runner
let runSingleTest test = 
    let testName, testFunction = test
    printf "%s" testName    
    testFunction()

let runTests =
    testsToRun |> List.map (fun t -> runSingleTest t)
    printfn "%s" "Ran all tests."
    
