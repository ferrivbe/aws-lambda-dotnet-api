// ***********************************************************************
// <copyright file="HttpLoggingMiddlewareExtension.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Microsoft.AspNetCore.Builder;
using Serverless.Api.Common.Settings;

namespace Serverless.Api.Middleware.HttpLogger
{
    public static class HttpLoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomHttpLogging(this IApplicationBuilder builder, ServiceSettings settings)
        {
            return builder.UseMiddleware<HttpLoggingMiddleware>(settings);
        }
    }
}

