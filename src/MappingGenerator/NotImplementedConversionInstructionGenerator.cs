using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MappingGenerator.LangObjects;

namespace MappingGenerator
{
    public class NotImplementedConversionInstructionGenerator : IConversionInstructionGenerator
    {
        public IEnumerable<LangObjects.Instruction> Generate(string sourceValue, Type source, Type destination)
        {
            return new[] { new Instruction { Code = "throw new System.NotImplementedException();\r\n" } };
        }

        public bool CanConvert(Type source, Type destination)
        {
            return true;
        }
    }
}
