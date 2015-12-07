using System;
namespace MappingGenerator
{
    public interface IMappingCreator
    {
        Mapping CreateMapping(Type source, Type dest);
    }
}
