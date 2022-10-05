// ***********************************************************************
// <copyright file="HttpLoggingMiddlewareExtension.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Microsoft.AspNetCore.Builder;
using Serverless.Api.Common.Settings;

namespace Serverless.Api.Middleware.HttpException
{
    public static class HttpErrorMiddlewareExtension
    {
        public static IApplicationBuilder UseHttpExceptionHendler(this IApplicationBuilder builder, ServiceSettings settings)
        {
            return builder.UseMiddleware<HttpErrorMiddleware>(settings);
        }
    }
}

