using System;
using System.Collections.Generic;
using MappingGenerator.LangObjects;

namespace MappingGenerator
{
    public interface IMappingClassBuilder
    {
        MethodDefinition CreatePropertyMappingMethod(MappingRule mappingRule, Mapping mapping);
        MethodDefinition CreateMainMappingMethod(Mapping mapping, List<MethodDefinition> methods);
    }
}
