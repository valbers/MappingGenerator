namespace CodeGeneration

open CodeGeneration.Records
open Mapping.Records

module Operations =

    let createMappingClass (name: string) (mapping: Mapping) : ClassDefinition =
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

        { AccessModifier = AccessModifier.Public
          OtherModifiers = [ Modifier.Partial ]
          IsInterface = false
          Namespace = None
          Name = name
          InstanceVariables = Seq.empty<InstanceVariable>
          Methods = Seq.empty<MethodDefinition>
          GenericArguments = Seq.empty<ClassDefinition>
          BaseClass = Some baseClass
          IsConcreteType = false }

    let instructToSetAVariable variableName value = { Code = sprintf "%s = %s;" variableName value }

    let withInjectedDependency (dependency: ClassDefinition) (dependencyName: string) (targetClass: ClassDefinition) =
        let dependencyFieldName = Conventions.Operations.fieldName dependencyName
        let instanceVariables =
            targetClass.InstanceVariables
            |> Seq.append (Seq.ofList [{ InstanceVariableType = InstanceVariableType.Field 
                                         AccessModifier = AccessModifier.Private
                                         OtherModifiers = Seq.empty
                                         Type = dependency
                                         Name = dependencyFieldName }])

        let addSetDependencyInstruction originalBody =
            originalBody
            |> Seq.append (Seq.ofList [instructToSetAVariable dependencyFieldName dependencyName])

        let methods =
            targetClass.Methods
            |> Seq.map (fun x -> match x.ReturnType with
                                    | None -> x
                                    | Some _ -> { x with Body = addSetDependencyInstruction x.Body })

        { targetClass with
            InstanceVariables = instanceVariables
            Methods = methods }

    let newConstructorWithDependency (dependency: ClassDefinition*string) (targetClass: ClassDefinition): MethodDefinition =
        let dependencyType, dependencyName = dependency
        { ReturnType = None
          AccessModifier = AccessModifier.Public
          OtherModifiers = Seq.empty
          Signature = { Name = targetClass.Name
                        GenericArguments = Seq.empty
                        Parameters = Seq.ofList [{Name = dependencyName; ParameterType = dependencyType}] }
          Body = Seq.ofList [ instructToSetAVariable (Conventions.Operations.fieldName dependencyName) dependencyName ]
        }

    let convertTypeToClassDef (theType: System.Type): ClassDefinition =
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

    let createGlobalMapperInterfaceDefinition (mappingSpecifications: MappingSpecification seq) =
        let globalMapperInterface = Conventions.Operations.globalMapperInterfaceDefinition
        let res, _ =
            mappingSpecifications
            |> Seq.fold (fun state input ->
                            let curr, index = state
                            let dependencyName = sprintf "m%d" index
                            let individualMapperInterfaceDef =
                                Conventions.Operations.individualMapperInterfaceDefinition (convertTypeToClassDef input.Source) (convertTypeToClassDef input.Destination)
                            (withInjectedDependency individualMapperInterfaceDef dependencyName curr), index + 1) (globalMapperInterface, 0)
        res 

    let createGlobalMapperClassDefinition (mappingSpecifications: MappingSpecification seq) =
        let globalMapperInterface = createGlobalMapperInterfaceDefinition mappingSpecifications
        { globalMapperInterface with
            IsInterface = false
            Name = globalMapperInterface.Name.TrimStart('I') }

    let buildClassFiles (mappingSpecifications: MappingSpecification seq): ClassFile seq =
        let mappingClasses, baseMappingClasses, _ =
            mappingSpecifications
            |> Seq.fold (fun state item ->
                            let x, y, i = state
                            let mappingClass =
                                Mapping.Operations.createMapping item.Source item.Destination
                                |> createMappingClass (sprintf "Mapper%d" i)
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
            let classDef = Conventions.Operations.individualMapperInterfaceDefinitionWithGenericTypes
            { Name = "IndividualMapper"; Classes = [classDef] }

        classFilesWithOneClassEach
        |> Seq.append classFilesWithTenClassesEach
        |> Seq.append [classFileWithTheIndividualMapperInterfaceDef] 