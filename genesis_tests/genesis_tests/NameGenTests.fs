module NameGenTests

open System.Collections.Generic

open TestFramework

// modules under test
open NameGenerator

// tests included in run
let nameGenTests = 
    [
        "countOcccurence will add a new entry for a new string occurence and init to 1",
        fun() ->
            let result = buildOccurenceTable ["abcdefg"]
            assertAreEqual 1 result.["a"];

        "countOcccurence will return 0 for a string that is not in the input",
        fun() ->
            let result = buildOccurenceTable ["abcdefg"]
            assertIsFalse (result.ContainsKey "z");    
    ]        
