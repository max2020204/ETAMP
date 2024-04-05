using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Models;
using System.Security.Cryptography;

namespace ETAMPManagment.Encryption
{
    /// <summary>
    /// Provides an implementation for managing Elliptic Curve Diffie-Hellman (ECDH) key pairs and facilitating cryptographic operations.
    /// </summary>
    public sealed class KeyPairProvider : IKeyPairProvider
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
        /// Initializes a new instance of the KeyPairProvider class with a specific instance of ECDiffieHellman,
        /// allowing for custom configuration and use of an existing ECDiffieHellman instance.
        /// </summary>
        /// <param name="ecDiffieHellman">An existing instance of ECDiffieHellman for cryptographic operations.</param>
        public KeyPairProvider(ECDiffieHellman ecDiffieHellman)
        {
            _eCDiffieHellman = ecDiffieHellman
                ?? throw new ArgumentNullException(nameof(ecDiffieHellman));
            KeyModelProvider = new ECDKeyModelProvider
            {
                PrivateKey = _eCDiffieHellman.ExportECPrivateKeyPem(),
                PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem()
            };
        }

        /// <summary>
        /// Initializes a new instance of the KeyPairProvider class using a public key in byte array format.
        /// This allows for the creation of a key pair provider based on an external public key, typically used for
        /// cryptographic operations like key exchange.
        /// </summary>
        /// <param name="publicKey">The public key as a byte array.</param>
        public KeyPairProvider(byte[]? publicKey)
        {
            ArgumentNullException.ThrowIfNull(publicKey);

            _eCDiffieHellman = ECDiffieHellman.Create();
            _eCDiffieHellman.ImportSubjectPublicKeyInfo(publicKey, out _);
            KeyModelProvider = new ECDKeyModelProvider
            {
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