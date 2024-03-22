using ETAMPManagment.Encryption.Interfaces;
using System.Security.Cryptography;

namespace ETAMPManagment.Encryption
{
    /// <summary>
    /// Provides an implementation for managing Elliptic Curve Diffie-Hellman (ECDH) key pairs and facilitating cryptographic operations.
    /// </summary>
    public class KeyPairProvider : IKeyPairProvider
    {
        /// <summary>
        /// Gets the private key in PEM format.
        /// </summary>
        public string PrivateKey { get; }

        /// <summary>
        /// Gets the public key in PEM format.
        /// </summary>
        public string PublicKey { get; }

        /// <summary>
        /// Backing field for the ECDH algorithm implementation.
        /// </summary>
        private readonly ECDiffieHellman _eCDiffieHellman;

        /// <summary>
        /// Gets the public key component of the ECDH key pair.
        /// </summary>
        public ECDiffieHellmanPublicKey HellmanPublicKey => _eCDiffieHellman.PublicKey;

        /// <summary>
        /// Initializes a new instance of the KeyPairProvider class using the default ECDH algorithm parameters.
        /// </summary>
        public KeyPairProvider()
        {
            _eCDiffieHellman = ECDiffieHellman.Create();
            PrivateKey = _eCDiffieHellman.ExportECPrivateKeyPem();
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem();
        }

        /// <summary>
        /// Initializes a new instance of the KeyPairProvider class with a specific instance of ECDiffieHellman.
        /// </summary>
        /// <param name="ecDiffieHellman">An instance of ECDiffieHellman to be used for cryptographic operations.</param>
        public KeyPairProvider(ECDiffieHellman ecDiffieHellman)
        {
            _eCDiffieHellman = ecDiffieHellman;
            PrivateKey = _eCDiffieHellman.ExportECPrivateKeyPem();
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem();
        }

        /// <summary>
        /// Initializes a new instance of the KeyPairProvider class with specific EC parameters.
        /// </summary>
        /// <param name="parameters">The EC parameters to be used for the ECDH key pair generation.</param>
        public KeyPairProvider(ECParameters parameters)
        {
            _eCDiffieHellman = ECDiffieHellman.Create(parameters);
            PrivateKey = _eCDiffieHellman.ExportECPrivateKeyPem();
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem();
        }

        /// <summary>
        /// Initializes a new instance of the KeyPairProvider class with a specific ECCurve.
        /// </summary>
        /// <param name="curve">The curve to be used for ECDH key pair generation.</param>
        public KeyPairProvider(ECCurve curve)
        {
            _eCDiffieHellman = ECDiffieHellman.Create(curve);
            PrivateKey = _eCDiffieHellman.ExportPkcs8PrivateKeyPem();
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem();
        }

        /// <summary>
        /// Initializes a new instance of the KeyPairProvider class using a specified public key.
        /// </summary>
        /// <param name="publicKey">
        /// The public key in binary format to be used for the Elliptic Curve Diffie-Hellman (ECDH) key pair.
        /// This public key is used to initialize the ECDH algorithm instance within the provider.
        /// </param>
        /// <remarks>
        /// This constructor is particularly useful when you have a public key in binary format
        /// and want to create a KeyPairProvider instance that can be used for cryptographic operations
        /// with this public key. The ECDH algorithm instance is created and the provided public key
        /// is imported into it. The PublicKey property of the provider will then contain
        /// the PEM representation of the imported public key.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="publicKey"/> parameter is null.
        /// </exception>
        /// <exception cref="CryptographicException">
        /// Thrown if the <paramref name="publicKey"/> parameter is not a valid public key or
        /// if the public key cannot be imported into the ECDH algorithm instance.
        /// </exception>
        public KeyPairProvider(byte[] publicKey)
        {
            ArgumentNullException.ThrowIfNull(publicKey);

            _eCDiffieHellman = ECDiffieHellman.Create();
            _eCDiffieHellman.ImportSubjectPublicKeyInfo(publicKey, out _);
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem();
        }

        /// <summary>
        /// Provides access to the underlying ECDiffieHellman instance for cryptographic operations.
        /// </summary>
        /// <returns>The ECDiffieHellman instance used by this provider.</returns>
        public virtual ECDiffieHellman GetECDiffieHellman()
        {
            return _eCDiffieHellman;
        }

        /// <summary>
        /// Releases all resources used by the KeyPairProvider.
        /// </summary>
        public virtual void Dispose()
        {
            _eCDiffieHellman.Dispose();
        }
    }
}