using ETAMP.Core.Interfaces;
using ETAMP.Core.Models;

namespace ETAMP.Core;

/// <summary>
///     This class represents the base implementation of the ETAMP (Encrypted Token And Message Protocol) functionality.
/// </summary>
public sealed class ETAMPBase : IETAMPBase
{
    /// <summary>
    ///     Represents version information for the application.
    /// </summary>
    private readonly VersionInfo _versionInfo;

    /// <summary>
    ///     This class represents the base implementation of the ETAMP (Encrypted Token And Message Protocol) functionality.
    /// </summary>
    public ETAMPBase(VersionInfo versionInfo)
    {
        _versionInfo = versionInfo ?? throw new ArgumentNullException(nameof(versionInfo));
    }

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
            Version = _versionInfo.ProtocolVersion,
            Token = payload,
            UpdateType = updateType,
            CompressionType = compressionType
        };
    }
}