using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.IO;

namespace MappingGenerator.Acceptance
{
    public class DefaultClassRendererTests
    {
        [Fact]
        public void should_render_class()
        {
            var dependencyDefinition = new ClassDefinition
            {
                AccessModifier = AccessModifier.Public,
                Name = "IMyDependency",
                Namespace = "Services",
                IsInterface = true
            };

            var classDefinition = new ClassDefinition
            {
                AccessModifier = AccessModifier.Public,
                InstanceVariables = new [] 
                {
                    new InstanceVariable
                    {
                        InstanceVariableType = InstanceVariableType.Field,
                        AccessModifier = AccessModifier.Private,
                        Type = dependencyDefinition,
                        Name = "_myDependency"
                    }
                },
                Name = "MyClass",
                Methods = new[] 
                { 
                    new MethodDefinition
                    {
                        Signature = new MethodSignature
                        {
                            Name = "MyClass",
                            Parameters = new []
                            {
                                new MethodParameter
                                {
                                    Name = "myDependency",
                                    ParameterType = dependencyDefinition
                                }
                            }
                        },
                        Body = new []
                        {
                            new Instruction {Code = "_myDependency = myDependency;"}
                        }
                    }
                }
            };
            var defaultClassRenderer = new DefaultClassRenderer();

            OutputFile(defaultClassRenderer, dependencyDefinition);
            OutputFile(defaultClassRenderer, classDefinition);
        }

        private void OutputFile(DefaultClassRenderer defaultClassRenderer, ClassDefinition classDefinition)
        {
            var testOutputFile = File.Create(string.Concat("..\\..\\TestOutput\\", classDefinition.Name, ".cs"));
            using (var streamWriter = new StreamWriter(testOutputFile))
            {
                defaultClassRenderer.RenderClass(classDefinition, streamWriter);
            }
        }
    }
}
