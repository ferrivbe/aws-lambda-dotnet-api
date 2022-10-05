// ***********************************************************************
// <copyright file="HttpException.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


namespace Serverless.Api.Models.Extensions
{
    /// <summary>
    /// The service HTTP exception.
    /// Extends the <see cref="Exception"/> class.
    /// </summary>
    public class HttpException: Exception
    {
        /// <summary>
        /// Gets or sets the exception error code.
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the exception status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="HttpException"/>.
        /// </summary>
        /// <param name="statusCode">The esception status code.</param>
        /// <param name="errorCode">The esception error code.</param>
        /// <param name="message">The esception message.</param>
        public HttpException(
            int statusCode,
            string errorCode,
            string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
            this.StatusCode = statusCode;
        }
    }
}
