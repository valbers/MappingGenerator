using MappingGenerator.LangObjects;
using System.Collections.Generic;

namespace MappingGenerator
{
    public class ClassFile
    {
        public string Name { get; set; }

        public List<ClassDefinition> Classes { get; set; }
    }
}
