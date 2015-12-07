using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator.Samples.Domain1
{
    public class Class1
    {
        public int MyInteger { get; set; }

        public string MyString { get; set; }

        public double? MyNullableDouble { get; set; }

        public float? MyFloat { get; set; }

        public Class2 MyClass2 { get; set; }

        public string MyUniqueProperty { get; set; }

        public List<string> MyListOfStrings { get; set; }

        public IEnumerable<string> MyListOfStrings2 { get; set; }

        public IEnumerable<string> MyListOfStrings3 { get; set; }
    }
}
