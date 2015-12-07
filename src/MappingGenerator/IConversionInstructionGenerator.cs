using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MappingGenerator.LangObjects;

namespace MappingGenerator
{
    public interface IConversionInstructionGenerator
    {
        IEnumerable<Instruction> Generate(string sourceValue, Type source, Type destination);
        bool CanConvert(Type source, Type destination);
    }
}
