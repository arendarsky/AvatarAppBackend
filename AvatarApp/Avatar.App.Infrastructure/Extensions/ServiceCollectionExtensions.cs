using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Settings;
using Avatar.App.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(AvatarAppContext).Assembly);
            var connection = configuration["DB_CONNECTION"];
            services.AddDbContext<AvatarAppContext>(options =>
                options.UseNpgsql(connection, b => b.MigrationsAssembly(typeof(AvatarAppContext).Assembly.FullName)));
            services.AddAutoMapper(typeof(AvatarAppContext).Assembly);
            services.AddGenericHandlers();
            services.Configure<EnvironmentConfig>(configuration);
            services.AddSingleton<IQueryManager, QueryManager>();
        }

        private static void AddGenericHandlers(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var mapperProvider = provider.GetService<IMapper>().ConfigurationProvider;
            var typeMaps = mapperProvider.GetAllTypeMaps();
            services.AddGenericHandlers(typeMaps);
        }

        private static void AddGenericHandlers(this IServiceCollection services, TypeMap[] typeMaps)
        {
            var openHandlerTypes = typeof(ServiceCollectionExtensions).Assembly.DefinedTypes.Where(type =>
                type.IsClass && !type.IsAbstract && type.IsAssignableToGenericType(typeof(IGenericHandler<,>)));

            foreach (var openHandlerType in openHandlerTypes)
            {
                services.AddGenericHandler(openHandlerType, typeMaps);
            }
        }

        private static void AddGenericHandler(this IServiceCollection services, TypeInfo openHandlerType,
            IEnumerable<TypeMap> typeMaps)
        {
            var constraints = openHandlerType.ImplementedInterfaces.Last().GenericTypeArguments;
            var filteredMaps = typeMaps.Where(typeMap =>
                constraints[0].IsAssignableFrom(typeMap.SourceType) &&
                constraints[1].IsAssignableFrom(typeMap.DestinationType));
            foreach (var typeMap in filteredMaps)
            {
                services.AddGenericHandler(openHandlerType, typeMap.SourceType, typeMap.DestinationType);
            }

        }

        private static void AddGenericHandler(this IServiceCollection services, Type handlerType, Type sourceType, Type destinationType)
        {
            var closedHandlerType = handlerType.MakeGenericType(sourceType, destinationType);
            var interfaceHandlerType = closedHandlerType.GetInterfaces()[0];
            services.Add(ServiceDescriptor.Transient(interfaceHandlerType, closedHandlerType));
        }

        private static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
            {
                return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = givenType.BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }
    }
}
