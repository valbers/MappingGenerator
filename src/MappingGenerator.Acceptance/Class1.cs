using AutoGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using MappingGenerator.Acceptance.TestDataObjects;

namespace MappingGenerator.Acceptance
{
    public class Class1
    {
        private IMapper _mapper;
        private IFixture _fixture;
        public Class1()
        {
            _fixture = new Fixture().Customize(new MultipleCustomization());

            _mapper = new IMapper(new Mapper1(() => _mapper), new Mapper2(() => _mapper), new Mapper3(() => _mapper), new Mapper4(() => _mapper));
        }

        [Fact]
        public void Test()
        {
            var complexSource = _fixture.Create<ComplexSource>();
            var complexDestination = _mapper.Map((ComplexDestination x) => complexSource);
            var bar = _mapper.Map((Bar x) => complexSource);
            Assert.Equal(complexSource.DictionaryOfStringAndObject.First().Key, complexDestination.DictionaryOfStringAndObject.First().Key);
        }
    }
}
