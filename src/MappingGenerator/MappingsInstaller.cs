using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public class MappingsInstaller
    {
        readonly IMappingsContainer _mappingsContainer;
        readonly IMappingCreator _mappingCreator;

        public MappingsInstaller(IMappingsContainer mappingsContainer, IMappingCreator mappingCreator)
        {
            _mappingsContainer = mappingsContainer;
            _mappingCreator = mappingCreator;
        }

        public IMappingsContainer InstallMappings(IEnumerable<KeyValuePair<Type, Type>> sourcesAndDestinations)
        {
            foreach(var sourceAndDestination in sourcesAndDestinations)
            {
                if (!_mappingsContainer.Contains(sourceAndDestination.Key, sourceAndDestination.Value))
                    _mappingsContainer.Register(_mappingCreator.CreateMapping(sourceAndDestination.Key, sourceAndDestination.Value));
            }

            return _mappingsContainer;
        }
    }
}
