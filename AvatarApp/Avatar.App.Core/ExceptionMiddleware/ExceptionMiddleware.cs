using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Core.Exceptions;
using Avatar.App.SharedKernel;
using Microsoft.Extensions.Logging;

namespace Avatar.App.Core.ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private  Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception.GetType() == typeof(UserNotAllowedException)) 
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            if (exception.GetType() == typeof(VideoNotFoundException))
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            if (exception.GetType() == typeof(InvalidPasswordException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return  context.Response.WriteAsync("false");
            }

            if (exception.GetType() == typeof(ReachedVideoLimitException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                return context.Response.WriteAsync("false");
            }

            if (exception.GetType() == typeof(IOException))
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            if (exception.GetType() == typeof(IncorrectFragmentIntervalException))
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

            if (exception.GetType() == typeof(UserAlreadyExistsException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                return context.Response.WriteAsync("false");
            }

            return context.Response.WriteAsync("");
        }
    }
}
