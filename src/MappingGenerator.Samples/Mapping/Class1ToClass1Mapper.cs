﻿using MappingGenerator.Samples.Mapping.AutoGenerated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator.Samples.Mapping
{
    public class Class1ToClass1Mapper : AutoGenerated.Mapper1Base
    {
        public Class1ToClass1Mapper(IMapper mapper) : base(mapper)
        {
        }

        protected override double? MapMyNullableDouble(MappingGenerator.Samples.Domain1.Class1 source)
        {
            if (source.MyNullableDouble == null)
                throw new Exception("We don't accept NULL values in any case!");

            return base.MapMyNullableDouble(source);
        }

        public override MappingGenerator.Samples.Domain2.Class1 Map(MappingGenerator.Samples.Domain1.Class1 source)
        {
            var destination = base.Map(source);
            destination.MyOwnThingProperty = source.MyUniqueProperty;
            return destination;
        }
    }
}
