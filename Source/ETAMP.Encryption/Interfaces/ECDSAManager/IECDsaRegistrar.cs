using System.Security.Cryptography;

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

/// <summary>
///     Provides a public interface for registering ECDsa instances within a cryptographic provider.
///     Allows for customized management of ECDsa instances to accommodate specific security needs.
/// </summary>
public interface IECDsaRegistrar
{
    /// <summary>
    ///     Registers an ECDsa instance with the cryptographic provider and returns an associated provider.
    ///     Enables dynamic management of cryptographic operations tailored to specific requirements.
    /// </summary>
    /// <param name="ecdsa">The ECDsa instance to register, already configured for use.</param>
    /// <returns>An IECDsaProvider that manages the registered ECDsa, facilitating access to its cryptographic functionalities.</returns>
    IECDsaProvider RegisterECDsa(ECDsa ecdsa);
}