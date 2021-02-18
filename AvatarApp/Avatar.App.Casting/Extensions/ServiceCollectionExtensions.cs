using Avatar.App.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Casting.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCastingComponent(this IServiceCollection services)
        {
            services.AddCronSchedulers(ServiceLifetime.Scoped, typeof(CastingComponent).Assembly);
            services.AddScoped<ICastingComponent, CastingComponent>();
        }

        public static void UseCastingComponent(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseCronSchedulers(configuration, typeof(CastingComponent).Assembly);
        }
    }
}
