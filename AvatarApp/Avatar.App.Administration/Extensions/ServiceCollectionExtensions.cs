using Avatar.App.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Administration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAdministrationComponent(this IServiceCollection services)
        {
            services.AddCronSchedulers(ServiceLifetime.Scoped, typeof(AdministrationComponent).Assembly);
            services.AddScoped<IAdministrationComponent, AdministrationComponent>();
        }

        public static void UseAdministrationComponent(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseCronSchedulers(configuration, typeof(AdministrationComponent).Assembly);
        }
    }
}
