using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Avatar.App.Infrastructure.AutoMapperProfiles;
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
            services.AddAutoMapper(typeof(CastingProfile));
        }
    }
}
