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
    /// <typeparam name="TDetail">The generic detail.</typeparam>
    public class HttpErrorDto<TDetail>
    {
        public ErrorDetailsDto<TDetail>? Error { get; set; }
    }
}

