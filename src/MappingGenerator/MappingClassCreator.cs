using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public class MappingClassCreator
    {
        private readonly IInstructionGenerator _instructionGenerator;
        private readonly IConversionInstructionGeneratorFactory _conversionInstructionGeneratorFactory;
        private readonly IClassModifier _classModifier;
        private readonly IMappingClassBuilder _mappingClassBuilder;

        public MappingClassCreator(IInstructionGenerator instructionGenerator,
                                   IConversionInstructionGeneratorFactory conversionInstructionGeneratorFactory,
                                   IClassModifier classModifier,
                                   IMappingClassBuilder mappingClassBuilder)
        {
            _instructionGenerator = instructionGenerator;
            _conversionInstructionGeneratorFactory = conversionInstructionGeneratorFactory;
            _classModifier = classModifier;
            _mappingClassBuilder = mappingClassBuilder;
        }

        public ClassDefinition CreateMappingClass(Mapping mapping, string name)
        {
            var baseClass = new ClassDefinition
            {
                AccessModifier = AccessModifier.Public,
                OtherModifiers = new[] { Modifier.Abstract, Modifier.Partial },
                Name = string.Concat(name, "Base"),
                BaseClass = Conventions.IndividualMapperInterfaceDefinition(mapping.Source, mapping.Destination)
            };

            var globalMapperInterfaceDefinition = Conventions.GlobalMapperInterfaceDefinition();
            var mapperFetcher = new ClassDefinition
            {
                Namespace = "System",
                Name = "Func",
                GenericArguments = new[]
                {
                    globalMapperInterfaceDefinition
                }
            };
            _classModifier.AddDependency(mapperFetcher, Conventions.ConstructorParameterName(globalMapperInterfaceDefinition), baseClass);

            var methods = new List<MethodDefinition>(baseClass.Methods);
            methods.AddRange(mapping.PropertiesMappingRules.Select(x => _mappingClassBuilder.CreatePropertyMappingMethod(x, mapping)));
            methods.Add(CreateMethodForCreateDestination(mapping));
            methods.Add(_mappingClassBuilder.CreateMainMappingMethod(mapping, methods));
            baseClass.Methods = methods;

            var mappingClass = new ClassDefinition
            {
                AccessModifier = AccessModifier.Public,
                OtherModifiers = new[] { Modifier.Partial },
                Name = name,
                BaseClass = baseClass
            };
            _classModifier.ReimplementBaseClassConstructors(mappingClass, baseClass);

            return mappingClass;
        }

        private MethodDefinition CreateMethodForCreateDestination(Mapping mapping)
        {
            var code = string.Format("return new {0}();", Utils.BuildTypeNameOfAVariable(mapping.Destination));
            if (mapping.Destination.GetConstructors().All(x => x.GetParameters() != null && x.GetParameters().Any()))
                code = "throw new System.NotImplementedException();";

            var method = Conventions.MethodForCreateDestination(mapping.Source, mapping.Destination);
            method.Body = new[] { new Instruction { Code = code } };

            return method;
        }

        public ClassDefinition CreateGlobalMapperInterfaceDefinition(MappingConfiguration mappingConfiguration)
        {
            var globalMapperInterface = Conventions.GlobalMapperInterfaceDefinition();

            var i = 0;
            var methods = new List<MethodDefinition>();
            foreach (var mapping in mappingConfiguration.AllMappings())
            {
                var dependencyName = string.Concat("m", i++);
                methods.Add(CreateGlobalMapperMethodDefinition(mapping.Source, mapping.Destination, Conventions.FieldName(dependencyName)));
                var dependency = Conventions.IndividualMapperInterfaceDefinition(mapping.Source, mapping.Destination);
                _classModifier.AddDependency(dependency, dependencyName, globalMapperInterface);
            }
            globalMapperInterface.Methods = globalMapperInterface.Methods.Union(methods);
            return globalMapperInterface;
        }

        public MethodDefinition CreateGlobalMapperMethodDefinition(ClassDefinition source, ClassDefinition destination, string individualMapperFieldName) 
        {
            var mappingMethod = new MethodDefinition
            {
                AccessModifier = AccessModifier.Public,
                OtherModifiers = new[] { Modifier.Virtual },
                ReturnType = destination
            };

            var sourceParameterName = Conventions.MainMappingMethodSourceParameterName();
            mappingMethod.Signature = new MethodSignature
            {
                Name = Conventions.MainMappingMethodName(),
                Parameters = new[]
                {
                    new MethodParameter
                    {
                        ParameterType = new ClassDefinition
                        {
                            Namespace = "System",
                            Name = "Func",
                            GenericArguments = new []
                            {
                                destination,
                                source
                            }
                        },
                        Name = sourceParameterName
                    }
                }
            };

            mappingMethod.Body = new[]
            {
                new Instruction
                {
                    Code = string.Format("return {0}.{1}({2});",
                    individualMapperFieldName,
                    Conventions.MainMappingMethodName(),
                    Conventions.MainMappingMethodSourceParameterName())
                }
            };

            return mappingMethod;

            //var aToBMapperBaseMapper = new ClassDefinition
            //{
            //    Name = Conventions.AtoBMapperBaseClassName(source, destination)
            //};
            //return new MethodDefinition
            //{
            //    AccessModifier = AccessModifier.Public,
            //    ReturnType = destination,
            //    Signature = new MethodSignature
            //    {
            //        Name = string.Format(Conventions.MethodNameForMapPropertyAtoPropertyB(source, destination)),
            //        Parameters = new[]
            //        {
            //            new MethodParameter
            //            {
            //                Name = Conventions.MapToPropertySourceParameterName(),
            //                ParameterType = source
            //            }
            //        }
            //    },
            //    Body = new[]
            //    {
            //        _instructionGenerator.ReturnValue(
            //        _instructionGenerator.CallMethod(Conventions.FieldName(aToBMapperBaseMapper),
            //                                         Conventions.MainMappingMethodName(),
            //                                         Conventions.MapToPropertySourceParameterName()).Code) }
            //};
        }
    }
}
