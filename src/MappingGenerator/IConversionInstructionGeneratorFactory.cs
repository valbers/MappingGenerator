using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public interface IConversionInstructionGeneratorFactory
    {
        IConversionInstructionGenerator Create(Type source, Type destination);
    }

    public class ConversionInstructionGeneratorFactory : IConversionInstructionGeneratorFactory
    {
        IConversionInstructionGenerator[] _conversionInstructionGenerators;

        public ConversionInstructionGeneratorFactory(IConversionInstructionGenerator[] conversionInstructionGenerators)
        {
            _conversionInstructionGenerators = conversionInstructionGenerators;
        }

        public IConversionInstructionGenerator Create(Type source, Type destination)
        {
            return _conversionInstructionGenerators.First(x => x.CanConvert(source, destination));
        }
    }
}
