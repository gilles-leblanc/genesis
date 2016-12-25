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
            let dict = new Dictionary<string, int>()
            let result = countOccurences "abcdefg" dict 1
            assertAreEqual 1 result.["a"];

        "countOcccurence will return 0 for a string that is not in the input",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurences "abcdefg" dict 1
            assertIsFalse (result.ContainsKey "z");    

        "countOcccurence will return 2 occurrences if a string occures twice",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurences "aabcdefg" dict 1
            assertAreEqual 2 result.["a"];

        "countOcccurence will add a new entry for a new 2 character string occurence and init to 1",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurences "abcdefg" dict 2
            assertAreEqual 1 result.["ab"];

        "countOcccurence will return 2 occurences if a 2 character string occures twice",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurences "abcdeabfg" dict 2
            assertAreEqual 2 result.["ab"];

        "countOcccurence will return 0 for a string with 2 characters that is not in the input",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurences "abcdefg" dict 2
            assertIsFalse (result.ContainsKey "ba");    

        "countOcccurence will add a new entry for a new 3 character string occurence and init to 1",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurences "abcdefg" dict 3
            assertAreEqual 1 result.["abc"];

        "countOcccurence will return 2 occurences if a 3 character string occures twice",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurences "abcdeabcfg" dict 3
            assertAreEqual 2 result.["abc"];

        "countOcccurence will return 0 for a string with 3 characters that is not in the input",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurences "abcdefg" dict 3
            assertIsFalse (result.ContainsKey "bca");

        "countOcccurence will add a new entry for a new 4 character string occurence and init to 1",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurences "abcdefg" dict 4
            assertAreEqual 1 result.["abcd"];

        "countOcccurence will return 2 occurences if a 4 character string occures twice",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurences "abcdeabcdfg" dict 4
            assertAreEqual 2 result.["abcd"];

        "countOcccurence will return 0 for a string with 4 characters that is not in the input",
        fun() ->
            let dict = new Dictionary<string, int>()
            let result = countOccurences "abcdefg" dict 4
            assertIsFalse (result.ContainsKey "bcaa");
    ]        
