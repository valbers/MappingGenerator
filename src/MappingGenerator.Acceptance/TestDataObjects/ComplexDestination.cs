using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator.Acceptance.TestDataObjects
{
    public class ComplexDestination
    {
        public List<string> ListOfString { get; set; }

        public Dictionary<string, object> DictionaryOfStringAndObject { get; set; }

        public Bar Foo { get; set; }
    }
}
