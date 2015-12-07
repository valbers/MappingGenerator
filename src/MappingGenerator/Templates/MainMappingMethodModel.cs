using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator.Templates
{
    public class MainMappingMethodModel
    {
        public Mapping Mapping { get; set; }

        public MethodDefinition MethodDefinition { get; set; }

        public string DestinationVariableName { get; set; }

        public List<LangObjects.MethodDefinition> PropertyMappingMethods { get; set; }
    }
}
