// ***********************************************************************
// <copyright file="HttpLoggingMiddleware.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Serverless.Api.Common.Constants;
using Serverless.Api.Common.Settings;
using Serverless.Api.Models.DataTranferObjects.Logger;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Serverless.Api.Middleware.HttpLogger
{
    /// <summary>
    /// Handles HTTP logging.
    /// </summary>
    public class HttpLoggingMiddleware
    {
        private readonly ILogger<HttpLoggingMiddleware> logger;
        private readonly RequestDelegate requestProcess;
        private readonly ServiceSettings settings;
        private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;

        /// <summary>
        /// Creates a new instrance of <see cref="HttpLoggingMiddleware"/>.
        /// </summary>
        /// <param name="requestProcess">The request delegate process.</param>
        /// <param name="logger">The logger representig an instance of <see cref="ILogger{HttpLoggingMiddleware}"/>.</param>
        /// <param name="settings">The service settings.</param>
        public HttpLoggingMiddleware(RequestDelegate requestProcess, ILogger<HttpLoggingMiddleware> logger, ServiceSettings settings)
        {
            this.logger = logger;
            this.requestProcess = requestProcess;
            this.settings = settings;
            this.recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        /// <summary>
        /// Invoke method.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }

        /// <summary>
        /// Logs an incoming HTTP request.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task LogRequest(HttpContext context)
        {
            await this.logger.LogRequest(context, this.recyclableMemoryStreamManager, this.settings.ServiceId);
        }

        /// <summary>
        /// Logs an outcome HTTP response.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task LogResponse(HttpContext context)
        {
            await this.logger.LogResponse(context, recyclableMemoryStreamManager, requestProcess, this.settings.ServiceId);
        }
    }
}

