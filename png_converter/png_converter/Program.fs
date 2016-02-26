open System.Drawing

let convertLine (y:int) (bitmap:System.Drawing.Bitmap) =
    [ for x in 0 .. bitmap.Width - 1 -> "\"" + bitmap.GetPixel(x, y).R.ToString() + "\"" ] |> String.concat ", "

let convert (path:string) : string = 
    let bitmap = new System.Drawing.Bitmap(path)
    let results = [ for y in 0 .. bitmap.Height - 1 -> convertLine y bitmap ] |> String.concat ", "
   
    "{\"X\": \"" + bitmap.Width.ToString() + 
    "\", \"Y\": \"" + bitmap.Width.ToString() + 
    "\", \"values\":[" + results + "]}"

[<EntryPoint>]
let main argv = 
    let arglist = argv |> List.ofSeq

    let returnValue = 
        match arglist with
            | path :: _ -> convert path
            | _ -> "Wrong number of arguments supplied"

    printfn "%s" returnValue
    
    match returnValue with
        | "Wrong number of arguments supplied" -> 1
        | _ -> 0