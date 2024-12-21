using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces.ECDSAManager;

namespace ETAMP.Encryption.ECDsaManager;

/// <summary>
///     Manages the importation of ECDsa keys and registers them for cryptographic operations.
/// </summary>
public class ECDsaKeyManager : IECDsaKeyManager
{
    private readonly IECDsaRegistrar _ecdsaRegistrar;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ECDsaKeyManager" /> class with the specified registrar.
    /// </summary>
    /// <param name="ecdsaRegistrar">The registrar to use for registering the imported ECDsa keys.</param>
    public ECDsaKeyManager(IECDsaRegistrar ecdsaRegistrar)
    {
        _ecdsaRegistrar = ecdsaRegistrar ??
                          throw new ArgumentNullException(nameof(ecdsaRegistrar), "ECDsa registrar cannot be null.");
    }

    /// <summary>
    ///     Imports an ECDsa private key from a byte array and registers it for use, specifying the elliptic curve.
    /// </summary>
    /// <param name="privateKey">The private key as a byte array.</param>
    /// <param name="curve">The elliptic curve to use for the ECDsa instance.</param>
    /// <returns>A provider for the imported and registered ECDsa instance.</returns>
    public IECDsaProvider ImportECDsa(byte[] privateKey, ECCurve curve)
    {
        var ecdsa = ECDsa.Create(curve);
        ecdsa.ImportPkcs8PrivateKey(privateKey, out _);
        return _ecdsaRegistrar.RegisterECDsa(ecdsa);
    }

    /// <summary>
    ///     Imports an ECDsa private key from a byte array and registers it for use, using the default elliptic curve.
    /// </summary>
    /// <param name="privateKey">The private key as a byte array.</param>
    /// <returns>A provider for the imported and registered ECDsa instance.</returns>
    public IECDsaProvider ImportECDsa(byte[] privateKey)
    {
        var ecdsa = ECDsa.Create();
        ecdsa.ImportPkcs8PrivateKey(privateKey, out _);
        return _ecdsaRegistrar.RegisterECDsa(ecdsa);
    }

    /// <summary>
    ///     Imports an ECDsa private key from a Base64-encoded string and registers it for use, specifying the elliptic curve.
    /// </summary>
    /// <param name="privateKey">The private key in Base64-encoded string format.</param>
    /// <param name="curve">The elliptic curve to use for the ECDsa instance.</param>
    /// <returns>A provider for the imported and registered ECDsa instance.</returns>
    public IECDsaProvider ImportECDsa(string privateKey, ECCurve curve)
    {
        var ecdsa = ECDsa.Create(curve);
        ecdsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
        return _ecdsaRegistrar.RegisterECDsa(ecdsa);
    }

    /// <summary>
    ///     Imports an ECDsa private key from a Base64-encoded string and registers it for use, using the default elliptic
    ///     curve.
    /// </summary>
    /// <param name="privateKey">The private key in Base64-encoded string format.</param>
    /// <returns>A provider for the imported and registered ECDsa instance.</returns>
    public IECDsaProvider ImportECDsa(string privateKey)
    {
        var ecdsa = ECDsa.Create();
        ecdsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
        return _ecdsaRegistrar.RegisterECDsa(ecdsa);
    }
}