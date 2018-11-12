module CodeGenerationOperations

open MappingRecords
open CodeGenerationRecords

let private instructToSetAVariable variableName value = { Code = sprintf "%s = %s;" variableName value }

let private newConstructorWithDependency (dependency: ClassDefinition) (dependencyName: string) (targetClass: ClassDefinition): MethodDefinition =
    { ReturnType = None
      AccessModifier = AccessModifier.Public
      OtherModifiers = Seq.empty
      Signature = { Name = targetClass.Name
                    GenericArguments = Seq.empty
                    Parameters = Seq.ofList [{Name = dependencyName; ParameterType = dependency}] }
      Body = Seq.ofList [ instructToSetAVariable (Conventions.fieldName dependencyName) dependencyName ]
    }

let withInjectedDependency (dependency: ClassDefinition) (dependencyName: string) (targetClass: ClassDefinition) =
    let dependencyFieldName = Conventions.fieldName dependencyName
    let instanceVariables =
        targetClass.InstanceVariables
        |> Seq.append (Seq.ofList [{ InstanceVariableType = InstanceVariableType.Field
                                     AccessModifier = AccessModifier.Private
                                     OtherModifiers = Seq.empty
                                     Type = dependency
                                     Name = dependencyFieldName }])

    let withAInstructionToSetTheDependency originalBody =
        originalBody
        |> Seq.append [instructToSetAVariable dependencyFieldName dependencyName]

    let dependencyParameter = {Name = dependencyName; ParameterType = dependency};
    let methods =
        targetClass.Methods
        |> Seq.map (fun x -> match x.ReturnType with
                                | Some _ -> x
                                | None -> { x with
                                                Body = x.Body |> withAInstructionToSetTheDependency
                                                Signature = 
                                                    { x.Signature with
                                                        Parameters =
                                                            [dependencyParameter]
                                                            |> Seq.append x.Signature.Parameters }})

    let finalMethods =
        if methods |> Seq.exists (fun x -> x.ReturnType = None) then methods
        else methods |> Seq.append [newConstructorWithDependency dependency dependencyName targetClass]

    { targetClass with
        InstanceVariables = instanceVariables
        Methods = finalMethods }

let private convertTypeToClassDef (theType: System.Type): ClassDefinition =
   { Name = "IMapper"
     Namespace = Some "AutoGeneration"
     IsInterface = true
     GenericArguments = Seq.empty
     AccessModifier = AccessModifier.Public
     OtherModifiers = Seq.empty
     BaseClass = None
     InstanceVariables = Seq.empty
     IsConcreteType = false
     Methods = Seq.empty }

let private createGlobalMapperInterfaceDefinition (mappingSpecifications: MappingSpecification seq) =
    let globalMapperInterface = Conventions.globalMapperInterfaceDefinition
    let res, _ =
        mappingSpecifications
        |> Seq.fold (fun state input ->
                        let curr, index = state
                        let dependencyName = sprintf "m%d" index
                        let individualMapperInterfaceDef =
                            Conventions.individualMapperInterfaceDefinition (convertTypeToClassDef input.Source) (convertTypeToClassDef input.Destination)
                        (withInjectedDependency individualMapperInterfaceDef dependencyName curr), index + 1) (globalMapperInterface, 0)
    res 

let private createGlobalMapperClassDefinition (mappingSpecifications: MappingSpecification seq) =
    let globalMapperInterface = createGlobalMapperInterfaceDefinition mappingSpecifications
    { globalMapperInterface with
        IsInterface = false
        Name = globalMapperInterface.Name.TrimStart('I') }

let rec private typeToClassDefinition (theType: System.Type): ClassDefinition =
    { IsConcreteType = true
      IsInterface = theType.IsInterface
      Namespace =
        match theType.Namespace with
        | x when not (x = null || x = System.String.Empty)  -> Some x
        | _ -> None
      Name = match theType.IsGenericType with
             | true -> theType.Name
             | false -> theType.Name.Split('`') |> Seq.item 0
      GenericArguments = match theType.IsGenericType with
                         | true -> theType.GetGenericArguments() |> (Seq.map typeToClassDefinition)
                         | false -> Seq.empty
      AccessModifier = AccessModifier.Public
      OtherModifiers = Seq.empty
      BaseClass = None
      InstanceVariables = Seq.empty
      Methods = Seq.empty }

