// ***********************************************************************
// <copyright file="HealthController.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Serverless.Api.Common.Constants;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Serverless.Api.Models.DataTranferObjects
{
    /// <summary>
    /// The service health data transfer object.
    /// </summary>
    public class HealthDto
    {
        /// <summary>
        /// Gets or sets the service health.
        /// </summary>
        /// <example>healthy</example>
        [DisplayName(DisplayNames.ServiceHealth)]
        [JsonPropertyName(DisplayNames.ServiceHealth)]
        public string? ServiceHealth { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        [DisplayName(DisplayNames.Timestamp)]
        [JsonPropertyName(DisplayNames.Timestamp)]
        public DateTime? Timestamp { get; set; }
    }
}
