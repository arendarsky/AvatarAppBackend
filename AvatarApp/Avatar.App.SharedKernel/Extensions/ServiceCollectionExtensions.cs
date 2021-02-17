using Avatar.App.SharedKernel.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.SharedKernel.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAvatarAppTools(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AvatarAppSettings>(configuration.GetSection("Avatar.App.Settings"));
        }
    }
}
