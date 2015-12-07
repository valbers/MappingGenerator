using MappingGenerator.Acceptance.TestDataObjects;
using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MappingGenerator.Acceptance
{
    public class Integration
    {
        [Fact]
        public void should_be_almighty()
        {
            var instructionGenerator = new InstructionGenerator();
            var mappingConfiguration = new MappingConfiguration();
            mappingConfiguration.Register(typeof(ComplexSource), typeof(ComplexDestination));
            mappingConfiguration.Register(typeof(IList<string>), typeof(List<string>));
            mappingConfiguration.Register(typeof(Foo), typeof(Bar));
            var conversionInstructionGeneratorFactory = new ConversionInstructionGeneratorFactory(new IConversionInstructionGenerator[]
            {
                new IdentityConversionInstructionGenerator(instructionGenerator),
                new MappingBasedConversionInstructionGenerator(mappingConfiguration, instructionGenerator),
                new NotImplementedConversionInstructionGenerator()
            });

            var mappingCreator = new MappingCreator();
            var mappingClassCreator = new MappingClassCreator(instructionGenerator, conversionInstructionGeneratorFactory, new ClassModifier(instructionGenerator), new MappingClassBuilder(conversionInstructionGeneratorFactory, instructionGenerator));
            var classRenderer = new DefaultClassRenderer();

            var renderedBaseClasses = new List<ClassDefinition>();
            var i = 1;
            foreach (var mapping in mappingConfiguration.AllMappings().Select(x => mappingCreator.CreateMapping(x.Source, x.Destination)))
            {
                var mappingClass = mappingClassCreator.CreateMappingClass(mapping, string.Format("Mapper{0}", (i++).ToString()));
                OutputFile(classRenderer, mappingClass);
                if (!renderedBaseClasses.Contains(mappingClass.BaseClass))
                {
                    OutputFile(classRenderer, mappingClass.BaseClass);
                    renderedBaseClasses.Add(mappingClass);
                }
            }

            var globalMapperInterfaceDefinition = mappingClassCreator.CreateGlobalMapperInterfaceDefinition(mappingConfiguration);
            OutputFile(classRenderer, globalMapperInterfaceDefinition);
            globalMapperInterfaceDefinition.IsInterface = false;
            globalMapperInterfaceDefinition.Name.TrimStart('I');
            OutputFile(classRenderer, globalMapperInterfaceDefinition);

            var individualMapperInterfaceDefinition = Conventions.IndividualMapperInterfaceDefinition();
            OutputFile(classRenderer, individualMapperInterfaceDefinition, "IndividualMapper");
        }

        private void OutputFile(DefaultClassRenderer defaultClassRenderer, ClassDefinition classDefinition, string fileName = null)
        {
            var testOutputFile = File.Create(string.Concat("..\\..\\TestOutput\\", fileName ?? classDefinition.Name, ".cs"));
            using (var streamWriter = new StreamWriter(testOutputFile))
            {
                defaultClassRenderer.RenderClass(classDefinition, streamWriter);
            }
        }
    }
}
