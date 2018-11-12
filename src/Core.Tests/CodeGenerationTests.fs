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

type MutableC =
    { mutable X: MutableA }

type MutableD =
    { mutable X: MutableB }

[<Fact>]
let ``"withInjectedDependency adds dependency`` () =
    //Arrange
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
    //Act
    let newTargetClass = targetClass |> withInjectedDependency dependency "fancyDep"

    //Assert
    let methods = newTargetClass.Methods |> Array.ofSeq
    methods |> should haveLength 1

[<Fact>]
let ``"buildClassFiles" builds a class file with correct name and main mapper inside it`` () =
    //Arrange
    let specifications: MappingRecords.MappingSpecification list =
      [{ Source = typeof<MutableA>
         Destination = typeof<MutableB> }]
    
    //Act
    let classFiles = specifications |> buildClassFiles
    
    //Assert
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

[<Fact>]
let ``"buildClassFiles" builds a class file with certain methods`` () =
    let specifications: MappingRecords.MappingSpecification list =
      [{ Source = typeof<MutableA>
         Destination = typeof<MutableB> }]
    let classFiles =
      specifications
      |> buildClassFiles
    
    classFiles |> should containf (fun x -> x.Name = "Mapper0")
    let classFile0 = classFiles |> Seq.find (fun x -> x.Name = "Mapper0")
    let class0 = classFile0.Classes |> Seq.head
    let methods = class0.Methods |> Array.ofSeq
    methods |> should haveLength 5
    (methods |> Seq.head).Signature.Name |> should equal "Mapper0"
    methods |> should containf (fun x -> x.Signature.Name = "CreateDestination")
    methods |> should containf (fun x -> x.Signature.Name = "Map")
    methods |> should containf (fun x -> x.Signature.Name = "MapFoo")
    methods |> should containf (fun x -> x.Signature.Name = "MapBar")

[<Fact>]
let ``"buildClassFiles" -> main mapper has a constructor with a global mapper interface as dependency`` () =
    //Arrange
    let specifications: MappingRecords.MappingSpecification list =
      [{ Source = typeof<MutableA>
         Destination = typeof<MutableB> }]
    
    //Act
    let classFiles = specifications |> buildClassFiles
    
    //Assert
    let classFile0 = classFiles |> Seq.find (fun x -> x.Name = "Mapper0")
    let class0 = classFile0.Classes |> Seq.head
    let method0 = class0.Methods |> Seq.head
    method0.Signature.Name |> should equal "Mapper0"
    method0.ReturnType |> should be null
    method0.Signature.Parameters |> should containf (fun (y: MethodParameter) -> y.Name = "mapperFetcher")
    class0.InstanceVariables |> should containf (fun (x: InstanceVariable) -> x.Name = "_mapperFetcher")
    method0.Body |> should containf (fun (y: Instruction) -> y.Code = "_mapperFetcher = mapperFetcher;")

[<Fact>]
let ``"buildClassFiles" -> main mapper has a method for creating destination object with certain instructions`` () =
    //Arrange
    let specifications: MappingRecords.MappingSpecification list =
      [{ Source = typeof<MutableA>
         Destination = typeof<MutableB> }]
    
    //Act
    let classFiles = specifications |> buildClassFiles
    
    //Assert
    let classFile0 = classFiles |> Seq.find (fun x -> x.Name = "Mapper0")
    let class0 = classFile0.Classes |> Seq.head
    let methodDefinition = class0.Methods |> Seq.find (fun x -> x.Signature.Name = "CreateDestination")
    methodDefinition.ReturnType |> should not' (be null)
    match methodDefinition.ReturnType with
        | Some x ->
            match x with
            | NonVoid y -> y.Name |> should equal "MutableB"
            | Void -> failwith "Expected that CreateDestination wasn't void, but it was."
        | None -> failwith "Expected that CreateDestination had a return type indication (including \"void\")"
    |> ignore

    methodDefinition.Signature.Parameters
    |> should containf (fun (x: MethodParameter) -> x.Name = "source" && x.ParameterType.Name = "MutableA")

    methodDefinition.Body |> should haveLength 1
    methodDefinition.Body |> should contain {Code = "return new MutableB();"}

