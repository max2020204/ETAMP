#region

using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.ECDsaManager;

/// <summary>
///     Facilitates the creation of ECDsa instances and registers them for later use.
/// </summary>
public class ECDsaCreator : IECDsaCreator
{
    private readonly IECDsaRegistrar _ecdsaRegistrar;

    /// <summary>
    ///     Initializes a new instance of the ECDsaECDsaCreator class with a registrar for ECDsa instances.
    /// </summary>
    /// <param name="ecdsaRegistrar">The registrar used to register ECDsa instances.</param>
    public ECDsaCreator(IECDsaRegistrar ecdsaRegistrar)
    {
        _ecdsaRegistrar = ecdsaRegistrar ??
                          throw new ArgumentNullException(nameof(ecdsaRegistrar), "ECDsa registrar cannot be null.");
    }

    /// <summary>
    ///     Creates and registers a new ECDsa instance using default parameters.
    /// </summary>
    /// <returns>A provider for the created and registered ECDsa instance.</returns>
    public IECDsaProvider CreateECDsa()
    {
        var ecdsa = ECDsa.Create();
        return _ecdsaRegistrar.RegisterECDsa(ecdsa);
    }

    /// <summary>
    ///     Creates and registers a new ECDsa instance using the specified elliptic curve.
    /// </summary>
    /// <param name="curve">The elliptic curve to use for the ECDsa instance.</param>
    /// <returns>A provider for the created and registered ECDsa instance.</returns>
    public IECDsaProvider CreateECDsa(ECCurve curve)
    {
        var ecdsa = ECDsa.Create(curve);
        return _ecdsaRegistrar.RegisterECDsa(ecdsa);
    }

    /// <summary>
    ///     Creates and registers a new ECDsa instance using a public key and the specified elliptic curve.
    /// </summary>
    /// <param name="publicKey">The public key in Base64-encoded string format.</param>
    /// <param name="curve">The elliptic curve to use for the ECDsa instance.</param>
    /// <returns>A provider for the created and registered ECDsa instance.</returns>
    public IECDsaProvider CreateECDsa(string publicKey, ECCurve curve)
    {
        return CreateAndRegisterECDsa(Convert.FromBase64String(publicKey), curve);
    }

    /// <summary>
    ///     Creates and registers a new ECDsa instance using a public key byte array and the specified elliptic curve.
    /// </summary>
    /// <param name="publicKey">The public key as a byte array.</param>
    /// <param name="curve">The elliptic curve to use for the ECDsa instance.</param>
    /// <returns>A provider for the created and registered ECDsa instance.</returns>
    public IECDsaProvider CreateECDsa(byte[] publicKey, ECCurve curve)
    {
        return CreateAndRegisterECDsa(publicKey, curve);
    }

    private IECDsaProvider CreateAndRegisterECDsa(byte[] publicKey, ECCurve curve)
    {
        var ecdsa = ECDsa.Create(curve);
        ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
        return _ecdsaRegistrar.RegisterECDsa(ecdsa);
    }
}