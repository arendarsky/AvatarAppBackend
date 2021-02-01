using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Semifinal.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSemifinalComponent(this IServiceCollection services)
        {
            services.AddMediatR(typeof(SemifinalComponent).Assembly);
            services.AddScoped<ISemifinalComponent, SemifinalComponent>();
            services.AddAutoMapper(typeof(SemifinalProfile));
        }
    }
}
