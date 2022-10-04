// ***********************************************************************
// <copyright file="HttpLoggingMiddleware.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serverless.Api.Common.Constants;
using Serverless.Api.Common.Settings;
using Serverless.Api.Models.Models.Logger;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Serverless.Api.Middleware.HttpLogger
{
    public class HttpLoggingMiddleware
    {
        private readonly ILogger<HttpLoggingMiddleware> logger;
        private readonly JsonSerializerOptions serializerOptions;
        private readonly RequestDelegate requestProcess;
        private readonly ServiceSettings settings;

        public HttpLoggingMiddleware(RequestDelegate requestProcess, ILogger<HttpLoggingMiddleware> logger, ServiceSettings settings)
        {
            this.logger = logger;
            this.requestProcess = requestProcess;
            this.settings = settings;

            this.serializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }


        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }


        private async Task LogRequest(HttpContext context)
        {
            var logMessage = CreateRequestLog(context);
            this.logger.LogInformation(logMessage );
        }

        private async Task LogResponse(HttpContext context)
        {
            await requestProcess(context);
            this.logger.LogInformation("response");
        }

        private string CreateRequestLog(HttpContext context)
            => CreateLog(context, LogSeverity.Info, LogOperation.ClientRequest);

        private string CreateLog(HttpContext context, LogSeverity severity, LogOperation operation)
        {
            var log = new LogDetail
            {
                Severity = severity,
                Target = new LogTarget
                {
                    Method = context.Request.Method,
                    Host = context.Request.Host.Host,
                    Route = context.Request.Path,
                },
                Environment = Environment.GetEnvironmentVariable(EnvironmentVariables.Stage),
                Operation = operation,
                RequestId = context.Request.Headers["requestId"],
                ServiceId = this.settings.ServiceId,
                XForwardedFor = context.Request.Headers["X-Forwarded-For"],
                Timestamp = DateTime.Now,
                Message = "hello from lambda"
            };

            return JsonSerializer.Serialize(log);
        }
    }
}

