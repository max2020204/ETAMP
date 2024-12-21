using System.Security.Cryptography;

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

/// <summary>
///     Provides methods for importing private keys into ECDsa instances, managing the underlying cryptographic keys.
/// </summary>
public interface IECDsaKeyManager
{
    /// <summary>
    ///     Imports a private key with a specified elliptic curve into an ECDsa instance.
    /// </summary>
    /// <param name="privateKey">The private key as a byte array.</param>
    /// <param name="curve">The ECCurve to use with the private key.</param>
    /// <returns>An IECDsaProvider instance containing the ECDsa instance initialized with the given private key and curve.</returns>
    IECDsaProvider ImportECDsa(byte[] privateKey, ECCurve curve);

    /// <summary>
    ///     Imports a private key into an ECDsa instance using the default elliptic curve.
    /// </summary>
    /// <param name="privateKey">The private key as a byte array.</param>
    /// <returns>An IECDsaProvider instance containing the ECDsa instance initialized with the given private key.</returns>
    IECDsaProvider ImportECDsa(byte[] privateKey);

    /// <summary>
    ///     Imports a private key with a specified elliptic curve into an ECDsa instance.
    /// </summary>
    /// <param name="privateKey">The private key in Base64 string format.</param>
    /// <param name="curve">The ECCurve to use with the private key.</param>
    /// <returns>An IECDsaProvider instance containing the ECDsa instance initialized with the given private key and curve.</returns>
    IECDsaProvider ImportECDsa(string privateKey, ECCurve curve);

    /// <summary>
    ///     Imports a private key into an ECDsa instance using the default elliptic curve.
    /// </summary>
    /// <param name="privateKey">The private key in Base64 string format.</param>
    /// <returns>An IECDsaProvider instance containing the ECDsa instance initialized with the given private key.</returns>
    IECDsaProvider ImportECDsa(string privateKey);
}