let rec private getFullyQualifiedNameIncludingGenerics (classDef: ClassDefinition) =
    let withoutGenericPartYet =
        match classDef.Namespace with
        | Some x -> sprintf "%s.%s" x classDef.Name
        | None -> classDef.Name
    match classDef.GenericArguments with
    | genericArgs when not (genericArgs = Seq.empty) ->
                  sprintf "%s<%s>" withoutGenericPartYet (System.String.Join(", ", genericArgs |> Seq.map getFullyQualifiedNameIncludingGenerics))
    | _ -> withoutGenericPartYet

let createMappingClass (name: string) (theyHaveAMappingSpecification: System.Type*System.Type -> bool) (mapping: Mapping) : ClassDefinition =
    let baseClass =
        { AccessModifier = AccessModifier.Public
          OtherModifiers = [Modifier.Abstract; Modifier.Partial]
          IsInterface = false
          Namespace = None
          Name = (sprintf "%sBase" name)
          InstanceVariables = Seq.empty<InstanceVariable>
          Methods = Seq.empty<MethodDefinition>
          GenericArguments = Seq.empty<ClassDefinition>
          BaseClass = None
          IsConcreteType = false }
    
    let destinationAsClassDef = mapping.Destination |> typeToClassDefinition
    let sourceAsClassDef = mapping.Source |> typeToClassDefinition
    let methodForCreateDestination: MethodDefinition =
        { AccessModifier = AccessModifier.Protected
          OtherModifiers = [Modifier.Virtual]
          ReturnType = Some (NonVoid destinationAsClassDef)
          Signature =
            { Name = "CreateDestination"
              Parameters = [{Name= "source"; ParameterType = sourceAsClassDef}]
              GenericArguments = Seq.empty}
          Body = [ { Code = sprintf "return new %s();" (getFullyQualifiedNameIncludingGenerics destinationAsClassDef) } ]
        }

    let systemFuncDef (genericArguments: ClassDefinition seq): ClassDefinition =
        { AccessModifier = AccessModifier.Public
          OtherModifiers = Seq.empty
          IsInterface = false
          Namespace = Some "System"
          Name = "Func"
          InstanceVariables = Seq.empty<InstanceVariable>
          Methods = Seq.empty<MethodDefinition>
          GenericArguments = genericArguments
          BaseClass = None
          IsConcreteType = true }

    let (|BothOfSameType|_|) (sourceType: System.Type, destinationType: System.Type) =
        match sourceType, destinationType with
        | (srcType, destType) when srcType = destType -> Some (srcType, destType)
        | _ -> None

    let (|TheyHaveAMappingSpecification|_|) (sourceType: System.Type, destinationType: System.Type) =
        match sourceType, destinationType with
        | (srcType, destType) when (theyHaveAMappingSpecification (srcType, destType)) -> Some (srcType, destType)
        | _ -> None
    
    let propertyMappingMethod (mappingRule: MappingRule): MethodDefinition =
        { AccessModifier = AccessModifier.Public
          OtherModifiers = [Modifier.Virtual]
          ReturnType = Some (typeToClassDefinition mappingRule.Destination.Type |> NonVoid)
          Signature =
            { Name = sprintf "Map%s" mappingRule.Source.Name
              Parameters = [ {Name = "source"; ParameterType = typeToClassDefinition mapping.Source} ]
              GenericArguments = Seq.empty
            }
          Body = match mappingRule.Source.Type, mappingRule.Destination.Type with
                 | BothOfSameType (_, _) -> [{Code = sprintf "return source.%s;" mappingRule.Source.Name}]
                 | TheyHaveAMappingSpecification (_, d) ->
                    [{
                        Code = 
                            sprintf "return %s().Map((%s x) => source.%s);"
                                (Conventions.fieldName "mapperFetcher")
                                (d |> typeToClassDefinition |> getFullyQualifiedNameIncludingGenerics)
                                mappingRule.Source.Name
                    }]
                 | _ -> [{Code = sprintf "return default(%s);" (getFullyQualifiedNameIncludingGenerics sourceAsClassDef)}]
        }

    let mainMappingMethodBody =
        let model =
            ["mainMappingMethodSourceParameterName", "sourceSelector"
             "sourceTypeName", getFullyQualifiedNameIncludingGenerics sourceAsClassDef
             "destTypeName", getFullyQualifiedNameIncludingGenerics destinationAsClassDef
             "propertyMappingInstructions",
                mapping.PropertiesMappingRules 
                |> Seq.map (fun r -> {Code = (sprintf "destination.%s = %s(source)" r.Destination.Name (r |> propertyMappingMethod).Signature.Name)})
                |> Seq.fold (fun state curr -> sprintf "%s%s%s" state System.Environment.NewLine curr.Code) ""
            ] |> dict
        let regex = System.Text.RegularExpressions.Regex @"(\<#=([^#]+)#\>)"
        System.IO.File.ReadAllLines "Templates/MainMappingMethodBodyTemplate.txt"
        |> Array.map (fun line ->
                          let modifiedLine = regex.Replace(line, fun (m: System.Text.RegularExpressions.Match) ->
                                                   let key = m.Groups.[2].Captures.[0].Value
                                                   if not (model.ContainsKey key) then
                                                    failwith (sprintf "Key %s not found (used in template MainMappingMethodBodyTemplate.txt))" key)
                                                   sprintf "%s" model.[key])
                          { Code = modifiedLine })

    let mainMappingMethod: MethodDefinition =
        {
          AccessModifier = AccessModifier.Public
          OtherModifiers = [Modifier.Virtual]
          ReturnType = Some (NonVoid destinationAsClassDef)
          Signature = 
              {
                Name = Conventions.mainMappingMethodName
                GenericArguments = Seq.empty
                Parameters =
                  [
                    { Name = Conventions.mainMappingMethodSourceParameterName
                      ParameterType = (systemFuncDef [destinationAsClassDef; sourceAsClassDef]) }
                  ]
              }
          Body = mainMappingMethodBody
        }

    let mapperFetcher = systemFuncDef [Conventions.globalMapperInterfaceDefinition]
    { IsConcreteType = false
      AccessModifier = AccessModifier.Public
      OtherModifiers = [Modifier.Partial]
      IsInterface = false
      Namespace = None
      Name = name
      BaseClass = Some baseClass
      Methods = (mapping.PropertiesMappingRules |> Seq.map propertyMappingMethod) |> Seq.append [mainMappingMethod; methodForCreateDestination]
      InstanceVariables = Seq.empty<InstanceVariable>
      GenericArguments = Seq.empty<ClassDefinition> }
    |> withInjectedDependency mapperFetcher "mapperFetcher"

