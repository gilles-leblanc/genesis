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

let assertAreEqual expected actual =
    match expected <> actual with
    | true -> sprintf "Test failed, expected %A, actual %A" expected actual  
    | false -> passed
        
let assertIsGreaterThan target actual =
     match target >= actual with
     | true -> sprintf "Test failed, expected %A to be greater than %A" target actual
     | false -> passed 
        
// test runner
let runSingleTest (testName, testFunction) = 
    sprintf "%s... %s" testName (testFunction())       

let runTests testList =    
    testList |> List.map (runSingleTest)
    
let consoleTestRunner testList =
    runTests testList |> List.iter (printfn "%s")
    printfn "%s" "Ran all tests."