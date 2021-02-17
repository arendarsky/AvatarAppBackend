using Avatar.App.Schedulers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Semifinal.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSemifinalComponent(this IServiceCollection services)
        {
            services.AddCronSchedulers(ServiceLifetime.Scoped, typeof(SemifinalComponent).Assembly);
            services.AddScoped<ISemifinalComponent, SemifinalComponent>();
        }

        public static void UseSemifinalComponent(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseCronSchedulers(configuration, typeof(SemifinalComponent).Assembly);
        }
    }
}
