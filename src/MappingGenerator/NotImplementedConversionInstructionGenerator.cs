using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MappingGenerator.LangObjects;

namespace MappingGenerator
{
    public class DefaultValueConversionInstructionGenerator : IConversionInstructionGenerator
    {
        public IEnumerable<LangObjects.Instruction> Generate(string sourceValue, Type source, Type destination)
        {
            return new[] { new Instruction { Code = string.Format("return default({0});\r\n", Utils.BuildTypeNameOfAVariable(destination)) } };
        }

        public bool CanConvert(Type source, Type destination)
        {
            return true;
        }
    }
}
