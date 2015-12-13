using MappingGenerator.LangObjects;
using System.Collections.Generic;
using System.Linq;

namespace MappingGenerator
{
    public class MappingGenerationManager
    {
        private IMappingClassCreator _mappingClassCreator;
        private IMappingCreator _mappingCreator;

        public MappingGenerationManager(IMappingCreator mappingCreator, IMappingClassCreator mappingClassCreator)
        {
            _mappingClassCreator = mappingClassCreator;
            _mappingCreator = mappingCreator;
        }

        public IEnumerable<ClassFile> BuildClassFiles(IMappingConfiguration mappingConfiguration, int maxBaseClassDefinitionsPerFile = 10)
        {
            var classDefinitionFiles = new ClassFiles();
            var baseClassDefinitionFiles = new ClassFiles(maxBaseClassDefinitionsPerFile);

            var i = 1;
            foreach (var mapping in mappingConfiguration.AllMappings().Select(x => _mappingCreator.CreateMapping(x.Source, x.Destination)))
            {
                var mappingClass = _mappingClassCreator.CreateMappingClass(mapping, string.Format("Mapper{0}", (i++).ToString()));
                classDefinitionFiles.Add(mappingClass);
                baseClassDefinitionFiles.Add(mappingClass.BaseClass);
            }

            classDefinitionFiles.Add(_mappingClassCreator.CreateGlobalMapperInterfaceDefinition(mappingConfiguration));
            classDefinitionFiles.Add(_mappingClassCreator.CreateGlobalMapperClassDefinition(mappingConfiguration));
            classDefinitionFiles.Add(
                new ClassFile()
                {
                    Name = "IndividualMapper",
                    Classes = new List<ClassDefinition> { Conventions.IndividualMapperInterfaceDefinition() }
                });

            var classFiles = classDefinitionFiles.Union(baseClassDefinitionFiles);
            return classFiles;
        }
    }
}
