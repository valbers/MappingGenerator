using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace MappingGenerator.Tests
{
    public class MappingCreatorTests
    {
        MappingCreator _mappingCreator;

        public MappingCreatorTests()
        {
            _mappingCreator = new MappingCreator();
        }

        [Fact]
        public void number_of_mapping_rules_matches_number_of_properties_with_equal_names()
        {
            var mapping = _mappingCreator.CreateMapping(typeof(Foo), typeof(Bar));

            Assert.Equal(1, mapping.PropertiesMappingRules.Count());
        }

        [Fact]
        public void name_of_source_is_same_as_the_propertys()
        {
            var mapping = _mappingCreator.CreateMapping(typeof(Foo), typeof(Bar));

            Assert.Equal("MyProperty", mapping.PropertiesMappingRules.ElementAt(0).Source.Name);
        }

        [Fact]
        public void type_of_source_is_that_of_property()
        {
            var mapping = _mappingCreator.CreateMapping(typeof(Foo), typeof(Bar));

            Assert.Equal(typeof(int), mapping.PropertiesMappingRules.ElementAt(0).Source.Type);
        }

        [Fact]
        public void name_of_destination_is_same_as_the_propertys()
        {
            var mapping = _mappingCreator.CreateMapping(typeof(Foo), typeof(Bar));

            Assert.Equal("MyProperty", mapping.PropertiesMappingRules.ElementAt(0).Destination.Name);
        }

        [Fact]
        public void type_of_destination_is_that_of_property()
        {
            var mapping = _mappingCreator.CreateMapping(typeof(Foo), typeof(Bar));

            Assert.Equal(typeof(int), mapping.PropertiesMappingRules.ElementAt(0).Destination.Type);
        }

        [Fact]
        public void indexers_are_not_mapped()
        {
            var mapping = _mappingCreator.CreateMapping(typeof(IList<string>), typeof(List<string>));

            Assert.DoesNotContain("Item", mapping.PropertiesMappingRules.Select(x => x.Source.Name));
        }
    }
}
