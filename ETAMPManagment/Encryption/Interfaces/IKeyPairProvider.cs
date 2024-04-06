using ETAMPManagment.Models;
using System.Security.Cryptography;

namespace ETAMPManagment.Encryption.Interfaces
{
    /// <summary>
    /// Defines a contract for managing Elliptic Curve Diffie-Hellman (ECDH) key pairs and facilitates access to their components,
    /// including the public and private keys. This interface abstracts the functionality necessary to work with ECDH keys
    /// in various cryptographic operations.
    /// </summary>
    public interface IKeyPairProvider : IDisposable
    {
        /// <summary>
        /// Gets the key model provider that contains the private and public keys in PEM format.
        /// </summary>
        /// <value>
        /// The <see cref="ECDKeyModelProvider"/> that holds the private and public key information.
        /// </value>
        ECDKeyModelProvider KeyModelProvider { get; }

        /// <summary>
        /// Gets the public key component of the ECDH key pair.
        /// </summary>
        /// <value>
        /// The public key as an <see cref="ECDiffieHellmanPublicKey"/>, which can be used in cryptographic operations
        /// such as key agreement or signature verification.
        /// </value>
        ECDiffieHellmanPublicKey HellmanPublicKey { get; }

        /// <summary>
        /// Retrieves the underlying <see cref="ECDiffieHellman"/> instance representing the ECDH key pair,
        /// allowing for direct cryptographic operations such as key exchange.
        /// </summary>
        /// <returns>An <see cref="ECDiffieHellman"/> instance representing the ECDH key pair, suitable for cryptographic operations.</returns>
        ECDiffieHellman GetECDiffieHellman();

        /// <summary>
        /// Imports a private key into the ECDH key pair provider.
        /// </summary>
        /// <param name="privateKey">The private key as a byte array to import into the provider.</param>
        void ImportPrivateKey(byte[] privateKey);

        /// <summary>
        /// Imports a public key into the ECDH key pair provider.
        /// </summary>
        /// <param name="publicKey">The public key as a byte array to import into the provider.</param>
        void ImportPublicKey(byte[] publicKey);
    }
}