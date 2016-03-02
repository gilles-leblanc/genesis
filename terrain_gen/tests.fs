module Tests

// modules under test
open HeightMap
open MidpointDisplacement

// assert functions
let assertAreEqual expected actual : string =
    if expected <> actual then 
        sprintf "Test failed, expected %A, actual %A" expected actual  
    else 
        "Test passed"

// tests included in run
let testsToRun = 
    [
        "test newHeightMap will return a 0 initialized height map",
        fun() ->
            let hm = newHeightMap 5
            let result = hm.Map |> Array.sum
            assertAreEqual 0.0 result
    ]

// test runner
let runSingleTest (testName, testFunction) = 
    sprintf "%s... %s" testName (testFunction())       

let runTests testList =    
    testList |> List.map (runSingleTest)
    
let consoleTestRunner testList =
    runTests testList |> List.iter (printfn "%s")
    printfn "%s" "Ran all tests."