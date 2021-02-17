using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Profile.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddProfileComponent(this IServiceCollection services)
        {
            services.AddScoped<IProfileComponent, ProfileComponent>();
        }
    }
}
