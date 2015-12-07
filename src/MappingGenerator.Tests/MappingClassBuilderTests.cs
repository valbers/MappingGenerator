using MappingGenerator.LangObjects;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MappingGenerator.Tests
{
    public class MappingClassBuilder_CreateMainMappingMethod_Tests
    {
        private Mock<IConversionInstructionGeneratorFactory> _conversionInstructionGeneratorFactory;
        private Mock<IInstructionGenerator> _instructionGenerator;
        private IFixture _fixture;
        private MappingClassBuilder _mappingClassBuilder;
        private Mapping _mapping;
        private List<MethodDefinition> _propertyMappingMethods;

        public MappingClassBuilder_CreateMainMappingMethod_Tests()
        {
            _fixture = new Fixture().Customize(new MultipleCustomization())
                                    .Customize(new AutoMoqCustomization());

            _conversionInstructionGeneratorFactory = _fixture.Freeze<Mock<IConversionInstructionGeneratorFactory>>();
            _instructionGenerator = _fixture.Freeze<Mock<IInstructionGenerator>>();

            _mappingClassBuilder = _fixture.Create<MappingClassBuilder>();

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

            _propertyMappingMethods = new List<MethodDefinition>
            {
                new MethodDefinition
                {
                    ReturnType = typeof(int),
                    Signature = new MethodSignature
                    {
                        Name = "MapMyProperty",
                        Parameters = new []
                        {
                            new MethodParameter
                            {
                                Name = "source",
                                ParameterType = typeof(Foo)
                            }
                        }
                    }
                }
            };
        }

        [Fact]
        public void destination_variable_is_set_with_return_value_of_create_destination_method()
        {
            var mainMappingMethod = Act();

            //Assert.Contains("destination = CreateDestination(source);", mainMappingMethod.Body.Select(x => x.Code));
        }

        private MethodDefinition Act()
        {
            return _mappingClassBuilder.CreateMainMappingMethod(_mapping, _propertyMappingMethods);
        }
    }
}
