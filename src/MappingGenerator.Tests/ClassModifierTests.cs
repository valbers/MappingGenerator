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
    public class ClassModifier_AddConstructor_Tests
    {
        private Mock<IInstructionGenerator> _instructionGenerator;
        private IFixture _fixture;
        private ClassModifier _classModifier;

        public ClassModifier_AddConstructor_Tests()
        {
            _fixture = new Fixture().Customize(new MultipleCustomization())
                                    .Customize(new AutoMoqCustomization());

            _instructionGenerator = _fixture.Freeze<Mock<IInstructionGenerator>>();

            _classModifier = _fixture.Create<ClassModifier>();
        }

        [Fact]
        public void constructor_has_the_same_name_as_the_class()
        {
            var classDefinition = new ClassDefinition
            {
                Name = "MyClass",
                Namespace = "MyNamespace"
            };

            var classModifier = new ClassModifier(_instructionGenerator.Object);
            classModifier.AddConstructor(classDefinition);

            Assert.Equal("MyClass", classDefinition.Methods.First().Signature.Name);
        }

        [Fact]
        public void constructor_has_the_same_name_as_a_generic_class()
        {
            var classDefinition = new ClassDefinition
            {
                Name = "MyClass",
                Namespace = "MyNamespace",
                GenericArguments = new[]
                {
                    new ClassDefinition
                    {
                        Name = "String",
                        Namespace = "System"
                    }
                }
            };

            _classModifier.AddConstructor(classDefinition);

            Assert.Equal("MyClass", classDefinition.Methods.First().Signature.Name);
        }

        [Fact]
        public void constructor_parameters_are_added()
        {
            var classDefinition = new ClassDefinition
            {
                Name = "MyClass",
                Namespace = "MyNamespace"
            };

            var myParameterType = new ClassDefinition { Name = "MyParamType", Namespace = "MyNamespace" };
            var myParameterType2 = new ClassDefinition { Name = "MyParamType2", Namespace = "MyOtherNamespace" };
            _classModifier.AddConstructor(classDefinition, new [] {
                myParameterType,
                myParameterType2});

            Assert.Equal("myParamType", classDefinition.Methods.First().Signature.Parameters.First().Name);
            Assert.Equal(myParameterType, classDefinition.Methods.First().Signature.Parameters.First().ParameterType);

            Assert.Equal("myParamType2", classDefinition.Methods.First().Signature.Parameters.ElementAt(1).Name);
            Assert.Equal(myParameterType2, classDefinition.Methods.First().Signature.Parameters.ElementAt(1).ParameterType);
        }

        [Fact]
        public void constructor_parameter_with_generic_arguments_itself_has_correct_name()
        {
            var classDefinition = new ClassDefinition
            {
                Name = "MyClass",
                Namespace = "MyNamespace"
            };

            var myParameterType = new ClassDefinition { Name = "MyParamType",
                Namespace = "MyNamespace",
                GenericArguments = new[]
                {
                    new ClassDefinition { Name = "String", Namespace = "System"},
                    new ClassDefinition { Name = "Int", Namespace = "System"}
                } };

            _classModifier.AddConstructor(classDefinition, new[] {
                myParameterType});

            Assert.Equal("myParamTypeOfstringAndint", classDefinition.Methods.First().Signature.Parameters.First().Name);
            Assert.Equal(myParameterType, classDefinition.Methods.First().Signature.Parameters.First().ParameterType);
        }
    }

    public class ClassModifier_ReimplementBaseClassConstructors_Tests
    {
        private Mock<IInstructionGenerator> _instructionGenerator;
        private IFixture _fixture;
        private ClassModifier _classModifier;

        public ClassModifier_ReimplementBaseClassConstructors_Tests()
        {
            _fixture = new Fixture().Customize(new MultipleCustomization())
                                    .Customize(new AutoMoqCustomization());

            _instructionGenerator = _fixture.Freeze<Mock<IInstructionGenerator>>();

            _classModifier = _fixture.Create<ClassModifier>();
        }

        [Fact]
        public void base_class_constructor_parameters_are_kept()
        {
            var stringClassDef = new ClassDefinition
            {
                Namespace = "System",
                Name = "String"
            };
            var baseClass = new ClassDefinition
            {
                Name = "BaseClass",
                Namespace = "MyNamespace",
                Methods = new []
                {
                    new MethodDefinition
                    {
                        AccessModifier = LangObjects.AccessModifier.Public,
                        Signature = new MethodSignature
                        {
                            Name = "BaseClass",
                            Parameters = new []
                            {
                                new MethodParameter
                                {
                                    Name = "myParam1",
                                    ParameterType = stringClassDef
                                }
                            }
                        }
                    }
                }
            };

            var classDefinition = new ClassDefinition
            {
                Name = "Subclass",
                Namespace = "MyNamespace",
                BaseClass = baseClass
            };

            _classModifier.ReimplementBaseClassConstructors(classDefinition, baseClass);

            //Act
            var constructors = GetConstructors(classDefinition.Methods);

            Assert.Contains(stringClassDef, constructors.First().Signature.Parameters.Select(x => x.ParameterType));
            var param = constructors.First().Signature.Parameters.First(x => x.ParameterType == stringClassDef);
            Assert.Equal("myParam1", param.Name);
        }

        private IEnumerable<MethodDefinition> GetConstructors(IEnumerable<MethodDefinition> methods)
        {
            return methods.Where(x => x.ReturnType == null);
        }
    }
}
