module CommandLine

type ToolsOption = MapGenerator | NameGenerator | NoOptions

type CommandLineOptions = { tool: ToolsOption }

let parseCommandLine args = 
    match args with 
    | [] -> 
        printfn "No option specified"
        { tool = NoOptions }
    | "/map"::xs -> { tool = MapGenerator }
    | "/name"::xs -> { tool = NameGenerator }    
    | x::xs ->  
        printfn "Option '%s' is unrecognized" x
        { tool = NoOptions }