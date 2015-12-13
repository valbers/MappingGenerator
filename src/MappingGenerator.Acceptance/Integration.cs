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
        public void generating_mapping_classes_and_outputting_them_works()
        {
            var instructionGenerator = new InstructionGenerator();
            var mappingConfiguration = new MappingConfiguration();
            var conversionInstructionGeneratorFactory = new ConversionInstructionGeneratorFactory(new IConversionInstructionGenerator[]
            {
                new IdentityConversionInstructionGenerator(instructionGenerator),
                new MappingBasedConversionInstructionGenerator(mappingConfiguration, instructionGenerator),
                new DefaultValueConversionInstructionGenerator()
            });
            var mappingCreator = new MappingCreator();
            var mappingClassCreator = new MappingClassCreator(instructionGenerator, conversionInstructionGeneratorFactory, new ClassModifier(instructionGenerator), new MappingClassBuilder(conversionInstructionGeneratorFactory, instructionGenerator));
            var classRenderer = new DefaultClassRenderer();
            var mappingGeneratorManager = new MappingGenerationManager(mappingCreator, mappingClassCreator);

            mappingConfiguration.Register(typeof(ComplexSource), typeof(ComplexDestination));
            mappingConfiguration.Register(typeof(IList<string>), typeof(List<string>));
            mappingConfiguration.Register(typeof(Foo), typeof(Bar));
            mappingConfiguration.Register(typeof(ComplexSource), typeof(Bar));

            foreach (var classFile in mappingGeneratorManager.BuildClassFiles(mappingConfiguration))
                OutputFile(classRenderer, classFile);
        }

        private void OutputFile(DefaultClassRenderer defaultClassRenderer, ClassFile classFile)
        {
            var testOutputFile = File.Create(string.Concat("..\\..\\TestOutput\\", classFile.Name ?? classFile.Classes.First().Name, ".cs"));
            using (var streamWriter = new StreamWriter(testOutputFile))
            {
                foreach (var classDefinition in classFile.Classes)
                    defaultClassRenderer.RenderClass(classDefinition, streamWriter);
            }
        }
    }
}
