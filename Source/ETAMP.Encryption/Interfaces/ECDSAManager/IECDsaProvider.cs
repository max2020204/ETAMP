using System.Security.Cryptography;

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

/// <summary>
///     Provides an interface for managing and accessing an ECDsa instance for cryptographic operations.
/// </summary>
public interface IECDsaProvider
{
    /// <summary>
    ///     Retrieves the ECDsa instance managed by the provider.
    /// </summary>
    /// <returns>The ECDsa instance for cryptographic operations.</returns>
    ECDsa? GetECDsa();
}