public abstract partial class Mapper2Base : AutoGeneration.IMapper<System.Collections.Generic.IList<System.String>, System.Collections.Generic.List<System.String>>
{
    private System.Func<AutoGeneration.IMapper> _mapper;
    public Mapper2Base(System.Func<AutoGeneration.IMapper> mapper)
    {
        _mapper = mapper;
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
        return destination;
    }
}
