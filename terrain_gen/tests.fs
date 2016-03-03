module Tests

open TestFramework

// modules under test
open HeightMap
open MidpointDisplacement

// tests included in run
let testsToRun = 
    [
        "newHeightMap will return a 0 initialized height map",
        fun() ->
            let hm = newHeightMap 5
            let result = hm.Map |> Array.sum
            assertAreEqual 0.0 result;
        
        "set will update height map",
        fun() ->
            let hm = newHeightMap 5
            hm.Set 1 1 0.5
            assertAreEqual 0.5 (hm.Get 1 1);
        
        "init corners will assign values to the height map corners",
        fun() ->
            // this test will fail if random returns 0.0 for all corners (very improbable but possible)
            let hm = newHeightMap 5
            initCorners hm
            let size = hm.Size - 1
            let corners = [hm.Get 0 0; hm.Get 0 size; hm.Get size 0; hm.Get size size]
            let result = List.reduce (fun acc elem -> acc + elem) corners 
            assertIsGreaterThan 0.0 result;
            
        "normalize will constrain all values between 0.0 and 1.0",
        fun() ->
            let matrix = [| 2.0; 0.5; 1.0; -1.0; 0.0; 34.0; 23.0; 0.0; 0.0; -3.5; 0.0; 0.4; 0.3; 0.2; 1.1; -0.1; -0.2; -10.0; 10.0; 1.0; 0.2; 0.03; 0.004; 1.001; 0.8539 |]
            let normalized = normalize matrix 
            assertAreEqual [| 1.0; 0.5; 1.0; 0.0; 0.0; 1.0; 1.0; 0.0; 0.0; 0.0; 0.0; 0.4; 0.3; 0.2; 1.0; 0.0; 0.0; 0.0; 1.0; 1.0; 0.2; 0.03; 0.004; 1.0; 0.8539 |] normalized                                               
    ]
