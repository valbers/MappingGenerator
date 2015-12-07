using MappingGenerator.LangObjects;
using MappingGenerator.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public class MappingClassBuilder : IMappingClassBuilder
    {
        private IConversionInstructionGeneratorFactory _conversionInstructionGeneratorFactory;
        private IInstructionGenerator _instructionGenerator;

        public MappingClassBuilder(IConversionInstructionGeneratorFactory conversionInstructionGeneratorFactory,
                                   IInstructionGenerator instructionGenerator)
        {
            _conversionInstructionGeneratorFactory = conversionInstructionGeneratorFactory;
            _instructionGenerator = instructionGenerator;
        }

        public MethodDefinition CreateMainMappingMethod(Mapping mapping, List<MethodDefinition> propertyMappingMethods)
        {
            var mappingMethod = new MethodDefinition
            {
                AccessModifier = AccessModifier.Public,
                OtherModifiers = new[] { Modifier.Virtual },
                ReturnType = mapping.Destination
            };

            var sourceSelectorFuncDefinition = typeof(Func<,>).MakeGenericType(mapping.Destination, mapping.Source);

            var sourceParameterName = Conventions.MainMappingMethodSourceParameterName();
            mappingMethod.Signature = new MethodSignature
            {
                Name = Conventions.MainMappingMethodName(),
                Parameters = new[]
                {
                    new MethodParameter
                    {
                        ParameterType = sourceSelectorFuncDefinition,
                        Name = sourceParameterName
                    }
                }
            };

            //var methodForCreateDestination = Conventions.MethodForCreateDestination(mapping.Source, mapping.Destination);
            var destinationVariableName = Conventions.MainMappingMethodDestinationVariableName();
            //var defaultOfSource = string.Format("default({0})", Utils.BuildTypeNameOfAVariable(mapping.Source));
            //var defaultOfDestination = string.Format("default({0})", Utils.BuildTypeNameOfAVariable(mapping.Destination));
            //var instructions = new List<Instruction>
            //{
            //    _instructionGenerator.GenerateIfBlock(
            //    new BooleanExpression
            //    {
            //        LeftOperand = sourceParameterName, Operator = "==", RightOperand = defaultOfSource
            //    }, _instructionGenerator.ReturnValue(defaultOfDestination)),

            //    _instructionGenerator.DeclareVariable(destinationVariableName, mapping.Destination),
            //    _instructionGenerator.SetVariableWithReturnValueFromMethod(destinationVariableName, methodForCreateDestination, sourceParameterName)
            //};
            //foreach (var mappingRule in mapping.PropertiesMappingRules)
            //{
            //    var method = propertyMappingMethods.First(x => x.ReturnType == mappingRule.Destination.Type &&
            //        x.Signature.Name ==
            //        Conventions.MapToPropertyMethodName(mappingRule.Source.Name, mappingRule.Destination.Name));
            //    instructions.Add(_instructionGenerator.SetVariableWithReturnValueFromMethod(string.Concat(destinationVariableName, ".", mappingRule.Destination.Name), method, sourceParameterName));
            //}

            //instructions.Add(_instructionGenerator.ReturnValue(destinationVariableName));
            var mainMappingMethodTemplate = new MainMappingMethodTemplate();
            mainMappingMethodTemplate.Session = new Dictionary<string, object>();
            mainMappingMethodTemplate.Session["Model"] = new MainMappingMethodModel
            {
                Mapping = mapping,
                DestinationVariableName = destinationVariableName,
                MethodDefinition = mappingMethod,
                PropertyMappingMethods = propertyMappingMethods
            };
            mainMappingMethodTemplate.Initialize();
            var methodBody = mainMappingMethodTemplate.TransformText();
            mappingMethod.Body = methodBody.Split(new[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries).Select(x => new Instruction { Code = x });
            //mappingMethod.Body = new [] {new Instruction { Code = mainMappingMethodTemplate.TransformText() }};
            //mappingMethod.Body = instructions;

            return mappingMethod;
        }

        public MethodDefinition CreatePropertyMappingMethod(MappingRule mappingRule, Mapping mapping)
        {
            var mappingMethod = new MethodDefinition
            {
                AccessModifier = AccessModifier.Public,
                OtherModifiers = new[] { Modifier.Virtual },
                ReturnType = mappingRule.Destination.Type
            };
            var sourceParameterName = Conventions.MapToPropertySourceParameterName();
            mappingMethod.Signature = new MethodSignature
            {
                Name = Conventions.MapToPropertyMethodName(mappingRule.Source.Name, mappingRule.Destination.Name),
                Parameters = new[]
                {
                    new MethodParameter
                    {
                        ParameterType = mapping.Source,
                        Name = sourceParameterName
                    }
                }
            };

            IConversionInstructionGenerator conversionInstructionGenerator = _conversionInstructionGeneratorFactory.Create(mappingRule.Source.Type, mappingRule.Destination.Type);
            mappingMethod.Body = conversionInstructionGenerator.Generate(string.Concat(sourceParameterName, ".", mappingRule.Source.Name), mappingRule.Source.Type, mappingRule.Destination.Type);

            return mappingMethod;
        }
    }
}
