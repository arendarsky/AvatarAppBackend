using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace Avatar.App.Core.ExceptionMiddleware
{
    public static class ExceptionMiddlewareExtensions
    { 
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
