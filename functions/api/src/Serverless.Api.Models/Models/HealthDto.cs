﻿// ***********************************************************************
// <copyright file="HealthController.cs">
//     Serverless example
// </copyright>
// ***********************************************************************

using Serverless.Api.Common.Constants;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Serverless.Api.Models.Models
{
    /// <summary>
    /// The service health data transfer object.
    /// </summary>
    public class HealthDto
    {
        /// <summary>
        /// Gets or sets the service health.
        /// </summary>
        [DisplayName(DisplayNames.ServiceHealth)]
        [JsonPropertyName(DisplayNames.ServiceHealth)]
        public string ServiceHealth { get; set; }
    }
}
