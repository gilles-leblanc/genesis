module GenesisTests

open TestFramework
open Tests

[<EntryPoint>]
let main argv =
    consoleTestRunner terrainGenTests
    0 // return an integer exit code
