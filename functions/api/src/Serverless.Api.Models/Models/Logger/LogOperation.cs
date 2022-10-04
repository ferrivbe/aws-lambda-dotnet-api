// ***********************************************************************
// <copyright file="LogOperation.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Serverless.Api.Common.Constants;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Serverless.Api.Models.Models.Logger
{
    /// <summary>
    /// Represents all possible values for log operation.
    /// </summary>
    [DataContract]
    public enum LogOperation
    {
        /// <summary>
        /// The client request log operation type.
        /// </summary>
        [EnumMember(Value = DisplayNames.ClientRequest)]
        [JsonPropertyName(DisplayNames.ClientRequest)]
        ClientRequest = 0,

        /// <summary>
        /// The client response log operation type.
        /// </summary>
        [EnumMember(Value = DisplayNames.ClientResponse)]
        ClientResponse = 1,

        /// <summary>
        /// The external request log operation type.
        /// </summary>
        [EnumMember(Value = DisplayNames.ExternalRequest)]
        ExternalRequest = 2,

        /// <summary>
        /// The external response log operation type.
        /// </summary>
        [EnumMember(Value = DisplayNames.ExternalResponse)]
        ExternalResponse = 3,
    }
}
