module CodeGenerationTests

open Xunit
open CodeGeneration.Records
open CodeGeneration.Operations
open FsUnit.Xunit
open FsUnit.CustomMatchers

type MutableA =
    { mutable Foo: string
      mutable Bar: int
      mutable Fizz: decimal }

type MutableB =
    { mutable Foo: string
      mutable Bar: int }

[<Fact>]
let ``"buildClassFiles" builds a class file given a mapping specification`` () =
    let specifications: Mapping.Records.MappingSpecification list =
      [{ Source = typeof<MutableA>
         Destination = typeof<MutableB> }]
    let classFiles =
      specifications
      |> buildClassFiles
    
    classFiles |> should containf (fun x -> x.Name = "Mapper0")
    let classFile0 = classFiles |> Seq.find (fun x -> x.Name = "Mapper0")
    
    let classes = Array.ofSeq classFile0.Classes
    classes |> should haveLength 1

    let class0 = classes |> Array.head
    class0.Name |> should equal "Mapper0"
    class0.AccessModifier |> should equal AccessModifier.Public
    class0.IsConcreteType |> should not' (be True)
    class0.IsInterface |> should not' (be True)
    class0.Namespace |> should be null
    class0.OtherModifiers |> should contain Modifier.Partial
    //class0.Methods |> Array.ofSeq |> should haveLength 4
