using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using System.Security.Cryptography;

namespace ETAMPManagment.Encryption.ECDsaManager
{
    /// <summary>
    /// Manages the importation of ECDSA keys and registers them for cryptographic operations.
    /// </summary>
    public class EcdsaKeyManager : IEcdsaKeyManager
    {
        private readonly IECDsaRegistrar _ecdsaRegistrar;

        /// <summary>
        /// Initializes a new instance of the <see cref="EcdsaKeyManager"/> class with the specified registrar.
        /// </summary>
        /// <param name="ecdsaRegistrar">The registrar to use for registering the imported ECDSA keys.</param>
        public EcdsaKeyManager(IECDsaRegistrar ecdsaRegistrar)
        {
            _ecdsaRegistrar = ecdsaRegistrar ??
                throw new ArgumentNullException(nameof(ecdsaRegistrar), "ECDSA registrar cannot be null.");
        }

        /// <summary>
        /// Imports an ECDSA private key from a byte array and registers it for use, specifying the elliptic curve.
        /// </summary>
        /// <param name="privateKey">The private key as a byte array.</param>
        /// <param name="curve">The elliptic curve to use for the ECDSA instance.</param>
        /// <returns>A provider for the imported and registered ECDSA instance.</returns>
        public IECDsaProvider ImportECDsa(byte[] privateKey, ECCurve curve)
        {
            ECDsa ecdsa = ECDsa.Create(curve);
            ecdsa.ImportPkcs8PrivateKey(privateKey, out _);
            return _ecdsaRegistrar.RegisterEcdsa(ecdsa);
        }

        /// <summary>
        /// Imports an ECDSA private key from a byte array and registers it for use, using the default elliptic curve.
        /// </summary>
        /// <param name="privateKey">The private key as a byte array.</param>
        /// <returns>A provider for the imported and registered ECDSA instance.</returns>
        public IECDsaProvider ImportECDsa(byte[] privateKey)
        {
            ECDsa ecdsa = ECDsa.Create();
            ecdsa.ImportPkcs8PrivateKey(privateKey, out _);
            return _ecdsaRegistrar.RegisterEcdsa(ecdsa);
        }

        /// <summary>
        /// Imports an ECDSA private key from a Base64-encoded string and registers it for use, specifying the elliptic curve.
        /// </summary>
        /// <param name="privateKey">The private key in Base64-encoded string format.</param>
        /// <param name="curve">The elliptic curve to use for the ECDSA instance.</param>
        /// <returns>A provider for the imported and registered ECDSA instance.</returns>
        public IECDsaProvider ImportECDsa(string privateKey, ECCurve curve)
        {
            ECDsa ecdsa = ECDsa.Create(curve);
            ecdsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
            return _ecdsaRegistrar.RegisterEcdsa(ecdsa);
        }

        /// <summary>
        /// Imports an ECDSA private key from a Base64-encoded string and registers it for use, using the default elliptic curve.
        /// </summary>
        /// <param name="privateKey">The private key in Base64-encoded string format.</param>
        /// <returns>A provider for the imported and registered ECDSA instance.</returns>
        public IECDsaProvider ImportECDsa(string privateKey)
        {
            ECDsa ecdsa = ECDsa.Create();
            ecdsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
            return _ecdsaRegistrar.RegisterEcdsa(ecdsa);
        }
    }
}