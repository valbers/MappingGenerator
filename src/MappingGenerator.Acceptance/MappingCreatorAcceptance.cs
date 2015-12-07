using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MappingGenerator.Acceptance.TestDataObjects;
using Ploeh.AutoFixture;

namespace MappingGenerator.Acceptance
{
    public class MappingCreatorAcceptance
    {
        MappingCreator _mappingCreator;

        public MappingCreatorAcceptance()
        {
            var mappingConfiguration = new MappingConfiguration(new List<MappingSpecification>());
            var instructionGenerator = new InstructionGenerator();
            _mappingCreator = new MappingCreator();
        }

        [Fact]
        public void class_with_one_property_is_mapped_to_class_with_one_property()
        {
            var mapping = _mappingCreator.CreateMapping(typeof(Foo), typeof(Bar));

            Assert.Equal(typeof(Foo), mapping.Source);
            Assert.Equal(typeof(Bar), mapping.Destination);
            Assert.Equal(1, mapping.PropertiesMappingRules.Count());
            Assert.Equal("MyProperty", mapping.PropertiesMappingRules.ElementAt(0).Source.Name);
            Assert.Equal("MyProperty", mapping.PropertiesMappingRules.ElementAt(0).Destination.Name);
        }
    }
}
