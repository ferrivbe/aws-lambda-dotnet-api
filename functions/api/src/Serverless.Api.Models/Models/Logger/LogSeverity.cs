// ***********************************************************************
// <copyright file="LogSeverity.cs">
//     Serverless example
// </copyright>
// ***********************************************************************


using Serverless.Api.Common.Constants;
using System.Runtime.Serialization;

namespace Serverless.Api.Models.Models.Logger
{
    /// <summary>
    /// Represents all possible values for log severity.
    /// </summary>
    [DataContract]
    public enum LogSeverity
    {
        /// <summary>
        /// The information severity level.
        /// </summary>
        [EnumMember(Value = DisplayNames.Information)]
        Info = 0,

        /// <summary>
        /// The warning severity level.
        /// </summary>
        [EnumMember(Value = DisplayNames.Warning)]
        Warn = 1,

        /// <summary>
        /// The error severity level.
        /// </summary>
        [EnumMember(Value = DisplayNames.Error)]
        Error = 2,

        /// <summary>
        /// The fatal severity level.
        /// </summary>
        [EnumMember(Value = DisplayNames.Fatal)]
        Fatal = 3,
    }
}
