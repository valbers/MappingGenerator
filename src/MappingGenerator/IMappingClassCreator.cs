using MappingGenerator.LangObjects;

namespace MappingGenerator
{
    public interface IMappingClassCreator
    {
        ClassDefinition CreateGlobalMapperInterfaceDefinition(IMappingConfiguration mappingConfiguration);
        ClassDefinition CreateGlobalMapperClassDefinition(IMappingConfiguration mappingConfiguration);
        MethodDefinition CreateGlobalMapperMethodDefinition(ClassDefinition source, ClassDefinition destination, string individualMapperFieldName);
        ClassDefinition CreateMappingClass(Mapping mapping, string name);
    }
}