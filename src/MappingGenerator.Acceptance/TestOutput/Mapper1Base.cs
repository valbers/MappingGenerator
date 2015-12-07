public abstract partial class Mapper1Base : AutoGeneration.IMapper<MappingGenerator.Acceptance.TestDataObjects.ComplexSource, MappingGenerator.Acceptance.TestDataObjects.ComplexDestination>
{
    private System.Func<AutoGeneration.IMapper> _mapper;
    public Mapper1Base(System.Func<AutoGeneration.IMapper> mapper)
    {
        _mapper = mapper;
    }
    public virtual System.Collections.Generic.List<System.String> MapListOfString(MappingGenerator.Acceptance.TestDataObjects.ComplexSource source)
    {
        return source.ListOfString;
    }
    public virtual System.Collections.Generic.Dictionary<System.String, System.Object> MapDictionaryOfStringAndObject(MappingGenerator.Acceptance.TestDataObjects.ComplexSource source)
    {
        return source.DictionaryOfStringAndObject;
    }
    public virtual MappingGenerator.Acceptance.TestDataObjects.Bar MapFoo(MappingGenerator.Acceptance.TestDataObjects.ComplexSource source)
    {
        return _mapper().Map((MappingGenerator.Acceptance.TestDataObjects.Bar x) => source.Foo);
    }
    protected virtual MappingGenerator.Acceptance.TestDataObjects.ComplexDestination CreateDestination(MappingGenerator.Acceptance.TestDataObjects.ComplexSource source)
    {
        return new MappingGenerator.Acceptance.TestDataObjects.ComplexDestination();
    }
    public virtual MappingGenerator.Acceptance.TestDataObjects.ComplexDestination Map(System.Func<MappingGenerator.Acceptance.TestDataObjects.ComplexDestination, MappingGenerator.Acceptance.TestDataObjects.ComplexSource> sourceSelector)
    {
        var source = sourceSelector(default(MappingGenerator.Acceptance.TestDataObjects.ComplexDestination));
        if (source == default(MappingGenerator.Acceptance.TestDataObjects.ComplexSource))
        {
            return default(MappingGenerator.Acceptance.TestDataObjects.ComplexDestination);
        }
        MappingGenerator.Acceptance.TestDataObjects.ComplexDestination destination;
        destination = CreateDestination(source);
        destination.ListOfString = MapListOfString(source);
        destination.DictionaryOfStringAndObject = MapDictionaryOfStringAndObject(source);
        destination.Foo = MapFoo(source);
        return destination;
    }
}
