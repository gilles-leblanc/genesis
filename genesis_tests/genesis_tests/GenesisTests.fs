module GenesisTests

open TestFramework
open TerrainGenTests
open NameGenTests

[<EntryPoint>]
let main argv =
    consoleTestRunner terrainGenTests
    consoleTestRunner nameGenTests
    0 // return an integer exit code
