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

public abstract partial class Mapper2Base : AutoGeneration.IMapper<System.Collections.Generic.IList<System.String>, System.Collections.Generic.List<System.String>>
{
    private System.Func<AutoGeneration.IMapper> _mapper;
    public Mapper2Base(System.Func<AutoGeneration.IMapper> mapper)
    {
        _mapper = mapper;
    }
    public virtual System.Int32 MapCapacity(System.Collections.Generic.IList<System.String> source)
    {
        return default(System.Int32);
    }
    protected virtual System.Collections.Generic.List<System.String> CreateDestination(System.Collections.Generic.IList<System.String> source)
    {
        return new System.Collections.Generic.List<System.String>();
    }
    public virtual System.Collections.Generic.List<System.String> Map(System.Func<System.Collections.Generic.List<System.String>, System.Collections.Generic.IList<System.String>> sourceSelector)
    {
        var source = sourceSelector(default(System.Collections.Generic.List<System.String>));
        if (source == default(System.Collections.Generic.IList<System.String>))
        {
            return default(System.Collections.Generic.List<System.String>);
        }
        System.Collections.Generic.List<System.String> destination;
        destination = CreateDestination(source);
        destination.Capacity = MapCapacity(source);
        return destination;
    }
}

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

public abstract partial class Mapper4Base : AutoGeneration.IMapper<MappingGenerator.Acceptance.TestDataObjects.ComplexSource, MappingGenerator.Acceptance.TestDataObjects.Bar>
{
    private System.Func<AutoGeneration.IMapper> _mapper;
    public Mapper4Base(System.Func<AutoGeneration.IMapper> mapper)
    {
        _mapper = mapper;
    }
    public virtual System.Int32 MapMyProperty(MappingGenerator.Acceptance.TestDataObjects.ComplexSource source)
    {
        return default(System.Int32);
    }
    protected virtual MappingGenerator.Acceptance.TestDataObjects.Bar CreateDestination(MappingGenerator.Acceptance.TestDataObjects.ComplexSource source)
    {
        return new MappingGenerator.Acceptance.TestDataObjects.Bar();
    }
    public virtual MappingGenerator.Acceptance.TestDataObjects.Bar Map(System.Func<MappingGenerator.Acceptance.TestDataObjects.Bar, MappingGenerator.Acceptance.TestDataObjects.ComplexSource> sourceSelector)
    {
        var source = sourceSelector(default(MappingGenerator.Acceptance.TestDataObjects.Bar));
        if (source == default(MappingGenerator.Acceptance.TestDataObjects.ComplexSource))
        {
            return default(MappingGenerator.Acceptance.TestDataObjects.Bar);
        }
        MappingGenerator.Acceptance.TestDataObjects.Bar destination;
        destination = CreateDestination(source);
        destination.MyProperty = MapMyProperty(source);
        return destination;
    }
}

