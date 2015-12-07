using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public class MappingBasedConversionInstructionGenerator : IConversionInstructionGenerator
    {
        IMappingConfiguration _mappingConfiguration;
        IInstructionGenerator _instructionGenerator;
        public MappingBasedConversionInstructionGenerator(IMappingConfiguration mappingConfiguration, IInstructionGenerator instructionGenerator)
        {
            _mappingConfiguration = mappingConfiguration;
            _instructionGenerator = instructionGenerator;
        }

        public bool CanConvert(Type source, Type destination)
        {
            return _mappingConfiguration.IsMappingConfigured(source, destination);
        }

        public IEnumerable<Instruction> Generate(string sourceValue, Type source, Type destination)
        {
            var mapper = Conventions.FieldNameForGlobalMapper();
            var mapAtoBMethod = Conventions.MethodNameForMapPropertyAtoPropertyB(source, destination);
            return new[]
            {
                new Instruction 
                { 
                    Code =
                    string.Format(
                    "return {0}().{1}(({2} x) => {3});",
                    mapper, mapAtoBMethod, Utils.BuildTypeNameOfAVariable(destination), sourceValue)
                }
            };
        }
    }
}
