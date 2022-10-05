// ***********************************************************************
// <copyright file="ErrorDetailsDto.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Serverless.Api.Common.Constants;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Serverless.Api.Models.DataTranferObjects.Error
{
    /// <summary>
    /// The service error details transfer object.
    /// </summary>
    public class ErrorDetailsDto
    {
        /// <summary>
        /// Gets or sets the service error code.
        /// </summary>
        /// <example>healthy</example>
        [DisplayName(DisplayNames.ErrorCode)]
        [JsonPropertyName(DisplayNames.ErrorCode)]
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the service error message.
        /// </summary>
        /// <example>This is an example message.</example>
        [DisplayName(DisplayNames.Message)]
        [JsonPropertyName(DisplayNames.Message)]
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the service target.
        /// </summary>
        /// <example>healthy</example>
        [DisplayName(DisplayNames.Target)]
        [JsonPropertyName(DisplayNames.Target)]
        public string? Target { get; set; }
    }
}


