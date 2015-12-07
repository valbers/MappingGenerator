using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator.LangObjects
{
    public class InstanceVariable
    {
        public string Name { get; set; }

        public InstanceVariableType InstanceVariableType { get; set; }

        public ClassDefinition Type { get; set; }

        public AccessModifier AccessModifier { get; set; }

        public IEnumerable<Modifier> OtherModifiers { get; set; }

        public InstanceVariable()
        {
            OtherModifiers = new List<Modifier>();
        }
    }
}
