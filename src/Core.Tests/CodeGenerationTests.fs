module CodeGenerationTests

open Xunit
open FsUnit.Xunit
open FsUnit.CustomMatchers

open CodeGenerationRecords
open CodeGenerationOperations
open MappingRecords

type MutableA =
    { mutable Foo: string
      mutable Bar: int
      mutable Fizz: decimal }

type MutableB =
    { mutable Foo: string
      mutable Bar: int }

type MutableC =
    { mutable X: MutableA
      mutable M: MutableA }

type MutableD =
    { mutable X: MutableB
      mutable N: MutableB }

type MutableA1   = {mutable Foo: string; mutable Bar: int}
type MutableA2   = {mutable Foo: string; mutable Bar: int}
type MutableA3   = {mutable Foo: string; mutable Bar: int}
type MutableA4   = {mutable Foo: string; mutable Bar: int}
type MutableA5   = {mutable Foo: string; mutable Bar: int}
type MutableA6   = {mutable Foo: string; mutable Bar: int}
type MutableA7   = {mutable Foo: string; mutable Bar: int}
type MutableA8   = {mutable Foo: string; mutable Bar: int}
type MutableA9   = {mutable Foo: string; mutable Bar: int}
type MutableA10  = {mutable Foo: string; mutable Bar: int}
type MutableA11  = {mutable Foo: string; mutable Bar: int}
type MutableA12  = {mutable Foo: string; mutable Bar: int}
type MutableA13  = {mutable Foo: string; mutable Bar: int}
type MutableA14  = {mutable Foo: string; mutable Bar: int}
type MutableA15  = {mutable Foo: string; mutable Bar: int}
type MutableA16  = {mutable Foo: string; mutable Bar: int}
type MutableA17  = {mutable Foo: string; mutable Bar: int}
type MutableA18  = {mutable Foo: string; mutable Bar: int}
type MutableA19  = {mutable Foo: string; mutable Bar: int}
type MutableA20  = {mutable Foo: string; mutable Bar: int}
type MutableA21  = {mutable Foo: string; mutable Bar: int}

type MutableB1   = {mutable Foo: string; mutable Bar: int}
type MutableB2   = {mutable Foo: string; mutable Bar: int}
type MutableB3   = {mutable Foo: string; mutable Bar: int}
type MutableB4   = {mutable Foo: string; mutable Bar: int}
type MutableB5   = {mutable Foo: string; mutable Bar: int}
type MutableB6   = {mutable Foo: string; mutable Bar: int}
type MutableB7   = {mutable Foo: string; mutable Bar: int}
type MutableB8   = {mutable Foo: string; mutable Bar: int}
type MutableB9   = {mutable Foo: string; mutable Bar: int}
type MutableB10  = {mutable Foo: string; mutable Bar: int}
type MutableB11  = {mutable Foo: string; mutable Bar: int}
type MutableB12  = {mutable Foo: string; mutable Bar: int}
type MutableB13  = {mutable Foo: string; mutable Bar: int}
type MutableB14  = {mutable Foo: string; mutable Bar: int}
type MutableB15  = {mutable Foo: string; mutable Bar: int}
type MutableB16  = {mutable Foo: string; mutable Bar: int}
type MutableB17  = {mutable Foo: string; mutable Bar: int}
type MutableB18  = {mutable Foo: string; mutable Bar: int}
type MutableB19  = {mutable Foo: string; mutable Bar: int}
type MutableB20  = {mutable Foo: string; mutable Bar: int}
type MutableB21  = {mutable Foo: string; mutable Bar: int}

