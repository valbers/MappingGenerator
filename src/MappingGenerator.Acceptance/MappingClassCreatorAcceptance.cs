using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MappingGenerator.Acceptance.TestDataObjects;

namespace MappingGenerator.Acceptance
{
    public class MappingClassCreatorAcceptance
    {
        private readonly MappingClassCreator _mappingClassCreator;
        private Mapping _mapping;

        public MappingClassCreatorAcceptance()
        {
            var mappingConfiguration = new MappingConfiguration(new List<MappingSpecification>());
            var instructionGenerator = new InstructionGenerator();
            var conversionInstructionGeneratorFactory = new ConversionInstructionGeneratorFactory(
                                                               new IConversionInstructionGenerator[]
                                                               {
                                                                   new IdentityConversionInstructionGenerator(instructionGenerator),
                                                                   new MappingBasedConversionInstructionGenerator(mappingConfiguration, instructionGenerator)
                                                               });
            _mappingClassCreator = new MappingClassCreator(instructionGenerator,
                                                           conversionInstructionGeneratorFactory,
                                                           new ClassModifier(instructionGenerator),
                                                           new MappingClassBuilder(conversionInstructionGeneratorFactory, instructionGenerator));

            _mapping = new Mapping()
            {
                Source = typeof(Foo),
                Destination = typeof(Bar),
                PropertiesMappingRules = new[]
                {
                    new MappingRule
                    {
                        Source = new MappingRuleParticipant
                        {
                            Name = "MyProperty",
                            Type = typeof(int)
                        },
                        Destination = new MappingRuleParticipant
                        {
                            Name = "MyProperty",
                            Type = typeof(int)
                        }
                    }
                }
            };
            mappingConfiguration.Register(_mapping.Source, _mapping.Destination);
        }

        private ClassDefinition ActAndGetBaseMappingClass()
        {
            return _mappingClassCreator.CreateMappingClass(_mapping, "MappingClass").BaseClass;
        }

        [Fact]
        public void mapping_class_is_a_subclass_of_a_mapping_class()
        {
            var mappingClass = _mappingClassCreator.CreateMappingClass(_mapping, "MappingClass");

            Assert.NotNull(mappingClass.BaseClass);
        }

        [Fact]
        public void mapping_class_has_correct_name()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            Assert.Equal("MappingClassBase", mappingClass.Name);
        }

