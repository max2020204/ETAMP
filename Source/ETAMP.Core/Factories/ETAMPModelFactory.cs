using ETAMP.Core.Interfaces;
using ETAMP.Core.Models;
using ETAMP.Core.Utils;

namespace ETAMP.Core.Factories;

/// <summary>
///     Factory class for creating instances of the <see cref="ETAMPModel{T}" />.
/// </summary>
/// <remarks>
///     This class generates ETAMP models, which encapsulate payload data, update type,
///     compression type, and other metadata specific to the ETAMP protocol.
/// </remarks>
public class ETAMPModelFactory : IETAMPBase
{
    /// <summary>
    ///     Represents the protocol version used within the ETAMPModelFactory.
    /// </summary>
    /// <remarks>
    ///     This variable is an instance of the <see cref="VersionInfo" /> class, encapsulating details
    ///     about the protocol's current version. It plays a critical role in ensuring that all models
    ///     created by the factory adhere to the same protocol version, maintaining consistency
    ///     across the communication framework.
    /// </remarks>
    private readonly VersionInfo _version;

    /// <summary>
    ///     A factory class for creating instances of the <see cref="ETAMPModel{T}" />.
    /// </summary>
    /// <remarks>
    ///     The ETAMPModelFactory provides a mechanism to create ETAMPModel instances with specific
    ///     properties like update type, token payload, and compression type. It also associates
    ///     these instances with a unique message ID and protocol version.
    /// </remarks>
    public ETAMPModelFactory(VersionInfo version)
    {
        _version = version;
    }

    /// <summary>
    ///     Creates an instance of the <see cref="ETAMPModel{T}" /> class with specified update type, payload, and compression
    ///     type.
    /// </summary>
    /// <typeparam name="T">The type of the token payload, constrained to inherit from <see cref="Token" />.</typeparam>
    /// <param name="updateType">The type of update being performed.</param>
    /// <param name="payload">The payload containing the token to be encapsulated within the model.</param>
    /// <param name="compressionType">The compression type to be applied to the model.</param>
    /// <returns>
    ///     An instance of <see cref="ETAMPModel{T}" /> populated with the provided parameters and additional default
    ///     values.
    /// </returns>
    public ETAMPModel<T> CreateETAMPModel<T>(string updateType, T payload, string compressionType) where T : Token
    {
        var messageId = Guid.CreateVersion7();
        payload.MessageId = messageId;
        return new ETAMPModel<T>
        {
            Id = messageId,
            Version = _version.ProtocolVersion,
            Token = payload,
            UpdateType = updateType,
            CompressionType = compressionType
        };
    }
}