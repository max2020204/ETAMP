using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Models;
using System.Security.Cryptography;

namespace ETAMPManagment.Encryption
{
    /// <summary>
    /// Provides an implementation for managing Elliptic Curve Diffie-Hellman (ECDH) key pairs and facilitating cryptographic operations.
    /// </summary>
    public class KeyPairProvider : IKeyPairProvider
    {
        /// <summary>
        /// Provides access to the model provider which contains the public and private keys.
        /// </summary>
        public ECDKeyModelProvider KeyModelProvider { get; }

        private readonly ECDiffieHellman _eCDiffieHellman;

        /// <summary>
        /// Gets the public key component of the ECDH key pair, allowing for public key exchange operations.
        /// </summary>
        public ECDiffieHellmanPublicKey HellmanPublicKey => _eCDiffieHellman.PublicKey;

        /// <summary>
        /// Initializes a new instance of the KeyPairProvider class using the default ECDH algorithm parameters,
        /// automatically generating a new public/private key pair.
        /// </summary>
        public KeyPairProvider()
        {
            _eCDiffieHellman = ECDiffieHellman.Create();
            KeyModelProvider = new ECDKeyModelProvider
            {
                PrivateKey = _eCDiffieHellman.ExportECPrivateKeyPem(),
                PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem()
            };
        }

        /// <summary>
        /// Initializes a new instance of the KeyPairProvider class with a specific instance of ECDiffieHellman,
        /// allowing for custom configuration and use of an existing ECDiffieHellman instance.
        /// </summary>
        /// <param name="ecDiffieHellman">An existing instance of ECDiffieHellman for cryptographic operations.</param>
        public KeyPairProvider(ECDiffieHellman ecDiffieHellman)
        {
            _eCDiffieHellman = ecDiffieHellman ?? throw new ArgumentNullException(nameof(ecDiffieHellman));
            KeyModelProvider = new ECDKeyModelProvider
            {
                PrivateKey = _eCDiffieHellman.ExportECPrivateKeyPem(),
                PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem()
            };
        }

        // Additional constructors as necessary...

        /// <summary>
        /// Initializes a new instance of the KeyPairProvider class using a public key in byte array format.
        /// This allows for the creation of a key pair provider based on an external public key, typically used for
        /// cryptographic operations like key exchange.
        /// </summary>
        /// <param name="publicKey">The public key as a byte array.</param>
        public KeyPairProvider(byte[] publicKey)
        {
            ArgumentNullException.ThrowIfNull(publicKey, nameof(publicKey));

            _eCDiffieHellman = ECDiffieHellman.Create();
            _eCDiffieHellman.ImportSubjectPublicKeyInfo(publicKey, out _);
            KeyModelProvider = new ECDKeyModelProvider
            {
                PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem()
            };
        }

        /// <summary>
        /// Initializes a new instance of the KeyPairProvider class using specified elliptic curve parameters.
        /// This constructor allows for precise control over the elliptic curve characteristics used in the
        /// Elliptic Curve Diffie-Hellman (ECDH) key pair generation, enabling the creation of a key pair with
        /// custom curve parameters.
        /// </summary>
        /// <param name="parameters">The EC parameters defining the elliptic curve and key pair characteristics.</param>
        public KeyPairProvider(ECParameters parameters)
        {
            _eCDiffieHellman = ECDiffieHellman.Create(parameters);
            KeyModelProvider = new ECDKeyModelProvider
            {
                PrivateKey = _eCDiffieHellman.ExportECPrivateKeyPem(),
                PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem()
            };
        }

        /// <summary>
        /// Initializes a new instance of the KeyPairProvider class using a specific elliptic curve.
        /// This constructor allows the creation of an Elliptic Curve Diffie-Hellman (ECDH) key pair based on the provided curve,
        /// facilitating the use of custom curve parameters for enhanced cryptographic flexibility and security.
        /// </summary>
        /// <param name="curve">The elliptic curve to be used for ECDH key pair generation. This parameter specifies the curve parameters,
        /// including the curve type and any associated domain parameters required for initializing the ECDH key pair.</param>
        public KeyPairProvider(ECCurve curve)
        {
            _eCDiffieHellman = ECDiffieHellman.Create(curve);
            KeyModelProvider = new ECDKeyModelProvider
            {
                PrivateKey = _eCDiffieHellman.ExportPkcs8PrivateKeyPem(),
                PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem()
            };
        }

        /// <summary>
        /// Provides access to the underlying ECDiffieHellman instance for advanced cryptographic operations.
        /// </summary>
        /// <returns>The ECDiffieHellman instance used by this provider.</returns>
        public ECDiffieHellman GetECDiffieHellman()
        {
            return _eCDiffieHellman;
        }

        /// <summary>
        /// Releases all resources used by the KeyPairProvider.
        /// </summary>
        public void Dispose()
        {
            _eCDiffieHellman.Dispose();
        }
    }
}