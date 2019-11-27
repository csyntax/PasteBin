namespace PasteBin.Services.Web.Mapping
{
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    public class MappingService : IMappingService
    {
        private readonly IMapper mapper;

        public MappingService(IMapper mapper) 
            => this.mapper = mapper;

        public TDestination Map<TDestination>(object source)
            where TDestination : class
            => this.mapper.Map<TDestination>(source);

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
            where TDestination : class
            => this.mapper.Map(source, destination);

        public IQueryable<TDestination> Map<TDestination>(IQueryable source, object parameters = null)
            where TDestination : class
            => source.ProjectTo<TDestination>(this.mapper.ConfigurationProvider, parameters);
    }
}