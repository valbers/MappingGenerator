using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator.LangObjects
{
    public class IfBlockInstruction : Instruction
    {
        public BooleanExpression BooleanExpression { get; set; }

        public Instruction TrueCaseInstruction { get; set; }

        public Instruction ElseCaseInstruction { get; set; }
    }
}
