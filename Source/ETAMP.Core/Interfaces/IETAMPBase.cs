using ETAMP.Core.Models;

namespace ETAMP.Core.Interfaces;

/// <summary>
///     Represents the interface for the ETAMPProtocol class.
/// </summary>
public interface IETAMPBase
{
    /// <summary>
    ///     Creates an instance of ETAMPModel.
    /// </summary>
    /// <typeparam name="T">The type parameter specifying the payload type.</typeparam>
    /// <param name="updateType">The update type.</param>
    /// <param name="payload">The payload object.</param>
    /// <param name="compressionType">The compression type.</param>
    /// <returns>An instance of ETAMPModel.</returns>
    ETAMPModel<T> CreateETAMPModel<T>(string updateType, T payload, string compressionType) where T : Token;
}