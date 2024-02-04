#r "nuget: Fluid.Core, 2.5.0"

open Fluid
open System
open System.Threading.Tasks
open System.IO

let args = Environment.GetCommandLineArgs()
let sourcePath = args[2]
let source = File.ReadAllText(sourcePath)
let parse = FluidParser(FluidParserOptions(AllowFunctions = true))
let template = parse.Parse(source)
let options = TemplateOptions()
options.MemberAccessStrategy <- UnsafeMemberAccessStrategy.Instance

let dims = [ "2"; "3"; "4" ]
let context = TemplateContext({| dims = dims |}, options)


let outputs
    (input: Values.FluidValue)
    (arguments: FilterArguments)
    (context: TemplateContext)
    : ValueTask<Values.FluidValue> =
    let input = input.ToStringValue()

    let args =
        arguments.Values |> Seq.map (fun x -> x.ToStringValue() :> obj) |> Seq.toArray

    ValueTask.FromResult(Values.StringValue(String.Format(input, args)))

options.Filters.AddFilter("format", outputs)

let result = template.Render(context)
File.WriteAllText(args[3], result)
