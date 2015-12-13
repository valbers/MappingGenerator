namespace AutoGeneration
{
    public class IMapper
    {
        private AutoGeneration.IMapper<MappingGenerator.Acceptance.TestDataObjects.ComplexSource, MappingGenerator.Acceptance.TestDataObjects.ComplexDestination> _m0;
        private AutoGeneration.IMapper<System.Collections.Generic.IList<System.String>, System.Collections.Generic.List<System.String>> _m1;
        private AutoGeneration.IMapper<MappingGenerator.Acceptance.TestDataObjects.Foo, MappingGenerator.Acceptance.TestDataObjects.Bar> _m2;
        private AutoGeneration.IMapper<MappingGenerator.Acceptance.TestDataObjects.ComplexSource, MappingGenerator.Acceptance.TestDataObjects.Bar> _m3;
        public IMapper(AutoGeneration.IMapper<MappingGenerator.Acceptance.TestDataObjects.ComplexSource, MappingGenerator.Acceptance.TestDataObjects.ComplexDestination> m0, AutoGeneration.IMapper<System.Collections.Generic.IList<System.String>, System.Collections.Generic.List<System.String>> m1, AutoGeneration.IMapper<MappingGenerator.Acceptance.TestDataObjects.Foo, MappingGenerator.Acceptance.TestDataObjects.Bar> m2, AutoGeneration.IMapper<MappingGenerator.Acceptance.TestDataObjects.ComplexSource, MappingGenerator.Acceptance.TestDataObjects.Bar> m3)
        {
            _m0 = m0;
            _m1 = m1;
            _m2 = m2;
            _m3 = m3;
        }
        public virtual MappingGenerator.Acceptance.TestDataObjects.ComplexDestination Map(System.Func<MappingGenerator.Acceptance.TestDataObjects.ComplexDestination, MappingGenerator.Acceptance.TestDataObjects.ComplexSource> sourceSelector)
        {
            return _m0.Map(sourceSelector);
        }
        public virtual System.Collections.Generic.List<System.String> Map(System.Func<System.Collections.Generic.List<System.String>, System.Collections.Generic.IList<System.String>> sourceSelector)
        {
            return _m1.Map(sourceSelector);
        }
        public virtual MappingGenerator.Acceptance.TestDataObjects.Bar Map(System.Func<MappingGenerator.Acceptance.TestDataObjects.Bar, MappingGenerator.Acceptance.TestDataObjects.Foo> sourceSelector)
        {
            return _m2.Map(sourceSelector);
        }
        public virtual MappingGenerator.Acceptance.TestDataObjects.Bar Map(System.Func<MappingGenerator.Acceptance.TestDataObjects.Bar, MappingGenerator.Acceptance.TestDataObjects.ComplexSource> sourceSelector)
        {
            return _m3.Map(sourceSelector);
        }
    }
}