let bigMappingSpecifications: MappingSpecification list = 
    [
        {
            Source = typeof<MutableA1>
            Destination = typeof<MutableB1>
        }
        {
            Source = typeof<MutableA2>
            Destination = typeof<MutableB2>
        }
        {
            Source = typeof<MutableA3>
            Destination = typeof<MutableB3>
        }
        {
            Source = typeof<MutableA4>
            Destination = typeof<MutableB4>
        }
        {
            Source = typeof<MutableA5>
            Destination = typeof<MutableB5>
        }
        {
            Source = typeof<MutableA6>
            Destination = typeof<MutableB6>
        }
        {
            Source = typeof<MutableA7>
            Destination = typeof<MutableB7>
        }
        {
            Source = typeof<MutableA8>
            Destination = typeof<MutableB8>
        }
        {
            Source = typeof<MutableA9>
            Destination = typeof<MutableB9>
        }
        {
            Source = typeof<MutableA10>
            Destination = typeof<MutableB10>
        }
        {
            Source = typeof<MutableA11>
            Destination = typeof<MutableB11>
        }
        {
            Source = typeof<MutableA12>
            Destination = typeof<MutableB2>
        }
        {
            Source = typeof<MutableA13>
            Destination = typeof<MutableB13>
        }
        {
            Source = typeof<MutableA14>
            Destination = typeof<MutableB14>
        }
        {
            Source = typeof<MutableA15>
            Destination = typeof<MutableB15>
        }
        {
            Source = typeof<MutableA16>
            Destination = typeof<MutableB16>
        }
        {
            Source = typeof<MutableA17>
            Destination = typeof<MutableB17>
        }
        {
            Source = typeof<MutableA18>
            Destination = typeof<MutableB18>
        }
        {
            Source = typeof<MutableA19>
            Destination = typeof<MutableB19>
        }
        {
            Source = typeof<MutableA20>
            Destination = typeof<MutableB20>
        }
        {
            Source = typeof<MutableA21>
            Destination = typeof<MutableB21>
        }
    ]

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
    classFiles |> should containf (fun (x:ClassFile) -> x.Name = "Mapper0")
    let classFile0 = classFiles |> Seq.find (fun (x:ClassFile) -> x.Name = "Mapper0")
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
    
    classFiles |> should containf (fun (x:ClassFile) -> x.Name = "Mapper0")
    let classFile0 = classFiles |> Seq.find (fun (x:ClassFile) -> x.Name = "Mapper0")
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
    let classFile = classFiles |> Seq.find (fun x -> x.Classes |> Seq.exists (fun y -> y.Methods |> Seq.exists (fun z -> z.Signature.Name = "MapX")))
    let ``class`` = classFile.Classes |> Seq.head
    let methods = ``class``.Methods |> Array.ofSeq
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

[<Fact>]
let ``"buildClassFiles" -> method for mapping complex property`` () =
    //Arrange
    let specifications: MappingRecords.MappingSpecification list =
      [{ Source = typeof<MutableA>
         Destination = typeof<MutableB>}
       { Source = typeof<MutableC>
         Destination = typeof<MutableD> }]
    
    //Act
    let classFiles = specifications |> buildClassFiles
    
    //Assert
    classFiles |> should containf (fun (x:ClassFile) -> x.Name = "Mapper1")
    let classFile1 = classFiles |> Seq.find (fun (x:ClassFile) -> x.Name = "Mapper1")
    let class1 = classFile1.Classes |> Seq.head
    let methods = class1.Methods |> Array.ofSeq
    methods |> should haveLength 4
    (methods |> Seq.head).Signature.Name |> should equal "Mapper1"
    methods |> should containf (fun x -> x.Signature.Name = "CreateDestination")
    methods |> should containf (fun x -> x.Signature.Name = "Map")
    methods |> should containf (fun x -> x.Signature.Name = "MapX")

[<Fact>]
let ``"buildClassFiles" -> a correct number of classes files are generated`` () =
    //Arrange
    let specifications = bigMappingSpecifications
    bigMappingSpecifications |> should not' (be null)
    //Act
    let classFiles =
        specifications
        |> buildClassFiles
        |> Array.ofSeq

    //Assert
    classFiles.Length |> should equal 27
    let classFileNames = classFiles |> Seq.map (fun (x:ClassFile) -> x.Name) |> Array.ofSeq
    [0..2] |> Seq.iter (fun i -> classFileNames |> should contain (sprintf "MapperBases%d" i))
    [0..20] |> Seq.iter (fun i -> classFileNames |> should contain (sprintf "Mapper%d" i))
    classFileNames |> should contain (sprintf "IMapper")
    classFileNames |> should contain (sprintf "Mapper")

[<Fact>]
let ``"buildClassFiles" -> base mapping class files have at most ten classes each`` () =
    //Arrange
    let specifications = bigMappingSpecifications

    //Act
    let classFiles = specifications |> buildClassFiles |> Array.ofSeq

    //Assert
    let classFileNames = classFiles |> Array.ofSeq |> Seq.map (fun (x:ClassFile) -> x.Name)
    [0..2] |> Seq.iter (fun i -> classFileNames |> should contain (sprintf "MapperBases%d" i))

    let mapperBases =
        classFiles
        |> Seq.filter (fun (x:ClassFile) -> x.Name.StartsWith "MapperBases")
    
    mapperBases |> Array.ofSeq |> should haveLength 3
    mapperBases
        |> Seq.filter (fun (x:ClassFile) ->
                            let classes = x.Classes |> Array.ofSeq
                            classes.Length = 10)
        |> Array.ofSeq |> should haveLength 2
    mapperBases
        |> Seq.filter (fun (x:ClassFile) ->
                            let classes = x.Classes |> Array.ofSeq
                            classes.Length = 1)
        |> Array.ofSeq |> should haveLength 1
    

[<Fact>]
let ``"buildClassFiles" -> each mapping has an own class file`` () =
    //Arrange
    let specifications = bigMappingSpecifications

    //Act
    let classFiles = specifications |> buildClassFiles

    //Assert
    let classFileNames = classFiles |> Seq.map (fun x -> x.Name) |> Array.ofSeq
    [0..20] |> Seq.iter (fun i -> classFileNames |> should contain (sprintf "Mapper%d" i))
