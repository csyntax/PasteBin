namespace PasteBin.Web.Infrastructure.Extensions
{
    using Microsoft.Extensions.DependencyInjection;

    using AutoMapper;

    using Infrastructure.Mapping;
    using Infrastructure.ViewModels;

    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);

            services.AddSingleton(AutoMapperConfig.MapperConfiguration);

            services.AddScoped<IMapper>(p => new Mapper(p.GetRequiredService<IConfigurationProvider>(), p.GetService));

            return services;
        }
    }
}
