module MappingRecords
    type MappingRuleParticipant = 
        { Type: System.Type
          Name: string }

    type MappingRule =
        { Source: MappingRuleParticipant
          Destination: MappingRuleParticipant }

    type Mapping =
        { Source: System.Type
          Destination: System.Type
          PropertiesMappingRules: MappingRule seq }

    type MappingSpecification =
        { Source: System.Type
          Destination: System.Type }