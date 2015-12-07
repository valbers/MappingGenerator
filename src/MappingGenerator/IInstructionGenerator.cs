using MappingGenerator.LangObjects;
using System;

namespace MappingGenerator
{
    public interface IInstructionGenerator
    {
        Instruction ReturnProperty(string variable, string property);

        Instruction ReturnValue(string value);

        Instruction YieldReturn(string value);

        Instruction DeclareVariable(string variable, ClassDefinition type);

        Instruction SetVariable(string variable, string value);

        Instruction SetVariableWithReturnValueFromMethod(string variable, MethodDefinition methodDefinition, params string[] methodArgs);

        Instruction CallMethod(string variable, string methodName, params string[] arguments);

        IfBlockInstruction GenerateIfBlock(BooleanExpression booleanExpression, Instruction trueCaseInstruction, Instruction elseCaseInstruction = null);

        ForEachBlock GenerateForEachBlock(string iEnumerableVariable, Instruction blockBody);
    }
}
