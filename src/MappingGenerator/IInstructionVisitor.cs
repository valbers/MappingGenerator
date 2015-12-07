using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public interface IInstructionVisitor
    {
        object Visit(Instruction instruction);
        object Visit(IfBlockInstruction instruction);
        object Visit(ForEachBlock instruction);
    }
}