let buildClassFiles (mappingSpecifications: MappingSpecification seq): ClassFile seq =
    let doTheyHaveAMappingSpecification (x, y) =
        mappingSpecifications |> Seq.exists (fun z -> z.Source = x && z.Destination = y)

    let mappingClasses, baseMappingClasses, _ =
        mappingSpecifications
        |> Seq.fold (fun state item ->
                        let x, y, i = state
                        let mappingClass =
                            MappingOperations.createMapping item.Source item.Destination
                            |> createMappingClass (sprintf "Mapper%d" i) doTheyHaveAMappingSpecification
                        let baseMappingClasses =
                            match mappingClass.BaseClass with
                            | Some baseClass -> [baseClass]
                            | None -> []
                        Seq.append x [mappingClass], Seq.append y baseMappingClasses, i+1) (Seq.empty, Seq.empty, 0)

    let fromClassDefinitionToClassFile (classDef:ClassDefinition) =
        { Name = classDef.Name; Classes = [classDef] }

    let classFilesWithOneClassEach = 
        [
            createGlobalMapperInterfaceDefinition mappingSpecifications
            createGlobalMapperClassDefinition mappingSpecifications
        ]
        |> Seq.append mappingClasses
        |> Seq.map fromClassDefinitionToClassFile
    
    let classFilesWithTenClassesEach = 
        baseMappingClasses
        |> Seq.chunkBySize 10
        |> Seq.map (fun chunk -> 
                        chunk 
                        |> Seq.map fromClassDefinitionToClassFile)
        |> Seq.collect id

    let classFileWithTheIndividualMapperInterfaceDef =
        let classDef = Conventions.individualMapperInterfaceDefinitionWithGenericTypes
        { Name = "IndividualMapper"; Classes = [classDef] }

    classFilesWithOneClassEach
    |> Seq.append classFilesWithTenClassesEach
    |> Seq.append [classFileWithTheIndividualMapperInterfaceDef] 