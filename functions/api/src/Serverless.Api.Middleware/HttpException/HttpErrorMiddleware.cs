// ***********************************************************************
// <copyright file="HttpErrorMiddleware.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Serverless.Api.Common.Constants;
using Serverless.Api.Common.Settings;
using Serverless.Api.Middleware.HttpLogger;
using Serverless.Api.Models.DataTranferObjects.Error;
using Serverless.Api.Models.DataTranferObjects.Logger;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Serverless.Api.Middleware.HttpException
{
    /// <summary>
    /// Handles HTTP logging.
    /// </summary>
    public class HttpErrorMiddleware
    {
        private readonly ILogger<HttpErrorMiddleware> logger;
        private readonly RequestDelegate requestProcess;
        public readonly ServiceSettings settings;

        /// <summary>
        /// Creates a new instrance of <see cref="HttpErrorMiddleware"/>.
        /// </summary>
        /// <param name="logger">The logger representig an instance of <see cref="ILogger{HttpLoggingMiddleware}"/>.</param>
        /// /// <param name="requestProcess">The request delegate process.</param>
        public HttpErrorMiddleware(ILogger<HttpErrorMiddleware> logger, RequestDelegate requestProcess, ServiceSettings settings)
        {
            this.logger = logger;
            this.requestProcess = requestProcess;
            this.settings = settings;
        }

        /// <summary>
        /// Invoke method.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.requestProcess(context).ConfigureAwait(false);
            }
            catch (Models.Extensions.HttpException exception)
            {
                var errorMessage = new HttpErrorDto(
                    exception.Message,
                    exception.ErrorCode,
                    context.Request.Host.Host);

                this.logger.LogError<HttpErrorDto>(context, errorMessage, this.settings.ServiceId);

                context.Response.ContentType = MimeTypes.ApplicationJson;
                context.Response.StatusCode = exception.StatusCode;

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorMessage)).ConfigureAwait(false);
            }
        }
    }
}

