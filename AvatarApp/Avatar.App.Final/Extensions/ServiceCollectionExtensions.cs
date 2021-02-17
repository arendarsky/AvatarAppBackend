using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Final.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFinalComponent(this IServiceCollection services)
        {
            services.AddScoped<IFinalComponent, FinalComponent>();
        }
    }
}
