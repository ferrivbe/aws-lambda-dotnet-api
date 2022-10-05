using System;
using Microsoft.AspNetCore.Http;
using Serverless.Api.Common.Constants;
using Serverless.Api.Models.DataTranferObjects.Logger;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace Serverless.Api.Middleware.HttpLogger
{
    /// <summary>
    /// Handles all logging scenarios.
    /// </summary>
    public static class HttpLogger
    {
        /// <summary>
        /// The JSON serializer options for logging methods.
        /// </summary>
        public static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {
                new JsonStringEnumConverter( JsonNamingPolicy.CamelCase),
            },
        };

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <typeparam name="TMessage">The generic message parameter.</typeparam>
        /// <param name="context">The HTTP context.</param>
        /// <param name="message">The message.</param>
        /// <param name="serviceId">The service identifier.</param>
        public static void LogError<TMessage>(this ILogger logger, HttpContext context, TMessage message, string? serviceId)
        {
            var logMessage = CreateErrorLog(context, message, serviceId);
            logger.LogError(logMessage);
        }

        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <typeparam name="TMessage">The generic message parameter.</typeparam>
        /// <param name="context">The HTTP context.</param>
        /// <param name="message">The message.</param>
        /// <param name="serviceId">The service identifier.</param>
        public static void LogInfo<TMessage>(this ILogger logger, HttpContext context, TMessage message, string? serviceId)
        {
            var logMessage = CreateInfoLog(context, message, serviceId);
            logger.LogInformation(logMessage);
        }

        /// <summary>
        /// Logs an incoming HTTP request.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="context">The HTTP context.</param>
        /// <param name="recyclableMemoryStreamManager">The recyclable mmory stream manager.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task LogRequest(
            this ILogger logger,
            HttpContext context,
            RecyclableMemoryStreamManager recyclableMemoryStreamManager,
            string? serviceId)
        {
            context.Request.EnableBuffering();
            await using var requestStream = recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            var logMessage = CreateRequestLog(
                context,
                ReadStream(requestStream),
                serviceId);
            logger.LogInformation(logMessage);

            context.Request.Body.Position = 0;
        }

        /// <summary>
        /// Logs an outcome HTTP response.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="context">The HTTP context.</param>
        /// <param name="recyclableMemoryStreamManager">The recyclable mmory stream manager.</param>
        /// <param name="requestProcess">The request delegate process.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task LogResponse(
            this ILogger logger,
            HttpContext context,
            RecyclableMemoryStreamManager recyclableMemoryStreamManager,
            RequestDelegate requestProcess,
            string? serviceId)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await requestProcess(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var logMessage = CreateResponseLog(context, text, serviceId);
            logger.LogInformation(logMessage);

            await responseBody.CopyToAsync(originalBodyStream);
        }

        /// <summary>
        /// Creates an error log.
        /// </summary>
        /// <typeparam name="TMessage">The generic message.</typeparam>
        /// <param name="context">The HTTP context.</param>
        /// <param name="message">The error message.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns>A <see cref="string"/> representig the error log.</returns>
        private static string CreateErrorLog<TMessage>(HttpContext context, TMessage message, string? serviceId)
            => CreateLog(context, LogSeverity.Error, LogOperation.Error, message, serviceId);

        /// <summary>
        /// Creates an info log.
        /// </summary>
        /// <typeparam name="TMessage">The generic message.</typeparam>
        /// <param name="context">The HTTP context.</param>
        /// <param name="message">The info message.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns>A <see cref="string"/> representig the info log.</returns>
        private static string CreateInfoLog<TMessage>(HttpContext context, TMessage message, string? serviceId)
            => CreateLog(context, LogSeverity.Info, LogOperation.Info, message, serviceId);

        /// <summary>
        /// Creates a request log.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns>A <see cref="string"/> representig the request log.</returns>
        private static string CreateRequestLog(HttpContext context, string? requestBody, string? serviceId)
            => CreateLog(context, LogSeverity.Info, LogOperation.ClientRequest, requestBody, serviceId);

        /// <summary>
        /// Creates a response log.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="responseBody">The response body.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns>A <see cref="string"/> representig the response log.</returns>
        private static string CreateResponseLog(HttpContext context, string? responseBody, string? serviceId)
            => CreateLog(context, LogSeverity.Info, LogOperation.ClientResponse, responseBody, serviceId);

        /// <summary>
        /// Creates a generic log.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="severity">The log severity.</param>
        /// <param name="operation">The log operation.</param>
        /// <param name="message">The log message</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns>A <see cref="string"/> representing a generic log.</returns>
        private static string CreateLog<TMessage>(
            HttpContext context,
            LogSeverity severity,
            LogOperation operation,
            TMessage message,
            string? serviceId)
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
                ServiceId = serviceId,
                XForwardedFor = context.Request.Headers[DisplayNames.XForwardedForHeader],
                Timestamp = DateTime.Now,
                Message = message,
            };

            var serializedLog = JsonSerializer.Serialize(log, serializerOptions);

            return serializedLog
                .Replace("\\u0022", "\"")
                .Replace("\"{", "{")
                .Replace("}\"", "}");
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
    }
}

