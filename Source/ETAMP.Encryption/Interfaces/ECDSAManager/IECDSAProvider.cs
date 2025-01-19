#region

using System.Security.Cryptography;

#endregion

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

/// <summary>
///     Represents the interface for an Elliptic Curve Digital Signature Algorithm (ECDSA) provider.
/// </summary>
/// <remarks>
///     This provider is responsible for retrieving ECDsa instances based on
///     unique identifiers or names. It serves as an abstraction for underlying
///     components managing ECDSA operations.
/// </remarks>
public interface IECDSAProvider
{
    /// <summary>
    ///     Retrieves the ECDsa instance associated with the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the desired ECDsa cryptographic instance.</param>
    /// <returns>The ECDsa instance corresponding to the provided unique identifier, or null if not found.</returns>
    ECDsa GetECDsa(Guid id);

    /// <summary>
    ///     Retrieves an ECDsa instance based on the specified name.
    /// </summary>
    /// <param name="name">The name identifier used to locate the ECDsa instance.</param>
    /// <returns>The ECDsa instance associated with the specified name, or null if not found.</returns>
    ECDsa GetECDsa(string name);
}