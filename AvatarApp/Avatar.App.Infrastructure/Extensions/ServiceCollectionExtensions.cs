using System;
using System.Linq;
using AutoMapper;
using Avatar.App.Infrastructure.Handlers.Abstract;
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
            services.AddGenericHandlers<IGenericQueryHandler>(typeof(IRequestHandler<,>));
            services.AddGenericHandlers<IGenericCommandHandler>(typeof(IRequestHandler<>));
        }

        private static void AddGenericHandlers<TDefinitionInterface>(this IServiceCollection services, Type handlerInterfaceType)
        {
            var provider = services.BuildServiceProvider();
            var mapperProvider = provider.GetService<IMapper>().ConfigurationProvider;
            var typeMaps = mapperProvider.GetAllTypeMaps();

            foreach (var typeMap in typeMaps)
            {
                services.AddGenericHandlers<TDefinitionInterface>(handlerInterfaceType, typeMap.SourceType, typeMap.DestinationType);
            }
        }

        private static void AddGenericHandlers<TDefinitionInterface>(this IServiceCollection services, Type handlerInterfaceType, Type sourceType, Type destinationType)
        {
            var basicHandlerTypes = typeof(AvatarAppContext).Assembly.DefinedTypes.Where(type =>
                type.IsClass && !type.IsAbstract && type.GetInterface(nameof(TDefinitionInterface)) != null);

            foreach (var basicHandlerType in basicHandlerTypes)
            {
                services.AddGenericHandler(handlerInterfaceType, basicHandlerType, sourceType, destinationType);
            }
        }

        private static void AddGenericHandler(this IServiceCollection services, Type handlerInterfaceType, Type handlerType, Type sourceType, Type destinationType)
        {
            var closedHandlerType = handlerType.MakeGenericType(sourceType, destinationType);
            var interfaceHandlerType = closedHandlerType.GetInterfaces().First(inter => inter.IsAssignableFrom(handlerInterfaceType));
            services.Add(ServiceDescriptor.Transient(interfaceHandlerType, closedHandlerType));
        }
    }
}
