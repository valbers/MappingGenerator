module Conventions

open CodeGenerationRecords

let constructorParameterName (dependency: ClassDefinition) =
    match dependency.IsInterface with
    | true -> dependency.Name.ToLowerInvariant().TrimStart('i')
    | false -> dependency.Name.ToLowerInvariant()

let mainMappingMethodSourceParameterName = "sourceSelector"

let fieldName (originalName:string) = sprintf "_%s" (originalName.TrimStart('_'))

let globalMapperInterfaceDefinition =
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

let individualMapperInterfaceDefinition (tSource: ClassDefinition) (tDestination: ClassDefinition): ClassDefinition =
    { Name = "IMapper"
      Namespace = Some "AutoGeneration"
      IsInterface = true
      GenericArguments = [ tSource; tDestination ]
      AccessModifier = AccessModifier.Public
      OtherModifiers = Seq.empty
      BaseClass = None
      InstanceVariables = Seq.empty
      IsConcreteType = false
      Methods = 
      [ { ReturnType = Some tDestination
          AccessModifier = AccessModifier.Public
          OtherModifiers = Seq.empty
          Body = Seq.empty
          Signature = { Name = "Map"
                        GenericArguments = [ tSource; tDestination ]
                        Parameters = [ { Name = mainMappingMethodSourceParameterName
                                         ParameterType = { Namespace = Some "System"
                                                           Name = "Func"
                                                           GenericArguments = [ tDestination; tSource ]
                                                           AccessModifier = AccessModifier.Public
                                                           OtherModifiers = Seq.empty
                                                           IsInterface = false
                                                           BaseClass = None
                                                           InstanceVariables = Seq.empty
                                                           Methods = Seq.empty
                                                           IsConcreteType = true }
                                    } ]
                      }
      } ]
    }
let individualMapperInterfaceDefinitionWithGenericTypes =
    let emptyClassDefinition : ClassDefinition =
        { Name = ""
          Namespace = None
          IsInterface = true
          GenericArguments = Seq.empty
          AccessModifier = AccessModifier.Public
          OtherModifiers = Seq.empty
          BaseClass = None
          InstanceVariables = Seq.empty
          IsConcreteType = false
          Methods = Seq.empty }
    let tSourceGenericParameter = { emptyClassDefinition with Name = "tSourceGenericParameter" }
    let tDestinationGenericParameter = { emptyClassDefinition with Name = "TDestination" }
    individualMapperInterfaceDefinition tSourceGenericParameter tDestinationGenericParameter