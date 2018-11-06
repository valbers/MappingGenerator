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

    let buildClassFiles (mappingSpecifications: MappingSpecification seq): ClassDefinition seq =
        let mappingClasses, _ =
            mappingSpecifications
            |> Seq.fold (fun state item ->
                            let x, i = state
                            let currTargetItem =
                                Mapping.Operations.createMapping item.Source item.Destination
                                |> createMappingClass (sprintf "Mapper%d" i)
                            let toBeAppended =
                                match currTargetItem.BaseClass with
                                | Some baseClass -> Seq.ofList [currTargetItem; baseClass]
                                | None -> Seq.ofList [currTargetItem]
                            Seq.append x toBeAppended, i+1) (Seq.empty, 0)
        [
            createGlobalMapperInterfaceDefinition mappingSpecifications
            createGlobalMapperClassDefinition mappingSpecifications
        ]
        |> Seq.append mappingClasses