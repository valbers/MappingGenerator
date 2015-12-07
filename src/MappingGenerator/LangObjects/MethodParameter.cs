using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator.LangObjects
{
    public class MethodParameter
    {
        public ClassDefinition ParameterType { get; set; }

        public string Name { get; set; }
    }
}
