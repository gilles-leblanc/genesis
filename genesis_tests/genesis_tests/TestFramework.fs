module TestFramework

let passed = "Test passed"

// assert functions
let assertIsFalse value =
    match value with
    | true -> "Test failed, expected false but was true"
    | false -> passed

let assertIsTrue value =
    match value with 
    | true -> passed
    | false -> "Test failed, expected true but was false"

// todo: replace with matches
let assertAreEqual expected actual =
    if expected <> actual then 
        sprintf "Test failed, expected %A, actual %A" expected actual  
    else 
        passed
        
let assertIsGreaterThan target actual =
     if target >= actual then
        sprintf "Test failed, expected %A to be greater than %A" target actual
     else
        passed 
        
// test runner
let runSingleTest (testName, testFunction) = 
    sprintf "%s... %s" testName (testFunction())       

let runTests testList =    
    testList |> List.map (runSingleTest)
    
let consoleTestRunner testList =
    runTests testList |> List.iter (printfn "%s")
    printfn "%s" "Ran all tests."