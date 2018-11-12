module MappingTests

open System
open Xunit
open FsUnit.Xunit
open FsUnit.CustomMatchers

open MappingRecords
open MappingOperations

type MutableA =
    { mutable Foo: string
      mutable Bar: int 
      mutable Fizz: decimal }

type MutableB =
    { mutable Foo: string
      mutable Bar: int
      mutable Feez: decimal }

[<Fact>]
let ``"createMapping" maps properties with same name`` () =
    let mappingRules = (createMapping typeof<MutableA> typeof<MutableB>).PropertiesMappingRules |> Array.ofSeq
    mappingRules |> should haveLength 2
    Assert.Equal("Foo", (mappingRules |> Seq.item 0).Source.Name)
    Assert.Equal("String", (mappingRules |> Seq.item 0).Source.Type.Name)
    Assert.Equal("Bar", (mappingRules |> Seq.item 1).Source.Name)
    Assert.Equal("Int32", (mappingRules |> Seq.item 1).Source.Type.Name)
