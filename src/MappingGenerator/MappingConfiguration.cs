using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public interface IMappingConfiguration
    {
        bool IsMappingConfigured(Type source, Type destination);
        void Register(Type source, Type destination);
        IEnumerable<MappingSpecification> AllMappings();
    }

    public class MappingConfiguration : IMappingConfiguration
    {
        List<MappingSpecification> _mappingSpecifications { get; set; }
        public MappingConfiguration(List<MappingSpecification> mappingSpecifications)
        {
            _mappingSpecifications = mappingSpecifications;
        }
        public MappingConfiguration()
        {
            _mappingSpecifications = new List<MappingSpecification>();
        }

        public bool IsMappingConfigured(Type source, Type destination)
        {
            return _mappingSpecifications.Any(x => x.Source == source && x.Destination == destination);
        }

        public void Register(Type source, Type destination)
        {
            if (!IsMappingConfigured(source, destination))
                _mappingSpecifications.Add(new MappingSpecification {Source = source, Destination = destination});
        }

        public IEnumerable<MappingSpecification> AllMappings()
        {
            return _mappingSpecifications.ToList();
        }
    }
}
