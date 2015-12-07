using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator.LangObjects
{
    public class MethodSignature
    {
        public string Name { get; set; }

        public IEnumerable<MethodParameter> Parameters { get; set; }

        public IEnumerable<ClassDefinition> GenericArguments { get; set; }
    }
}
