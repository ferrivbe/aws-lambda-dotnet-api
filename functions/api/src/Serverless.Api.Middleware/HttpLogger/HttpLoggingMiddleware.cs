// ***********************************************************************
// <copyright file="HttpLoggingMiddleware.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serverless.Api.Models.Models.Logger;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Serverless.Api.Middleware.HttpLogger
{
    public class HttpLoggingMiddleware
    {
        private readonly ILogger<HttpLoggingMiddleware> logger;
        private readonly JsonSerializerOptions serializerOptions;


        public HttpLoggingMiddleware(RequestDelegate _requestProcess, ILogger<HttpLoggingMiddleware> logger)
        {
            this.logger = logger;

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
            this.logger.LogInformation("response");
        }

        private string CreateRequestLog(HttpContext context)
        {
            var logMessage = new LogDetail
            {
                Severity = LogSeverity.Info,
                Host = context.Request.Host.Host,
                ClientId = context.Request.Scheme,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                Operation = LogOperation.ClientRequest,
                RequestId = context.Request.Headers["requestId"],
                ServiceId = "service_id",
                XForwardedFor = context.Request.Headers["X-Forwarded-For"],
                Timestamp = DateTime.Now,
                Message = "hello from lambda"
            };

            return JsonSerializer.Serialize(logMessage, this.serializerOptions);
        }
    }
}

