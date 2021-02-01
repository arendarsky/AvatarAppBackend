using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Semifinal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Final.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFinalComponent(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(FinalComponent).Assembly);
            services.AddScoped<IFinalistsSetter, FinalComponent>();
            services.AddScoped<IFinalComponent, FinalComponent>();
        }
    }
}
