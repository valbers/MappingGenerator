namespace AutoGeneration
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(System.Func<TDestination, TSource> sourceSelector);
    }
}
