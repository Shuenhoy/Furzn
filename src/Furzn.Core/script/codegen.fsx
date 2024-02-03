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

let dims = (Seq.init 4 (fun i -> sprintf "%d" (i + 1)))
let context = TemplateContext({| dims = dims |}, options)

let result = template.Render(context)
File.WriteAllText(args[3], result)
