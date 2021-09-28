// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

let toImageTag (imgFilename: string) =
    //if (idx % 24 = 0) then
    //    if (idx <> 0) then
    //        "</div>"
    //    "<div class=\"grid-container\">"
    $"<div class=\"grid-item\"><img src=\"{imgFilename}\"/></div>"

let wrapWithContainer (divs:string seq) =
    "<div class=\"grid-container\">\n"
    + String.Join( "\n",divs)
    + "</div>\n"
[<EntryPoint>]
let main argv =
    let inputFolder = argv.[0]
    let gridContainers = 
        Directory.GetFiles(inputFolder)
        |> Seq.map Path.GetFileName
        |> Seq.filter (fun f -> f.EndsWith("jpg"))
        |> Seq.map toImageTag
        |> Seq.chunkBySize 24
        |> Seq.map wrapWithContainer
        |> Seq.toArray
        
    let templateContents = 
        File.ReadAllText("index-template.html")
    let resultFileContents = 
        String.Format(templateContents, String.Join( "\n", gridContainers))
    
    let indexFilename = Path.Combine(inputFolder, "imagesIndex.html") 
    File.WriteAllText(indexFilename, resultFileContents)
    0 // return an integer exit code