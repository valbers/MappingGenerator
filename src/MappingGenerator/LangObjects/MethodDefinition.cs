using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator.LangObjects
{
    public class MethodDefinition
    {
        public AccessModifier AccessModifier { get; set; }

        public IEnumerable<Modifier> OtherModifiers { get; set; }

        public MethodSignature Signature { get; set; }

        public ClassDefinition ReturnType { get; set; }

        public IEnumerable<Instruction> Body { get; set; }

        public bool IsBaseConstructorReimplementation { get; set; }

        public MethodDefinition()
        {
            OtherModifiers = new List<Modifier>();
            Body = new List<Instruction>();
        }
    }
}
