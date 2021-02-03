using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Authentication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthenticationComponent(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationComponent, AuthenticationComponent>();
            services.AddMediatR(typeof(AuthenticationComponent).Assembly);
        }
    }
}
