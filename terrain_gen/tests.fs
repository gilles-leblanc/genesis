module tests

open HeightMap
open MidpointDisplacement

let assertAreEqual expected actual =
    // todo check if need ()
    if (expected != actual) then failwith "Test failed"  // todo add sprintf or similar
    else true

// todo: change function name
let newHeightMap_will_return_0_initialized_height_map = 
    let hm = newHeightMap 5
    printfn newHeightMap.Map.ToString 
    // assertAreEqual 0 result
    ()
    