// ***********************************************************************
// <copyright file="LogDetail.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Serverless.Api.Common.Constants;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Serverless.Api.Models.DataTranferObjects.Logger
{
    /// <summary>
    /// The service logger detail object.
    /// </summary>
    /// <typeparam name="TMessage">The generic message type.</typeparam>
    public class LogDetail<TMessage>
    {
        /// <summary>
        /// Gets or sets the log severity.
        /// </summary>
        /// <example>info</example>
        [DisplayName(DisplayNames.Severity)]
        [JsonPropertyName(DisplayNames.Severity)]
        public LogSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        [DisplayName(DisplayNames.Timestamp)]
        [JsonPropertyName(DisplayNames.Timestamp)]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <example>Development</example>
        [DisplayName(DisplayNames.Environment)]
        [JsonPropertyName(DisplayNames.Environment)]
        public string? Environment { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        [DisplayName(DisplayNames.Target)]
        [JsonPropertyName(DisplayNames.Target)]
        public LogTarget? Target { get; set; }

        /// <summary>
        /// Gets or sets the x-forwarded-for header.
        /// </summary>
        /// <example>localhost</example>
        [DisplayName(DisplayNames.XForwardedFor)]
        [JsonPropertyName(DisplayNames.XForwardedFor)]
        public string? XForwardedFor { get; set; }

        /// <summary>
        /// Gets or sets the service identifier. 
        /// </summary>
        /// <example><see cref="Guid.NewGuid()"/></example>
        [DisplayName(DisplayNames.ServiceId)]
        [JsonPropertyName(DisplayNames.ServiceId)]
        public string? ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        /// <example><see cref="Guid.NewGuid()"/></example>
        [DisplayName(DisplayNames.RequestId)]
        [JsonPropertyName(DisplayNames.RequestId)]
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <example><see cref="Guid.NewGuid()"/></example>
        [DisplayName(DisplayNames.ClientId)]
        [JsonPropertyName(DisplayNames.ClientId)]
        public string? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <example>clientRequest</example>
        [DisplayName(DisplayNames.Operation)]
        [JsonPropertyName(DisplayNames.Operation)]
        public LogOperation Operation { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <example>A custom log message.</example>
        [DisplayName(DisplayNames.Message)]
        [JsonPropertyName(DisplayNames.Message)]
        public TMessage? Message { get; set; }
    }
}
