module Tests

// modules under test
open HeightMap
open MidpointDisplacement

// assert functions
let assertAreEqual expected actual =
    printf "... "
    if expected <> actual then 
        printfn "Test failed, expected %A, actual %A" expected actual  
    else 
        printfn "Test passed"

// tests

// // Note: this test will fail if the four corners are all initialized to 0.0. While this should be exceptional the test should be better designed.
// let initCornersWillInitializeTheFourCorners () =
//     let hm = newHeightMap 5
//     initCorners hm    

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
    printf "%s" testName    
    testFunction()

let runTests testList =
    testList |> List.iter runSingleTest
    printfn "%s" "Ran all tests."
    
