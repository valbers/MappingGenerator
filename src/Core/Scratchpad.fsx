#load "Mapping\\Records.fs"
#load "Mapping\\Operations.fs"

open Mapping.Records
open Mapping.Operations
open Microsoft.FSharp.Collections

type A =
  { Foo: string
    Bar: int 
    Fizz: decimal }

type B =
  { Foo: string
    Bar: int }

//let mapping = createMapping typeof<A> typeof<B>

let mappingRules = createMappingRules typeof<A> typeof<B>

printfn "mappingRules 1 %A" mappingRules
