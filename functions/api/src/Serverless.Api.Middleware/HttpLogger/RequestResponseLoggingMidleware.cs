using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace Serverless.Api.Middleware.HttpLogger
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate requestProcess;
        private readonly ILogger logger;


        public RequestResponseLoggingMiddleware(RequestDelegate requestProcess, ILoggerFactory loggerFactory)
        {
            this.requestProcess = requestProcess;
            this.logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }


        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }


        private async Task LogRequest(HttpContext context)
        {
            this.logger.LogInformation(context.Request.Host.Host);
        }

        private async Task LogResponse(HttpContext context)
        {
            this.logger.LogInformation("response");
        }
    }
}

