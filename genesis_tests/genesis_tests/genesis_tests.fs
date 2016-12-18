module GenesisTests

open TestFramework
open Tests

[<EntryPoint>]
let main argv =
    consoleTestRunner terrainGenTests
    printf "Ran all tests."
    0 // return an integer exit code
