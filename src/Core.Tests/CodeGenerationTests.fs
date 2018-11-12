module CodeGenerationTests

open Xunit
open FsUnit.Xunit
open FsUnit.CustomMatchers

open CodeGenerationRecords
open CodeGenerationOperations

type MutableA =
    { mutable Foo: string
      mutable Bar: int
      mutable Fizz: decimal }

type MutableB =
    { mutable Foo: string
      mutable Bar: int }

[<Fact>]
let ``"withInjectedDependency adds dependency`` () =
    let targetClass =
        { IsInterface = false
          Namespace = None
          AccessModifier = AccessModifier.Public
          OtherModifiers = Seq.empty
          Name = "TheTargetClass"
          BaseClass = None
          InstanceVariables = Seq.empty
          Methods = Seq.empty
          GenericArguments = Seq.empty
          IsConcreteType = false }
    let dependency =
        { IsInterface = false
          Namespace = None
          AccessModifier = AccessModifier.Public
          OtherModifiers = Seq.empty
          Name = "FancyDep"
          BaseClass = None
          InstanceVariables = Seq.empty
          Methods = Seq.empty
          GenericArguments = Seq.empty
          IsConcreteType = false }

    let newTargetClass = targetClass |> withInjectedDependency dependency "fancyDep"
    let methods = newTargetClass.Methods |> Array.ofSeq
    methods |> should haveLength 1

[<Fact>]
let ``"buildClassFiles" builds a class file given a mapping specification`` () =
    let specifications: MappingRecords.MappingSpecification list =
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
    let methods = class0.Methods |> Array.ofSeq
    methods |> should haveLength 5
    (methods |> Seq.head).Signature.Name |> should equal "Mapper0"
    methods |> should containf (fun x -> x.Signature.Name = "CreateDestination")
    methods |> should containf (fun x -> x.Signature.Name = "Map")
    methods |> should containf (fun x -> x.Signature.Name = "MapFoo")
    methods |> should containf (fun x -> x.Signature.Name = "MapBar")
    let instanceVariables = class0.InstanceVariables |> Array.ofSeq
    instanceVariables |> should haveLength 1
    let instanceVariable = instanceVariables |> Array.head
    instanceVariable.Name |> should equal "_mapperFetcher"
    methods
    |> Seq.filter (fun x ->
                    x.ReturnType = None &&
                    x.Signature.Name = class0.Name &&
                    x.Signature.Parameters |> Seq.exists (fun y -> y.Name = "mapperFetcher"))
    |> Seq.iter (fun x -> x.Body |> should containf (fun (y: Instruction) -> y.Code = "_mapperFetcher = mapperFetcher;"))
