namespace CodeGeneration.Mapping

open Mapping.Records
open Records

module CodeGenerationOperations =

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

        