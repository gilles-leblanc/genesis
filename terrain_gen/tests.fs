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
            // this test will fail if random returns 0.0 for all corners
            let hm = newHeightMap 5
            initCorners hm
            let result = Array.reduce (fun acc elem -> acc + elem) hm.Map 
            assertIsGreaterThan 0.0 result
    ]
