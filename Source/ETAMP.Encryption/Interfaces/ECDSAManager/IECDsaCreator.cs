using System.Security.Cryptography;

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

/// <summary>
///     Defines methods for creating instances of ECDsa (Elliptic Curve Digital Signature Algorithm).
/// </summary>
public interface IECDsaCreator
{
    /// <summary>
    ///     Creates a new ECDsa instance using the default parameters.
    /// </summary>
    /// <returns>An IECDsaProvider instance containing the newly created ECDsa instance.</returns>
    IECDsaProvider CreateECDsa();

    /// <summary>
    ///     Creates a new ECDsa instance using a specified elliptic curve.
    /// </summary>
    /// <param name="curve">The ECCurve to use for the ECDsa instance.</param>
    /// <returns>An IECDsaProvider instance containing the newly created ECDsa instance.</returns>
    IECDsaProvider CreateECDsa(ECCurve curve);

    /// <summary>
    ///     Creates a new ECDsa instance using a specified public key and elliptic curve.
    /// </summary>
    /// <param name="publicKey">The public key in Base64 string format.</param>
    /// <param name="curve">The ECCurve associated with the public key.</param>
    /// <returns>An IECDsaProvider instance containing the newly created ECDsa instance.</returns>
    IECDsaProvider CreateECDsa(string publicKey, ECCurve curve);

    /// <summary>
    ///     Creates a new ECDsa instance using a specified public key and elliptic curve.
    /// </summary>
    /// <param name="publicKey">The public key as a byte array.</param>
    /// <param name="curve">The ECCurve associated with the public key.</param>
    /// <returns>An IECDsaProvider instance containing the newly created ECDsa instance.</returns>
    IECDsaProvider CreateECDsa(byte[] publicKey, ECCurve curve);
}