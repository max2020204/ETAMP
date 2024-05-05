#region

using System.Security.Cryptography;

#endregion

namespace ETAMPManagement.Encryption.ECDsaManager.Interfaces;

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