using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator.LangObjects
{
    public class ForEachBlock : Instruction
    {
        public string IEnumerableVariable { get; set; }

        public Instruction BlockBody { get; set; }
    }
}
