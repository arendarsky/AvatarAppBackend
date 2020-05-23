using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Avatar.App.Core.Services.Impl
{
    public class MyHttpContext
    {
        private static IHttpContextAccessor m_httpContextAccessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => m_httpContextAccessor.HttpContext;

        public static string AppBaseUrl => $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}";

        public static void Configure(IHttpContextAccessor contextAccessor)
        {
            m_httpContextAccessor = contextAccessor;
        }
    }
}
