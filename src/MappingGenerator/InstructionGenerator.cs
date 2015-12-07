using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public class InstructionGenerator : MappingGenerator.IInstructionGenerator
    {
        public Instruction ReturnProperty(string variable, string property)
        {
            return ReturnValue(string.Concat(variable, ".", property));
        }

        public Instruction ReturnValue(string value)
        {
            return new Instruction { Code = string.Format("return {0};", value.TrimEnd(';')) };
        }

        public Instruction YieldReturn(string value)
        {
            return new Instruction { Code = string.Format("yield return {0};", value) };
        }

        public Instruction DeclareVariable(string variable, ClassDefinition type)
        {
            return new Instruction { Code = string.Format("{0} {1};", Utils.BuildTypeNameOfAVariable(type), variable) };
        }

        public Instruction SetVariableWithReturnValueFromMethod(string variable, MethodDefinition methodDefinition, params string[] methodArgs)
        {
            return new Instruction { Code = string.Format("{0} = {1}({2});", variable, methodDefinition.Signature.Name, (methodArgs != null && methodArgs.Any()) ? string.Join(", ", methodArgs) : "") };
        }

        public Instruction SetVariable(string variable, string value)
        {
            return new Instruction { Code = string.Format("{0} = {1};", variable, value) };
        }

        public Instruction CallMethod(string variable, string methodName, params string[] arguments)
        {
            return new Instruction { Code = string.Format("{0}.{1}({2});", variable, methodName, string.Join(", ", arguments)) };
        }

        public IfBlockInstruction GenerateIfBlock(BooleanExpression booleanExpression, Instruction trueCaseInstruction, Instruction elseCaseInstruction = null)
        {
            return new IfBlockInstruction
            {
                BooleanExpression = booleanExpression,
                TrueCaseInstruction = trueCaseInstruction,
                ElseCaseInstruction = elseCaseInstruction 
            };
        }

        public ForEachBlock GenerateForEachBlock(string iEnumerableVariable, Instruction blockBody)
        {
            return new ForEachBlock
            {
                IEnumerableVariable = iEnumerableVariable,
                BlockBody = blockBody
            };
        }
    }
}
