using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public class MappingsContainer : IMappingsContainer
    {
        List<Mapping> _mappings = new List<Mapping>();

        public void Register(Mapping mapping)
        {
            if (!Contains(mapping.Source, mapping.Destination))
                _mappings.Add(mapping);
        }

        public Mapping Resolve(Type source, Type dest)
        {
            return _mappings.FirstOrDefault(x => x.Source == source && x.Destination == dest);
        }

        public bool Contains(Type source, Type dest)
        {
            return _mappings.Any(x => x.Source == source && x.Destination == dest);
        }
    }
}