        [Fact]
        public void mapping_class_has_a_main_method_with_correct_name()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            Assert.Contains(mappingClass.Methods, x => x.Signature.Name == "Map");
        }

        [Fact]
        public void mapping_class_has_a_main_method_containing_source_as_parameter()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            Assert.Contains(mappingClass.Methods, x => x.Signature.Name == "Map" && x.Signature.Parameters.Any(y => y.Name == "sourceSelector"));
            Assert.Contains(mappingClass.Methods, x => x.Signature.Name == "Map" && x.Signature.Parameters.Any(y => y.ParameterType == typeof(Func<Bar, Foo>)));
        }

        [Fact]
        public void main_method_of_mapping_class_contains_declaration_of_return_variable()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            var mainMethod = GetMainMethod(mappingClass.Methods);

            Assert.Contains(mainMethod.Body.Where(x => x.Code != null), x => x.Code.StartsWith("MappingGenerator.Acceptance.TestDataObjects.Bar destination;"));
        }

        [Fact]
        public void in_main_method_destination_variable_is_set_with_return_value_of_create_destination_method()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            var mainMethod = GetMainMethod(mappingClass.Methods);

            Assert.Contains("destination = CreateDestination(source);", mainMethod.Body.Select(x => x.Code));
        }

        [Fact]
        public void main_method_of_mapping_class_contains_calls_to_property_mapping_methods()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            var mainMethod = GetMainMethod(mappingClass.Methods);
            
            Assert.Contains(mainMethod.Body.Where(x => x.Code != null), x => x.Code.StartsWith("destination.MyProperty = MapMyProperty(source);"));
        }

        [Fact]
        public void mapping_class_has_mapping_method_specific_for_a_certain_mapped_property()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            Assert.Contains(mappingClass.Methods, x => x.Signature.Name == "MapMyProperty");
            var mapToMyProperty = mappingClass.Methods.First(x => x.Signature.Name == "MapMyProperty");
            Assert.Contains(mapToMyProperty.Signature.Parameters, x => x.Name == "source");
            var fooParameter = mapToMyProperty.Signature.Parameters.First(x => x.Name == "source");
            Assert.Equal(fooParameter.ParameterType, typeof(Foo));
        }

        [Fact]
        public void mapping_method_specific_for_a_certain_mapped_property_returns_this_properties_value()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            var mapToMyProperty = mappingClass.Methods.First(x => x.Signature.Name == "MapMyProperty");
            Assert.Contains(mapToMyProperty.Signature.Parameters, x => x.Name == "source");
            Assert.StartsWith("return source.MyProperty;", mapToMyProperty.Body.First().Code);
            Assert.Equal(typeof(int), mapToMyProperty.ReturnType);
        }

        private MethodDefinition GetMainMethod(IEnumerable<LangObjects.MethodDefinition> methodsDefinitions)
        {
            return methodsDefinitions.Last();
        }
    }

    public class MappingClassCreatorAcceptance_OfComplexTypes
    {
        private readonly MappingClassCreator _mappingClassCreator;
        private Mapping _mapping;

        public MappingClassCreatorAcceptance_OfComplexTypes()
        {
            var mappingConfiguration = new MappingConfiguration(new List<MappingSpecification>());
            var instructionGenerator = new InstructionGenerator();
            var conversionInstructionGeneratorFactory = new ConversionInstructionGeneratorFactory(
                                                               new IConversionInstructionGenerator[]
                                                               {
                                                                   new IdentityConversionInstructionGenerator(instructionGenerator),
                                                                   new MappingBasedConversionInstructionGenerator(mappingConfiguration, instructionGenerator),
                                                                   new NotImplementedConversionInstructionGenerator()
                                                               });
            _mappingClassCreator = new MappingClassCreator(instructionGenerator,
                                                           conversionInstructionGeneratorFactory,
                                                           new ClassModifier(instructionGenerator),
                                                           new MappingClassBuilder(conversionInstructionGeneratorFactory, instructionGenerator));

            _mapping = new Mapping()
            {
                Source = typeof(ComplexSource),
                Destination = typeof(ComplexDestination),
                PropertiesMappingRules = new[]
                {
                    new MappingRule
                    {
                        Source = new MappingRuleParticipant
                        {
                            Name = "Foo",
                            Type = typeof(Foo)
                        },
                        Destination = new MappingRuleParticipant
                        {
                            Name = "Foo",
                            Type = typeof(Bar)
                        }
                    },
                    new MappingRule
                    {
                        Source = new MappingRuleParticipant
                        {
                            Name = "ListOfString",
                            Type = typeof(IList<string>)
                        },
                        Destination = new MappingRuleParticipant
                        {
                            Name = "ListOfString",
                            Type = typeof(List<string>)
                        }
                    }
                }
            };
            var mapping2 = new Mapping
            {
                Source = typeof(Foo),
                Destination = typeof(Bar),
                PropertiesMappingRules = new[]
                {
                    new MappingRule
                    {
                        Source = new MappingRuleParticipant
                        {
                            Name = "MyProperty",
                            Type = typeof(int)
                        },
                        Destination = new MappingRuleParticipant
                        {
                            Name = "MyProperty",
                            Type = typeof(int)
                        }
                    }
                }
            };
            mappingConfiguration.Register(_mapping.Source, _mapping.Destination);
            mappingConfiguration.Register(mapping2.Source, mapping2.Destination);
        }

        private ClassDefinition ActAndGetBaseMappingClass()
        {
            return _mappingClassCreator.CreateMappingClass(_mapping, "MappingClass").BaseClass;
        }

        private MethodDefinition GetMainMethod(IEnumerable<LangObjects.MethodDefinition> methodsDefinitions)
        {
            return methodsDefinitions.Last();
        }

        [Fact]
        public void method_for_mapping_foo_has_correct_name()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            Assert.Contains(mappingClass.Methods, x => x.Signature.Name == "MapFoo");
        }

        [Fact]
        public void method_for_mapping_foo_has_correct_parameter_name()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            var mapFoo = GetMethodForMappingFoo(mappingClass);

            Assert.Contains(mapFoo.Signature.Parameters, x => x.Name == "source");
        }

        [Fact]
        public void method_for_mapping_foo_has_correct_parameter_type()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            var mapFoo = GetMethodForMappingFoo(mappingClass);

            Assert.Contains(mapFoo.Signature.Parameters, x => x.ParameterType == typeof(ComplexSource));
        }

        [Fact]
        public void method_for_mapping_foo_has_correct_return_type()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            var mapFoo = GetMethodForMappingFoo(mappingClass);

            Assert.Equal(typeof(Bar), mapFoo.ReturnType);
        }

        [Fact]
        public void method_for_mapping_foo_has_correct_return_instruction()
        {
            var mappingClass = ActAndGetBaseMappingClass();

            var mapFoo = GetMethodForMappingFoo(mappingClass);

            Assert.StartsWith("return _mapper().Map((MappingGenerator.Acceptance.TestDataObjects.Bar x) => source.Foo);", mapFoo.Body.First().Code);
        }

        private static MethodDefinition GetMethodForMappingFoo(ClassDefinition mappingClass)
        {
            return mappingClass.Methods.First(x => x.Signature.Name == "MapFoo");
        }
    }
}
