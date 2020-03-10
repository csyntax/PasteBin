namespace PasteBin.Web.Infrastructure.Mapping
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;

    using AutoMapper;

    public static class AutoMapperConfig
    {
        public static IConfigurationProvider MapperConfiguration { get; private set; }

        public static void RegisterMappings(Assembly assembly)
        {
            var types = assembly.GetExportedTypes();

            MapperConfiguration = new MapperConfiguration(configuration =>
            {
                // IMapFrom<>
                foreach (var map in GetFromMaps(types))
                {
                    configuration.CreateMap(map.Source, map.Destination);
                }

                // IMapTo<>
                foreach (var map in GetToMaps(types))
                {
                    configuration.CreateMap(map.Source, map.Destination);
                }

                // IHaveCustomMappings
                foreach (var map in GetCustomMappings(types))
                {
                    map.CreateMappings(configuration);
                }
            });
        }

        private static IEnumerable<TypeMap> GetFromMaps(Type[] types) =>
             from t in types
             from i in t.GetTypeInfo().GetInterfaces()
             where i.GetTypeInfo().IsGenericType &&
                 i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                 !t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsInterface
             select new TypeMap
             {
                 Source = i.GetTypeInfo().GetGenericArguments().First(),
                 Destination = t,
             };

        private static IEnumerable<TypeMap> GetToMaps(Type[] types) =>
            from t in types
            from i in t.GetTypeInfo().GetInterfaces()
            where i.GetTypeInfo().IsGenericType &&
                i.GetTypeInfo().GetGenericTypeDefinition() == typeof(IMapTo<>) &&
                !t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsInterface
            select new TypeMap
            {
                Source = t,
                Destination = i.GetTypeInfo().GetGenericArguments().First(),
            };

        private static IEnumerable<IHaveCustomMappings> GetCustomMappings(Type[] types) =>
             from t in types
             from i in t.GetTypeInfo().GetInterfaces()
             where typeof(IHaveCustomMappings).GetTypeInfo().IsAssignableFrom(t) &&
                 !t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsInterface
             select (IHaveCustomMappings) Activator.CreateInstance(t);
    }
}