// ***********************************************************************
// <copyright file="HttpErrorDto.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Serverless.Api.Common.Constants;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Serverless.Api.Models.DataTranferObjects.Error
{
    /// <summary>
    /// The service HTTP error data transfer object.
    /// </summary>
    public class HttpErrorDto
    {
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        [DisplayName(DisplayNames.Error)]
        [JsonPropertyName(DisplayNames.Error)]
        public ErrorDetailsDto Error { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="HttpErrorDto{TMessage}"/>
        /// with custom parameters.
        /// </summary>
        /// <param name="message">The HTTP error message.</param>
        /// <param name="errorCode">The HTTP error code.</param>
        /// <param name="target">The HTTP error target.</param>
        public HttpErrorDto(
            string? message,
            string? errorCode,
            string? target)
        {
            this.Error = new ErrorDetailsDto
            {
                ErrorCode = errorCode,
                Message = message,
                Target = target,
            };
        }
    }
}

