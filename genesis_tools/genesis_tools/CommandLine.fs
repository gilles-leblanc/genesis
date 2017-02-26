module CommandLine

type MapGeneratorOptions = MidpointDisplacement | ValueNoise
type NameGeneratorOptions = { Length:int; FileName:string }
type NameSerializerOptions = { Length:int; InFileName:string; OutFileName:string }

type ToolsOption = 
        | MapGenerator of MapGeneratorOptions
        | NameGenerator of NameGeneratorOptions
        | NameSerializer of NameSerializerOptions
        | TerrainGenerator
        | NoOptions

type CommandLineOptions = { tool:ToolsOption }

let parseCommandLine args = 
    let error msgFunc =
        msgFunc
        { tool = NoOptions }     

    match args with 
    | [] -> error (printfn "No option specified")
    | "/map"::["midpoint"] -> { tool = MapGenerator MidpointDisplacement  }
    | "/map"::["valuenoise"] -> { tool = MapGenerator ValueNoise }
    | "/map"::xs -> error (printfn "/map is missing parameters" )
    | "/name"::l::[n] -> { tool = NameGenerator { Length=l |> int; FileName=n } }    
    | "/name"::xs -> error (printfn "/name is missing parameters")
    | "/serialize"::l::i::[o] -> { tool = NameSerializer { Length=l |> int; InFileName=i; OutFileName=o } }
    | "/serialize"::xs -> error (printfn "/serialize is missing parameters") 
    | "/terrain"::xs -> { tool = TerrainGenerator }
    | x::xs -> error (printfn "Option '%s' is unrecognized" x)