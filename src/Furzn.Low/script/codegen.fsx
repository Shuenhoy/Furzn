#r "nuget: Fluid.Core, 2.5.0"

open Fluid
open System
open System.IO

let args = Environment.GetCommandLineArgs()
let sourcePath = args[2]
let source = File.ReadAllText(sourcePath)
let parse = FluidParser()
let template = parse.Parse(source)
let options = TemplateOptions()
options.MemberAccessStrategy <- UnsafeMemberAccessStrategy.Instance

let dims =
    Seq.allPairs [ 1..4 ] [ 1..4 ]
    |> Seq.map (fun (x, y) -> x * y)
    |> Seq.distinct
    |> Seq.toList

let context = TemplateContext({| dims = dims |}, options)

let result = template.Render(context)
File.WriteAllText(args[3], result)
