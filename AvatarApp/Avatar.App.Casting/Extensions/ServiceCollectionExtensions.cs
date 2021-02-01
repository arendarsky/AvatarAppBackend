using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Casting.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCastingComponent(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CastingComponent).Assembly);
        }
    }
}
