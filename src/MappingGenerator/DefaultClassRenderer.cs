using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public class DefaultClassRenderer
    {
        public void RenderClass(ClassDefinition classDefinition, TextWriter textWriter)
        {
            var result = new StringBuilder();
            int indentationLevel = 0;
            bool withNamespace;
            if(withNamespace = !string.IsNullOrWhiteSpace(classDefinition.Namespace))
            {
                result.AppendFormat("namespace {0}", classDefinition.Namespace);
                OpenCodeBlock(result, ref indentationLevel);
            }

            result.Append(Indent(indentationLevel));
            result.Append(KeywordToString(classDefinition.AccessModifier));
            result.Append(" ");
            AppendOtherModifiers(result, classDefinition.OtherModifiers);
            result.Append(classDefinition.IsInterface ? "interface" : "class");
            result.Append(" ");
            result.Append(Utils.BuildTypeNameOfClassDefinition(classDefinition));

            if (classDefinition.BaseClass != null)
                result.AppendFormat(" : {1}", Indent(indentationLevel), Utils.BuildTypeNameOfAVariable(classDefinition.BaseClass));

            OpenCodeBlock(result, ref indentationLevel);

            AddInstanceVariables(result, classDefinition.InstanceVariables, ref indentationLevel, classDefinition.IsInterface);
            AddMethods(result, classDefinition, ref indentationLevel);

            CloseCodeBlock(result, ref indentationLevel);
            if(withNamespace) CloseCodeBlock(result, ref indentationLevel);

            textWriter.Write(result);
        }

        private void AddInstanceVariables(StringBuilder result, IEnumerable<InstanceVariable> instanceVariables, ref int indentationLevel, bool isInterface = false)
        {
            foreach (var instanceVariable in instanceVariables)
            {
                result.Append(Indent(indentationLevel));
                if (!isInterface)
                {
                    result.Append(KeywordToString(instanceVariable.AccessModifier));
                    result.Append(" ");
                }

                AppendOtherModifiers(result, instanceVariable.OtherModifiers);

                result.AppendFormat("{0} {1}{2}",
                                    Utils.BuildTypeNameOfAVariable(instanceVariable.Type),
                                    instanceVariable.Name,
                                    instanceVariable.InstanceVariableType == InstanceVariableType.Field ? ";" : " { get; set; }");
                AppendSingleLine(result, indentationLevel);
            }
        }

        private void AddMethods(StringBuilder result, ClassDefinition classDefinition, ref int indentationLevel)
        {
            foreach (var method in classDefinition.Methods)
            {
                result.Append(Indent(indentationLevel));
                if (!classDefinition.IsInterface)
                {
                    result.Append(KeywordToString(method.AccessModifier));
                    result.Append(" ");
                }

                AppendOtherModifiers(result, method.OtherModifiers);

                if (method.ReturnType != null)
                {
                    result.AppendFormat(Utils.BuildTypeNameOfAVariable(method.ReturnType));
                    result.Append(" ");
                }
                
                result.AppendFormat("{0}({1})",
                                    method.Signature.Name,
                                    string.Join(", ", method.Signature.Parameters.Select(x => string.Concat(Utils.BuildTypeNameOfAVariable(x.ParameterType), " ", x.Name))));

                if (!classDefinition.IsInterface)
                {
                    if (method.IsBaseConstructorReimplementation)
                    {
                        result.AppendFormat(" : base ({0})", string.Join(", ", method.Signature.Parameters.Select(x => x.Name)));
                    }

                    OpenCodeBlock(result, ref indentationLevel);
                    foreach (var instruction in method.Body)
                    {
                        AppendSingleLine(result, indentationLevel, instruction);
                    }
                    CloseCodeBlock(result, ref indentationLevel);
                }
                else
                {
                    result.Append(";");
                }

                AppendSingleLine(result);
            }
        }

        private static string Indent(int indentationLevel)
        {
            var indentResult = new StringBuilder();
            for (var i = 0; i < indentationLevel; i++)
                indentResult.Append("    ");

            return indentResult.ToString();
        }

        private static void OpenCodeBlock(StringBuilder result, ref int indentationLevel)
        {
            AppendSingleLine(result);
            AppendSingleLine(result, indentationLevel++, "{");
        }

        private static void CloseCodeBlock(StringBuilder result, ref int indentationLevel)
        {
            AppendSingleLine(result);
            AppendSingleLine(result, --indentationLevel, "}");
        }

        private static void AppendOtherModifiers(StringBuilder result, IEnumerable<Modifier> otherModifiers)
        {
            var modifiers = string.Join(" ", otherModifiers.Select(x => KeywordToString(x)));
            if (!string.IsNullOrEmpty(modifiers))
            {
                result.Append(modifiers);
                result.Append(" ");
            }
        }

        private static string KeywordToString(object keyword)
        {
            return keyword.ToString().ToLowerInvariant();
        }

        private static void AppendSingleLine(StringBuilder stringBuilder, int indentationLevel = 0, string lineContent = null)
        {
            if (lineContent != null)
            {
                stringBuilder.Append(Indent(indentationLevel));
                stringBuilder.Append(lineContent);
            }

            if (stringBuilder[stringBuilder.Length - 1] == '\n')
                stringBuilder.Length--;
            if (stringBuilder[stringBuilder.Length - 1] == '\r')
                stringBuilder.Length--;

            stringBuilder.AppendLine();
        }

        private static void AppendSingleLine(StringBuilder stringBuilder, int indentationLevel, Instruction instruction)
        {
            if (instruction != null)
            {
                if (instruction.Code != null)
                {
                    stringBuilder.Append(Indent(indentationLevel));
                    stringBuilder.Append(instruction.Code);
                }
                else
                    AppendSingleLine(stringBuilder, indentationLevel, instruction as IfBlockInstruction);
            }

            if (stringBuilder[stringBuilder.Length - 1] == '\n')
                stringBuilder.Length--;
            if (stringBuilder[stringBuilder.Length - 1] == '\r')
                stringBuilder.Length--;

            stringBuilder.AppendLine();
        }

        private static void AppendSingleLine(StringBuilder stringBuilder, int indentationLevel, IfBlockInstruction instruction)
        {
            if (instruction != null)
            {
                stringBuilder.Append(Indent(indentationLevel));
                stringBuilder.AppendFormat("if ({0} {1} {2})",
                    instruction.BooleanExpression.LeftOperand,
                    instruction.BooleanExpression.Operator,
                    instruction.BooleanExpression.RightOperand);

                OpenCodeBlock(stringBuilder, ref indentationLevel);
                AppendSingleLine(stringBuilder, indentationLevel, instruction.TrueCaseInstruction);
                CloseCodeBlock(stringBuilder, ref indentationLevel);

                if(instruction.ElseCaseInstruction != null)
                {
                    OpenCodeBlock(stringBuilder, ref indentationLevel);
                    AppendSingleLine(stringBuilder, indentationLevel, instruction.ElseCaseInstruction);
                    CloseCodeBlock(stringBuilder, ref indentationLevel);
                }
            }

            if (stringBuilder[stringBuilder.Length - 1] == '\n')
                stringBuilder.Length--;
            if (stringBuilder[stringBuilder.Length - 1] == '\r')
                stringBuilder.Length--;

            stringBuilder.AppendLine();
        }
    }
}
