// ***********************************************************************
// <copyright file="HttpErrorDto.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


namespace Serverless.Api.Models.Models.Error
{
    /// <summary>
    /// The service HTTP error data transfer object.
    /// </summary>
    public class HttpErrorDto
    {
        public ErrorDetailsDto? Error { get; set; }
    }
}

