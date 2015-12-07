using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MappingGenerator.LangObjects;

namespace MappingGenerator
{
    public class IdentityConversionInstructionGenerator : IConversionInstructionGenerator
    {
        IInstructionGenerator _instructionGenerator;

        public IdentityConversionInstructionGenerator(IInstructionGenerator instructionGenerator)
        {
            _instructionGenerator = instructionGenerator;
        }

        public bool CanConvert(Type source, Type destination)
        {
            return source == destination;

            //if(IsIEnumerableButNoString(source) &&
            //   IsIEnumerableButNoString(destination) &&
            //   GetIEnumerableUnderlyingType(source) == GetIEnumerableUnderlyingType(destination))
            //    return true;
        }

        public IEnumerable<Instruction> Generate(string sourceValue, Type source, Type destination)
        {
            //if (IsIEnumerableButNoString(source) &&
            //   IsIEnumerableButNoString(destination) &&
            //   GetIEnumerableUnderlyingType(source) == GetIEnumerableUnderlyingType(destination))
            //    return InstructionsForIEnumerableMapping(sourceValue, source, destination);

            return new [] {_instructionGenerator.ReturnValue(sourceValue)};
        }

        //private IEnumerable<Instruction> InstructionsForIEnumerableMapping(string sourceValue, Type source, Type destination)
        //{
        //    var eachInstruction = _instructionGenerator.YieldReturn(Conventions.ItemNameInForEachBlock());
        //    var @foreach = _instructionGenerator.GenerateForEachBlock(sourceValue, eachInstruction);

        //    return new Instruction[] { @foreach };
        //}

        //private bool IsIEnumerableButNoString(Type type)
        //{
        //    if(type == typeof(string))
        //        return false;

        //    var interfaces = type.GetInterfaces();
        //    if (interfaces == null || !interfaces.Any())
        //        return false;

        //    return interfaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        //}

        //private Type GetIEnumerableUnderlyingType(Type type)
        //{
        //    var ienumerableDef = type.GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        //    return ienumerableDef.GetGenericArguments()[0];
        //}
    }
}
