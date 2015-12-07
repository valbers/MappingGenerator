using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public class MappingRule
    {
        public MappingRuleParticipant Source { get; set; }

        public MappingRuleParticipant Destination { get; set; }
    }
}
