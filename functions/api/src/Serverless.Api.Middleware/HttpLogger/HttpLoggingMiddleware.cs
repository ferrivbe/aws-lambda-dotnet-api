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
        private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;

        public HttpLoggingMiddleware(RequestDelegate requestProcess, ILogger<HttpLoggingMiddleware> logger, ServiceSettings settings)
        {
            this.logger = logger;
            this.requestProcess = requestProcess;
            this.settings = settings;
            this.recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();

            this.serializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = {
                    new JsonStringEnumConverter( JsonNamingPolicy.CamelCase),
                },
                WriteIndented = true,
            };
        }


        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }


        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = this.recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            var logMessage = CreateRequestLog(context, ReadStreamInChunks(requestStream));
            this.logger.LogInformation(logMessage);

            context.Request.Body.Position = 0;
        }

        private static object? ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            var stringItem = textWriter.ToString();

            return stringItem;
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = this.recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await requestProcess(context);
            
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            await requestProcess(context);

            var logMessage = CreateResponseLog(context, text);
            this.logger.LogInformation(logMessage);

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private string CreateRequestLog(HttpContext context, object? requestBody)
            => CreateLog(context, LogSeverity.Info, LogOperation.ClientRequest, requestBody);

        private string CreateResponseLog(HttpContext context, object? responseBody)
            => CreateLog(context, LogSeverity.Info, LogOperation.ClientResponse, responseBody);

        private string CreateLog(
            HttpContext context,
            LogSeverity severity,
            LogOperation operation,
            object? message)
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
                RequestId = context.Request.Headers[DisplayNames.RequestId],
                ServiceId = this.settings.ServiceId,
                XForwardedFor = context.Request.Headers["X-Forwarded-For"],
                Timestamp = DateTime.Now,
                Message = message
            };

            return JsonSerializer.Serialize(log, this.serializerOptions);
        }
    }
}

