// ***********************************************************************
// <copyright file="LogTarget.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Serverless.Api.Common.Constants;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Serverless.Api.Models.Models.Logger
{
    /// <summary>
    /// The service logger target object.
    /// </summary>
    public class LogTarget
    {
        /// <summary>
        /// Gets or sets the log target method.
        /// </summary>
        /// <example>GET</example>
        [DisplayName(DisplayNames.Method)]
        [JsonPropertyName(DisplayNames.Method)]
        public string? Method { get; set; }

        /// <summary>
        /// Gets or sets the log target host.
        /// </summary>
        /// <example>GET</example>
        [DisplayName(DisplayNames.Host)]
        [JsonPropertyName(DisplayNames.Host)]
        public string? Host { get; set;  }

        /// <summary>
        /// Gets or sets the log target route.
        /// </summary>
        /// <example>GET</example>
        [DisplayName(DisplayNames.Route)]
        [JsonPropertyName(DisplayNames.Route)]
        public string? Route { get; set; }
    }
}
