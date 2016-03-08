module TerrainGen

open System.Drawing

open HeightMap  
open MidpointDisplacement
open TestFramework
open Tests

let heightMapToTxt (heightMap:HeightMap) (filename:string) =
    let out = Array.init (heightMap.Size * heightMap.Size) (fun e -> heightMap.Map.[e].ToString())
    System.IO.File.WriteAllLines(filename, out)

let heightMapToPng (heightMap:HeightMap) (filename:string) =
    let png = new Bitmap(heightMap.Size, heightMap.Size)
    for x in [0..heightMap.Size-1] do
        for y in [0..heightMap.Size-1] do
            let red, green, blue = convertFloatToRgb (heightMap.Get x y) 
            png.SetPixel(x, y, Color.FromArgb(255, red, green, blue))
    
    png.Save(filename, Imaging.ImageFormat.Png) |> ignore

[<EntryPoint>]
let main argv =
    consoleTestRunner testsToRun
    let map = newHeightMap 3
    generate map
    heightMapToPng map "out.png"
    heightMapToTxt map "out.txt"  
    0 
