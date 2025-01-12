#region

using ETAMP.Core.Interfaces;
using ETAMP.Core.Models;
using ETAMP.Core.Utils;

#endregion

namespace ETAMP.Core;

/// <summary>
///     This class represents the base implementation of the ETAMP (Encrypted Token And Message Protocol) functionality.
/// </summary>
public sealed class ETAMPProtocol : IETAMPBase
{
    /// <summary>
    ///     Creates an ETAMP model for the given payload.
    /// </summary>
    /// <typeparam name="T">The type of the payload.</typeparam>
    /// <param name="updateType">The update type.</param>
    /// <param name="payload">The payload object.</param>
    /// <param name="compressionType">The compression type.</param>
    /// <returns>The ETAMP model.</returns>
    public ETAMPModel<T> CreateETAMPModel<T>(string updateType, T payload, string compressionType) where T : Token
    {
        var messageId = Guid.NewGuid();
        payload.MessageId = messageId;
        return new ETAMPModel<T>
        {
            Id = messageId,
            Version = VersionInfo.ProtocolVersion,
            Token = payload,
            UpdateType = updateType,
            CompressionType = compressionType
        };
    }
}