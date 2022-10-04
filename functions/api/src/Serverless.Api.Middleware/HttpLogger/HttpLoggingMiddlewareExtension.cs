// ***********************************************************************
// <copyright file="HttpLoggingMiddlewareExtension.cs">
//     Serverless example
// </copyright>
// ***********************************************************************

using Microsoft.AspNetCore.Builder;

namespace Serverless.Api.Middleware.HttpLogger
{
    public static class HttpLoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomHttpLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpLoggingMiddleware>();
        }
    }
}

