using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MappingGenerator.LangObjects;

namespace MappingGenerator
{
    public class MappingCreator : MappingGenerator.IMappingCreator
    {
        public Mapping CreateMapping(Type source, Type dest)
        {
            var mapping = new Mapping
            {
                Source = source,
                Destination = dest
            };

            mapping.PropertiesMappingRules = CreateMappingRules(source, dest);

            return mapping;
        }

        protected virtual bool DoPropertiesMatch(PropertyInfo sourceProp, PropertyInfo destProp)
        {
            return string.Compare(destProp.Name, sourceProp.Name, ignoreCase: true) == 0;
        }

        private IEnumerable<MappingRule> CreateMappingRules(Type source, Type dest)
        {
            var sourceProperties = GetProperties(source);
            var destProperties = GetProperties(dest);

            foreach(var destProp in destProperties.Where(x => x.CanWrite))
            {
                dynamic sourceProp = sourceProperties.FirstOrDefault(x => DoPropertiesMatch(destProp, x));
                if (sourceProp == null)
                    sourceProp = new { Name = source.Name, PropertyType = source };

                yield return new MappingRule
                {
                    Source = new MappingRuleParticipant { Name = sourceProp.Name, Type = sourceProp.PropertyType },
                    Destination = new MappingRuleParticipant { Name = destProp.Name, Type = destProp.PropertyType },
                };
            }
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties().Where(x => !x.GetIndexParameters().Any());
        }
    }
}
