using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MappingGenerator
{
    public interface IMappingsContainer
    {
        void Register(Mapping mapping);
        Mapping Resolve(Type source, Type dest);
        bool Contains(Type source, Type dest);
    }
}
