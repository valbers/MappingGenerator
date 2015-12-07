using System;
using MappingGenerator.LangObjects;
using System.Collections.Generic;

namespace MappingGenerator
{
    public interface IClassModifier
    {
        MethodDefinition AddConstructor(ClassDefinition classDefinition, params ClassDefinition[] parameters);
        MethodDefinition AddConstructor(ClassDefinition classDefinition, IEnumerable<MethodParameter> parameters);
        void ReimplementBaseClassConstructors(ClassDefinition classDefinition, ClassDefinition baseClass);
        void AddDependency(ClassDefinition dependency, string name, ClassDefinition classDefinition);
    }
}
