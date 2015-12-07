using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MappingGenerator.Tests
{
    public class MyType
    {
        public string DestinationType { get; set; }
        public string SourceType { get; set; }
    }

    public class TestingT4Templates
    {
        [Fact]
        public void MyTestMethod()
        {
            var mappingClassTemplate = new MappingClassTemplate();
            mappingClassTemplate.Session = new Dictionary<string, object>();
            mappingClassTemplate.Session["model"] = new MyType { SourceType = "Domain1.Class1", DestinationType = "Domain2.Class1" };
            mappingClassTemplate.Initialize();
            var generatedClass = mappingClassTemplate.TransformText();
        }
    }
}
