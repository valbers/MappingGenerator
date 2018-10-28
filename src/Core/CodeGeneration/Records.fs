namespace CodeGeneration.Mapping

module Records =

    type Instruction =
        { Code: string }

    type AccessModifier = 
        | Public
        | Private
        | Internal
        | Protected

    type Modifier =
        | Abstract
        | Async
        | Const
        | Event
        | Extern
        | New
        | Override
        | Partial
        | Readonly
        | Sealed
        | Static
        | Unsafe
        | Virtual
        | Volatile

    type InstanceVariableType =
        | Property
        | Field

    type ClassDefinition =
        { IsInterface: bool
          Namespace: string
          AccessModifier: AccessModifier
          OtherModifiers: Modifier seq
          Name: string 
          BaseClass: ClassDefinition
          InstanceVariables: InstanceVariable seq
          Methods: MethodDefinition seq
          GenericArguments: ClassDefinition seq
          IsConcreteType: bool }
    and InstanceVariable =
        { Name: string
          InstanceVariableType: InstanceVariableType
          Type: ClassDefinition
          AccessModifier: AccessModifier
          OtherModifiers: Modifier seq }
    and MethodDefinition =
        { AccessModifier: AccessModifier
          OtherModifiers: Modifier seq
          Signature: MethodSignature
          ReturnType: ClassDefinition
          Body: Instruction seq}
    and MethodSignature = 
        { Name: string
          Parameters: MethodParameter seq
          GenericArguments: ClassDefinition seq }
    and MethodParameter =
        { ParameterType: ClassDefinition
          Name: string }