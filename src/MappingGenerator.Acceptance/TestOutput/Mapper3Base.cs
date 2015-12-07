public abstract partial class Mapper3Base : AutoGeneration.IMapper<MappingGenerator.Acceptance.TestDataObjects.Foo, MappingGenerator.Acceptance.TestDataObjects.Bar>
{
    private System.Func<AutoGeneration.IMapper> _mapper;
    public Mapper3Base(System.Func<AutoGeneration.IMapper> mapper)
    {
        _mapper = mapper;
    }
    public virtual System.Int32 MapMyProperty(MappingGenerator.Acceptance.TestDataObjects.Foo source)
    {
        return source.MyProperty;
    }
    protected virtual MappingGenerator.Acceptance.TestDataObjects.Bar CreateDestination(MappingGenerator.Acceptance.TestDataObjects.Foo source)
    {
        return new MappingGenerator.Acceptance.TestDataObjects.Bar();
    }
    public virtual MappingGenerator.Acceptance.TestDataObjects.Bar Map(System.Func<MappingGenerator.Acceptance.TestDataObjects.Bar, MappingGenerator.Acceptance.TestDataObjects.Foo> sourceSelector)
    {
        var source = sourceSelector(default(MappingGenerator.Acceptance.TestDataObjects.Bar));
        if (source == default(MappingGenerator.Acceptance.TestDataObjects.Foo))
        {
            return default(MappingGenerator.Acceptance.TestDataObjects.Bar);
        }
        MappingGenerator.Acceptance.TestDataObjects.Bar destination;
        destination = CreateDestination(source);
        destination.MyProperty = MapMyProperty(source);
        return destination;
    }
}
