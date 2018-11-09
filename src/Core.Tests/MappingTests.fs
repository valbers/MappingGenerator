module MappingTests

open System
open Xunit

open Mapping.Records
open Mapping.Operations

type MutableA =
    { mutable Foo: string
      mutable Bar: int 
      mutable Fizz: decimal }

type MutableB =
    { mutable Foo: string
      mutable Bar: int }

[<Fact>]
let ``"createMapping" maps properties with same name`` () =
    let mappingRules: MappingRule seq = (createMapping typeof<MutableA> typeof<MutableB>).PropertiesMappingRules
    Assert.Equal(2, mappingRules |> Seq.length)
    Assert.Equal("Foo", (mappingRules |> Seq.item 0).Source.Name)
    Assert.Equal("String", (mappingRules |> Seq.item 0).Source.Type.Name)
    Assert.Equal("Bar", (mappingRules |> Seq.item 1).Source.Name)
    Assert.Equal("Int32", (mappingRules |> Seq.item 1).Source.Type.Name)