module NameGenTests

open System.Collections.Generic

open TestFramework

// modules under test
open NameGenerator

// tests included in run
let nameGenTests = 
    [
        "countOcccurrence will add a new entry for a new string occurrence and init to 1",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurrences "abcdefg" dict 1
            assertAreEqual 1 result.["a"];

        "countOcccurrence will return 0 for a string that is not in the input",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurrences "abcdefg" dict 1
            assertIsFalse (result.ContainsKey "z");    

        "countOcccurrence will return 2 occurrences if a string occures twice",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurrences "aabcdefg" dict 1
            assertAreEqual 2 result.["a"];

        "countOcccurrence will add a new entry for a new 2 character string occurrence and init to 1",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurrences "abcdefg" dict 2
            assertAreEqual 1 result.["ab"];

        "countOcccurrence will return 2 occurrences if a 2 character string occures twice",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurrences "abcdeabfg" dict 2
            assertAreEqual 2 result.["ab"];

        "countOcccurrence will return 0 for a string with 2 characters that is not in the input",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurrences "abcdefg" dict 2
            assertIsFalse (result.ContainsKey "ba");    

        "countOcccurrence will add a new entry for a new 3 character string occurrence and init to 1",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurrences "abcdefg" dict 3
            assertAreEqual 1 result.["abc"];

        "countOcccurrence will return 2 occurrences if a 3 character string occures twice",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurrences "abcdeabcfg" dict 3
            assertAreEqual 2 result.["abc"];

        "countOcccurrence will return 0 for a string with 3 characters that is not in the input",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurrences "abcdefg" dict 3
            assertIsFalse (result.ContainsKey "bca");

        "countOcccurrence will add a new entry for a new 4 character string occurrence and init to 1",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurrences "abcdefg" dict 4
            assertAreEqual 1 result.["abcd"];

        "countOcccurrence will return 2 occurrences if a 4 character string occures twice",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurrences "abcdeabcdfg" dict 4
            assertAreEqual 2 result.["abcd"];

        "countOcccurrence will return 0 for a string with 4 characters that is not in the input",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurrences "abcdefg" dict 4
            assertIsFalse (result.ContainsKey "bcaa");
    ]        
