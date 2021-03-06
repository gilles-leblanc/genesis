module NameGenTests

open System
open System.Collections.Generic

open TestFramework

// modules under test
open ProbabilityTable
open NameGenerator

// tests included in run
let nameGenTests = 
    [
        "countOcccurrence will add a new entry for a new string occurrence and init to 1",
        fun() ->
            let map = Map.empty
            let result = countOccurrences "abcdefg" map 1
            assertAreEqual 1.0 result.["a"];

        "countOcccurrence will return 0 for a string that is not in the input",
        fun() ->
            let map = Map.empty
            let result = countOccurrences "abcdefg" map 1
            assertIsFalse (result.ContainsKey "z");    

        "countOcccurrence will return 2 occurrences if a string occures twice",
        fun() ->
            let map = Map.empty
            let result = countOccurrences "aabcdefg" map 1
            assertAreEqual 2.0 result.["a"];

        "countOcccurrence will add a new entry for a new 2 character string occurrence and init to 1",
        fun() ->
            let map = Map.empty
            let result = countOccurrences "abcdefg" map 2
            assertAreEqual 1.0 result.["ab"];

        "countOcccurrence will return 2 occurrences if a 2 character string occures twice",
        fun() ->
            let map = Map.empty
            let result = countOccurrences "abcdeabfg" map 2
            assertAreEqual 2.0 result.["ab"];

        "countOcccurrence will return 0 for a string with 2 characters that is not in the input",
        fun() ->
            let map = Map.empty
            let result = countOccurrences "abcdefg" map 2
            assertIsFalse (result.ContainsKey "ba");    

        "countOcccurrence will add a new entry for a new 3 character string occurrence and init to 1",
        fun() ->
            let map = Map.empty
            let result = countOccurrences "abcdefg" map 3
            assertAreEqual 1.0 result.["abc"];

        "countOcccurrence will return 2 occurrences if a 3 character string occures twice",
        fun() ->
            let map = Map.empty
            let result = countOccurrences "abcdeabcfg" map 3
            assertAreEqual 2.0 result.["abc"];

        "countOcccurrence will return 0 for a string with 3 characters that is not in the input",
        fun() ->
            let map = Map.empty
            let result = countOccurrences "abcdefg" map 3
            assertIsFalse (result.ContainsKey "bca");

        "countOcccurrence will add a new entry for a new 4 character string occurrence and init to 1",
        fun() ->
            let map = Map.empty
            let result = countOccurrences "abcdefg" map 4
            assertAreEqual 1.0 result.["abcd"];

        "countOcccurrence will return 2 occurrences if a 4 character string occures twice",
        fun() ->
            let map = Map.empty
            let result = countOccurrences "abcdeabcdfg" map 4
            assertAreEqual 2.0 result.["abcd"];

        "countOcccurrence will return 0 for a string with 4 characters that is not in the input",
        fun() ->
            let map = Map.empty
            let result = countOccurrences "abcdefg" map 4
            assertIsFalse (result.ContainsKey "bcaa");

        "buildProbabilityTable will correctly compute 2 letter substrings 1",
        fun() ->
            let {probabilities = probabilities} = buildProbabilityTable " James John Max Gary Jess Gilles Mary " 2
            assertAreEqual 0.75 probabilities.["a"].["m"]

        "buildProbabilityTable will correctly compute 2 letter substrings 2",
        fun() ->
            let {probabilities = probabilities} = buildProbabilityTable " James John Max Gary Jess Gilles Mary " 2
            assertAreEqual 0.25 probabilities.["a"].["r"]

        "buildProbabilityTable will correctly compute 2 letter substrings 3",
        fun() ->
            let {probabilities = probabilities} = buildProbabilityTable " James John Max Gary Jess Gilles Mary " 2
            assertAreEqual 0.0 probabilities.["a"].["x"]

        "buildProbabilityTable will correctly compute 2 letter substrings 4",
        fun() ->
            let {probabilities = probabilities} = buildProbabilityTable " James John Max Gary Jess Gilles Mary " 2
            assertAreEqual 0.5 probabilities.["g"].["a"]

        "buildProbabilityTable will correctly compute 2 letter substrings 5",
        fun() ->
            let {probabilities = probabilities} = buildProbabilityTable " James John Max Gary Jess Gilles Mary " 2
            assertAreEqual 0.0 probabilities.["g"].["i"]

        "Names start with upper case letter",
        fun() ->
            let probabilities = buildProbabilityTable " James John Max Gary Jess Gilles Mary " 2
            let name = generateRandomName probabilities
            assertIsTrue (Char.IsUpper name.[0])

        "The rest of the name is in lower case",
        fun() ->
            let probabilities = buildProbabilityTable " James John Max Gary Jess Gilles Mary " 2
            let restOfName = (generateRandomName probabilities).[1..]
            assertIsTrue (Seq.forall Char.IsLower restOfName)     

        "Names will be at least 3 characters long",
        fun() -> 
            let probabilities = buildProbabilityTable " Aa Ab Ac Ba Bb Bc Ca " 2
            let name = generateRandomName probabilities
            assertIsGreaterThan 2 3
        ]        
