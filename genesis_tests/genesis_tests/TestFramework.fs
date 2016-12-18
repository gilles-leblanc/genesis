module TestFramework

// assert functions
let assertAreEqual expected actual =
    if expected <> actual then 
        sprintf "Test failed, expected %A, actual %A" expected actual  
    else 
        "Test passed"
        
let assertIsGreaterThan target actual =
     if target >= actual then
        sprintf "Test failed, expected %A to be greater than %A" target actual
     else
        "Test passed" 
        
// test runner
let runSingleTest (testName, testFunction) = 
    sprintf "%s... %s" testName (testFunction())       

let runTests testList =    
    testList |> List.map (runSingleTest)
    
let consoleTestRunner testList =
    runTests testList |> List.iter (printfn "%s")
    printfn "%s" "Ran all tests."