[<Fact>]
let ``"buildClassFiles" -> main mapper has a method for mapping property \"Foo\" and property \"Bar\" of type MutableA`` () =
    //Arrange
    let specifications: MappingRecords.MappingSpecification list =
      [{ Source = typeof<MutableA>
         Destination = typeof<MutableB> }]
    
    //Act
    let classFiles = specifications |> buildClassFiles
    
    //Assert
    let classFile0 = classFiles |> Seq.find (fun x -> x.Name = "Mapper0")
    let class0 = classFile0.Classes |> Seq.head
    let methodDefinition1 = class0.Methods |> Seq.find (fun x -> x.Signature.Name = "MapFoo")
    methodDefinition1.ReturnType |> should not' (be null)
    match methodDefinition1.ReturnType with
        | Some x ->
            match x with
            | NonVoid y ->
                y.Name |> should equal "String"
                y.Namespace |> should equal (Some "System")
            | Void -> failwith "Expected that MapFoo wasn't void, but it was."
        | None -> failwith "Expected that MapFoo had a return type indication (including \"void\")"
    |> ignore

    methodDefinition1.Signature.Parameters
    |> should containf (fun (x: MethodParameter) -> x.Name = "source" && x.ParameterType.Name = "MutableA")

    methodDefinition1.Body |> should haveLength 1
    methodDefinition1.Body |> should contain {Code = "return source.Foo;"}

    let methodDefinition2 = class0.Methods |> Seq.find (fun x -> x.Signature.Name = "MapBar")
    methodDefinition2.ReturnType |> should not' (be null)
    match methodDefinition2.ReturnType with
        | Some x ->
            match x with
            | NonVoid y ->
                y.Name |> should equal "Int32"
                y.Namespace |> should equal (Some "System")
            | Void -> failwith "Expected that MapFoo wasn't void, but it was."
        | None -> failwith "Expected that MapFoo had a return type indication (including \"void\")"
    |> ignore

    methodDefinition2.Signature.Parameters
    |> should containf (fun (x: MethodParameter) -> x.Name = "source" && x.ParameterType.Name = "MutableA")

    methodDefinition2.Body |> should haveLength 1
    methodDefinition2.Body |> should contain {Code = "return source.Bar;"}

[<Fact>]
let ``"buildClassFiles" -> main mapper has a method for mapping complex property`` () =
    //Arrange
    let specifications: MappingRecords.MappingSpecification list =
      [{ Source = typeof<MutableA>
         Destination = typeof<MutableB>}
       { Source = typeof<MutableC>
         Destination = typeof<MutableD> }]
    
    //Act
    let classFiles = specifications |> buildClassFiles
    
    //Assert
    classFiles |> should containf (fun x -> x.Classes |> Seq.exists (fun y -> y.Methods |> Seq.exists (fun z -> z.Signature.Name = "MapX")))
    let classFile0 = classFiles |> Seq.find (fun x -> x.Classes |> Seq.exists (fun y -> y.Methods |> Seq.exists (fun z -> z.Signature.Name = "MapX")))
    let class0 = classFile0.Classes |> Seq.head
    let methods = class0.Methods |> Array.ofSeq
    methods |> should containf (fun x -> x.Signature.Name = "MapX")
    let methodDefinition = methods |> Array.find (fun x -> x.Signature.Name = "MapX")
    methodDefinition.ReturnType |> should not' (be null)
    match methodDefinition.ReturnType with
        | Some x ->
            match x with
            | NonVoid y -> y.Name |> should equal "MutableB"
            | Void -> failwith "Expected that CreateDestination wasn't void, but it was."
        | None -> failwith "Expected that CreateDestination had a return type indication (including \"void\")"
    |> ignore

    methodDefinition.Signature.Parameters
    |> should containf (fun (x: MethodParameter) -> x.Name = "source" && x.ParameterType.Name = "MutableC")

    methodDefinition.Body |> should haveLength 1
    methodDefinition.Body |> should contain {Code = "return _mapperFetcher().Map((MutableB x) => source.X);"}

