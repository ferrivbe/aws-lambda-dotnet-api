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
    /// <summary>
    /// Handles HTTP logging.
    /// </summary>
    public class HttpLoggingMiddleware
    {
        private readonly ILogger<HttpLoggingMiddleware> logger;
        private readonly JsonSerializerOptions serializerOptions;
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

            this.serializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = {
                    new JsonStringEnumConverter( JsonNamingPolicy.CamelCase),
                },
            };
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
        /// Logs an error message.
        /// </summary>
        /// <typeparam name="TMessage">The generic message parameter.</typeparam>
        /// <param name="context">The HTTP context.</param>
        /// <param name="message">The message.</param>
        public void LogError<TMessage>(HttpContext context, TMessage message)
        {
            var logMessage = CreateErrorLog(context, message);
            this.logger.LogError(logMessage);
        }

        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <typeparam name="TMessage">The generic message parameter.</typeparam>
        /// <param name="context">The HTTP context.</param>
        /// <param name="message">The message.</param>
        public void LogInfo<TMessage>(HttpContext context, TMessage message)
        {
            var logMessage = CreateInfoLog(context, message);
            this.logger.LogError(logMessage);
        }

        /// <summary>
        /// Reads the stream.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <returns>A <see cref="string?"/> representig the request body.</returns>
        private static string? ReadStream(Stream stream)
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

            return stringItem != string.Empty ? stringItem : null;
        }

        /// <summary>
        /// Logs an incoming HTTP request.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = this.recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            var logMessage = CreateRequestLog(context, ReadStream(requestStream));
            this.logger.LogInformation(logMessage);

            context.Request.Body.Position = 0;
        }

        /// <summary>
        /// Logs an outcome HTTP response.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = this.recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await requestProcess(context);
            
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var logMessage = CreateResponseLog(context, text);
            this.logger.LogInformation(logMessage);

            await responseBody.CopyToAsync(originalBodyStream);
        }

        /// <summary>
        /// Creates an error log.
        /// </summary>
        /// <typeparam name="TMessage">The generic message.</typeparam>
        /// <param name="context">The HTTP context.</param>
        /// <param name="message">The error message.</param>
        /// <returns>A <see cref="string"/> representig the error log.</returns>
        private string CreateErrorLog<TMessage>(HttpContext context, TMessage message)
            => CreateLog(context, LogSeverity.Error, LogOperation.Error, message);

        /// <summary>
        /// Creates an info log.
        /// </summary>
        /// <typeparam name="TMessage">The generic message.</typeparam>
        /// <param name="context">The HTTP context.</param>
        /// <param name="message">The info message.</param>
        /// <returns>A <see cref="string"/> representig the info log.</returns>
        private string CreateInfoLog<TMessage>(HttpContext context, TMessage message)
            => CreateLog(context, LogSeverity.Info, LogOperation.Info, message);

        /// <summary>
        /// Creates a request log.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="requestBody">The request body.</param>
        /// <returns>A <see cref="string"/> representig the request log.</returns>
        private string CreateRequestLog(HttpContext context, string? requestBody)
            => CreateLog(context, LogSeverity.Info, LogOperation.ClientRequest, requestBody);

        /// <summary>
        /// Creates a response log.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="responseBody">The response body.</param>
        /// <returns>A <see cref="string"/> representig the response log.</returns>
        private string CreateResponseLog(HttpContext context, string? responseBody)
            => CreateLog(context, LogSeverity.Info, LogOperation.ClientResponse, responseBody);

        /// <summary>
        /// Creates a generic log.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="severity">The log severity.</param>
        /// <param name="operation">The log operation.</param>
        /// <param name="message">The log message</param>
        /// <returns>A <see cref="string"/> representing a generic log.</returns>
        private string CreateLog<TMessage>(
            HttpContext context,
            LogSeverity severity,
            LogOperation operation,
            TMessage message)
        {
            var log = new LogDetail<TMessage>
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
                XForwardedFor = context.Request.Headers[DisplayNames.XForwardedForHeader],
                Timestamp = DateTime.Now,
                Message = message,
            };

            var serializedLog = JsonSerializer.Serialize(log, this.serializerOptions);

            return serializedLog
                .Replace("\\u0022", "\"")
                .Replace("\"{", "{")
                .Replace("}\"", "}");
        }
    }
}

