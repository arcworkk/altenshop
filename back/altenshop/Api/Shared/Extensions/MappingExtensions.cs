using AutoMapper;

namespace Api.Shared.Extensions;

public static class MappingExtensions
{
    /// <summary>
    /// Mappe un objet vers un type destination via AutoMapper.
    /// </summary>
    public static TDestination MapTo<TDestination>(this object source, IMapper mapper)
        => mapper.Map<TDestination>(source);

    /// <summary>
    /// Mappe un objet source dans une instance destination existante
    /// </summary>
    public static TDestination MapInto<TSource, TDestination>(this TSource source, TDestination destination, IMapper mapper)
        => mapper.Map(source, destination);
}