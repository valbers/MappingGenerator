using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MappingGenerator.LangObjects;

namespace MappingGenerator
{
    public class Mapping
    {
        public Type Source { get; set; }

        public Type Destination { get; set; }

        public IEnumerable<MappingRule> PropertiesMappingRules { get; set; }
    }
}
