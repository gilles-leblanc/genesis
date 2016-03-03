module TerrainGen

open System.Drawing

open HeightMap  
open MidpointDisplacement
open TestFramework
open Tests

let heightMapToPng (heightMap:HeightMap) (filename:string) =
    let png = new Bitmap(heightMap.Size, heightMap.Size)
    for x in [0..heightMap.Size-1] do
        for y in [0..heightMap.Size-1] do
            // will need to convert float 1.0 to 255 with conversion function
            // let greyScale = heightMap.Get x y 
            png.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0))
    
    png.Save(filename, Imaging.ImageFormat.Png) |> ignore

[<EntryPoint>]
let main argv =
    consoleTestRunner testsToRun
    let map = newHeightMap 10
    heightMapToPng map "out.png"  
    0 
