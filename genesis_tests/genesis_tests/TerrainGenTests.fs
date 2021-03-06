module TerrainGenTests

open TestFramework

// modules under test
open HeightMap
open MidpointDisplacement
open Terraform

// tests included in run
let terrainGenTests = 
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
            initCorners hm (System.Random())
            let size = hm.Size - 1
            let corners = [hm.Get 0 0; hm.Get 0 size; hm.Get size 0; hm.Get size size]
            let result = List.reduce (( + )) corners 
            assertIsGreaterThan 0.0 result;
            
        "normalize will constrain all values between 0.0 and 1.0",
        fun() ->
            let matrix = [| 2.0; 0.5; 1.0; -1.0; 0.0; 34.0; 23.0; 0.0; 0.0; -3.5; 0.0; 0.4; 0.3;
                            0.2; 1.1; -0.1; -0.2; -10.0; 10.0; 1.0; 0.2; 0.03; 0.004; 1.001; 0.8539 |]
            let normalized = matrix |> Array.map normalizeValue 
            assertAreEqual [| 1.0; 0.5; 1.0; 0.0; 0.0; 1.0; 1.0; 0.0; 0.0; 0.0; 0.0; 0.4; 0.3; 0.2; 
                              1.0; 0.0; 0.0; 0.0; 1.0; 1.0; 0.2; 0.03; 0.004; 1.0; 0.8539 |] normalized;
            
        "convertFloatToRgb will convert 0.0 to r:0, g:0, b:0",
        fun() ->
            let red, green, blue = convertFloatToRgb 0.0
            assertAreEqual (0, 0, 0) (red, green, blue);
            
        "convertFloatToRgb will convert 1.0 to r:255, g:255, b:255",
        fun() ->
            let red, green, blue = convertFloatToRgb 1.0
            assertAreEqual (255, 255, 255) (red, green, blue);                                               
            
        "convertFloatToRgb will convert 0.5 to r:127, g:127, b:127",
        fun() ->
            let red, green, blue = convertFloatToRgb 0.5
            assertAreEqual (127, 127, 127) (red, green, blue);
            
        "set will correctly change the values of an heightmap",
        fun() ->
            let hm = newHeightMap 2
            hm.Set 1 2 0.5
            assertAreEqual 0.5 (hm.Get 1 2)
            
        "middle will set the midpoint value between each corner to the average of the corners plus the result of a function",
        fun() ->
            let variationFunction x = x + 0.1
            let hm = newHeightMap 2
            hm.Set 0 0 0.5
            hm.Set 0 4 0.5
            hm.Set 4 0 1.0
            hm.Set 4 4 0.25
            middle hm (0, 0) (4, 0) (0, 4) (4, 4) variationFunction
            let middleValues = [hm.Get 0 2; hm.Get 2 0; hm.Get 2 4; hm.Get 4 2]
            assertAreEqual [0.6; 0.85; 0.475; 0.725] middleValues;
            
        "center will set the center point to the average of the four middle values between the corners plus the result of a function",
        fun() ->
            let variationFunction x = x + 0.1
            let hm = newHeightMap 2
            hm.Set 0 0 0.5
            hm.Set 0 4 0.5
            hm.Set 4 0 1.0
            hm.Set 4 4 0.25
            middle hm (0, 0) (4, 0) (0, 4) (4, 4) variationFunction
            center hm (0, 0) (4, 0) (0, 4) (4, 4) variationFunction
            let center = hm.Get 2 2
            assertAreEqual 0.7625 center;

        "findLowestNeighbors will return the neighboring point with the lowest value 1",
        fun() ->
            let hm = newHeightMap 3
            hm.Set 0 0 0.5
            hm.Set 0 1 0.9
            hm.Set 0 2 0.6
            hm.Set 1 0 0.3
            hm.Set 1 1 0.5
            hm.Set 1 2 0.35
            hm.Set 2 0 0.4
            hm.Set 2 1 0.6
            hm.Set 2 2 0.4
            let lowestNeighbor = findLowestNeighbors hm (1, 1) |> List.head
            assertAreEqual (1, 0) lowestNeighbor;

        "findLowestNeighbors will return the neighboring point with the lowest value 2",
        fun() ->
            let hm = newHeightMap 3
            hm.Set 0 0 0.5
            hm.Set 0 1 0.9
            hm.Set 0 2 0.6
            hm.Set 1 0 0.3
            hm.Set 1 1 0.5
            hm.Set 1 2 0.35
            hm.Set 2 0 0.4
            hm.Set 2 1 0.1
            hm.Set 2 2 0.4
            let lowestNeighbor = findLowestNeighbors hm (1, 1) |> List.head
            assertAreEqual (2, 1) lowestNeighbor;

        "findLowestNeighbors will return the neighboring point with the lowest value 3",
        fun() ->
            let hm = newHeightMap 3
            hm.Set 0 0 0.5
            hm.Set 0 1 0.01
            hm.Set 0 2 0.6
            hm.Set 1 0 0.3
            hm.Set 1 1 0.5
            hm.Set 1 2 0.35
            hm.Set 2 0 0.4
            hm.Set 2 1 0.6
            hm.Set 2 2 0.4
            let lowestNeighbor = findLowestNeighbors hm (1, 1) |> List.head
            assertAreEqual (0, 1) lowestNeighbor;

        "findLowestNeighbors will return the neighboring point with the lowest value 4",
        fun() ->
            let hm = newHeightMap 3
            hm.Set 0 0 0.5
            hm.Set 0 1 1.0
            hm.Set 0 2 0.6
            hm.Set 1 0 1.0
            hm.Set 1 1 0.1
            hm.Set 1 2 0.35
            hm.Set 2 0 0.4
            hm.Set 2 1 0.6
            hm.Set 2 2 0.2
            let lowestNeighbor = findLowestNeighbors hm (1, 1) |> List.head
            assertAreEqual (2, 2) lowestNeighbor;

        "findLowestNeighbors will return the neighboring point with the lowest value with invalid coordinates 1",
        fun() ->
            let hm = newHeightMap 3
            hm.Set 0 0 0.5
            hm.Set 0 1 0.9
            hm.Set 0 2 0.6
            hm.Set 1 0 0.3
            hm.Set 1 1 0.5
            hm.Set 1 2 0.35
            hm.Set 2 0 0.4
            hm.Set 2 1 0.6
            hm.Set 2 2 0.4
            let lowestNeighbor = findLowestNeighbors hm (0, 1) |> List.head
            assertAreEqual (1, 0) lowestNeighbor;

        "findLowestNeighbors will return the neighboring point with the lowest value with invalid coordinates 2",
        fun() ->
            let hm = newHeightMap 3
            hm.Set 0 0 0.5
            hm.Set 0 1 0.9
            hm.Set 0 2 0.6
            hm.Set 1 0 0.3
            hm.Set 1 1 0.5
            hm.Set 1 2 0.35
            hm.Set 2 0 0.4
            hm.Set 2 1 0.6
            hm.Set 2 2 0.4
            let lowestNeighbor = findLowestNeighbors hm (0, 0) |> List.head
            assertAreEqual (1, 0) lowestNeighbor;

        "findLowestNeighbors will return the two lowest neighboring points",
        fun() ->
            let hm = newHeightMap 3
            hm.Set 0 0 0.5
            hm.Set 0 1 1.0
            hm.Set 0 2 0.2
            hm.Set 1 0 1.0
            hm.Set 1 1 0.1
            hm.Set 1 2 0.35
            hm.Set 2 0 0.4
            hm.Set 2 1 0.6
            hm.Set 2 2 0.2
            let lowestNeighbors = findLowestNeighbors hm (1, 1) 
            assertAreEqual 2 (lowestNeighbors |> List.length);            

        "findLowestNeighbors will return the three lowest neighboring points",
        fun() ->
            let hm = newHeightMap 3
            hm.Set 0 0 0.5
            hm.Set 0 1 1.0
            hm.Set 0 2 0.2
            hm.Set 1 0 0.2
            hm.Set 1 1 0.1
            hm.Set 1 2 0.35
            hm.Set 2 0 0.4
            hm.Set 2 1 0.6
            hm.Set 2 2 0.2
            let lowestNeighbors = findLowestNeighbors hm (1, 1) 
            assertAreEqual 3 (lowestNeighbors |> List.length);            

        "heightmap.CoordValid will correctly indicate if a coordinate is valid 1",
        fun() ->
            let hm = newHeightMap 10
            assertIsTrue(hm.CoordValid 1023 1023);

        "heightmap.CoordValid will correctly indicate if a coordinate is valid 2",
        fun() ->
            let hm = newHeightMap 10
            assertIsFalse(hm.CoordValid 1024 1025);

        "heightmap Add will correctly add the value to the existing value",
        fun() ->
            let hm = newHeightMap 4
            hm.Set 1 1 0.5
            hm.Add 1 1 0.2
            assertAreEqual 0.7 (hm.Get 1 1);

        "heightmap Substract will correctly substract the value to the existing value",
        fun() ->
            let hm = newHeightMap 4
            hm.Set 1 1 0.5
            hm.Substract 1 1 0.2
            assertAreEqual 0.3 (hm.Get 1 1);
    ]