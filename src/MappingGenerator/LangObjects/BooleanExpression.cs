using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator.LangObjects
{
    public class BooleanExpression
    {
        public string LeftOperand { get; set; }

        public string RightOperand { get; set; }

        public string Operator { get; set; }
    }
}
