using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Rating.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRatingComponent(this IServiceCollection services)
        {
            services.AddScoped<IRatingComponent, RatingComponent>();
        }
    }
}
