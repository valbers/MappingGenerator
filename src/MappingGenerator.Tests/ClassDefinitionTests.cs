using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MappingGenerator.Tests
{
    public class ClassDefinitionTests
    {
        [Fact]
        public void foo_class_definition_equals_foo_type()
        {
            var fooClassDefinition = new ClassDefinition
            {
                Name = "Foo",
                Namespace = "MappingGenerator.Tests",
            };

            Assert.True(fooClassDefinition == typeof(Foo));
            Assert.True(typeof(Foo) == fooClassDefinition);
        }

        [Fact]
        public void when_type_is_converted_to_class_definition_class_definition_name_doesnt_contain_accent()
        {
            ClassDefinition classDefinition = typeof(List<string>);
            Assert.DoesNotContain("`", classDefinition.Name);
        }

        [Fact]
        public void list_of_string_class_defintion_equals_list_of_string_type()
        {
            var listOfString = new ClassDefinition
            {
                Name = "List",
                Namespace = "System.Collections.Generic",
                GenericArguments = new [] 
                {
                    new ClassDefinition
                    {
                        Name = "String",
                        Namespace = "System"
                    }
                }
            };

            Assert.True(listOfString == typeof(List<string>));
            Assert.True(typeof(List<string>) == listOfString);
        }
    }
}
