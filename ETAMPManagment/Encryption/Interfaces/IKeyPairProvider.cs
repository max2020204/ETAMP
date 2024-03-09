using System.Security.Cryptography;

namespace ETAMPManagment.Encryption.Interfaces
{
    /// <summary>
    /// Provides functionality for managing an Elliptic Curve Diffie-Hellman (ECDH) key pair and accessing its components.
    /// </summary>
    public interface IKeyPairProvider : IDisposable
    {
        /// <summary>
        /// Gets the private key of the ECDH key pair in a string format.
        /// </summary>
        /// <value>
        /// The private key as a string.
        /// </value>
        string PrivateKey { get; }

        /// <summary>
        /// Gets the public key of the ECDH key pair in a string format.
        /// </summary>
        /// <value>
        /// The public key as a string.
        /// </value>
        string PublicKey { get; }

        /// <summary>
        /// Gets the public key of the ECDH key pair as an <see cref="ECDiffieHellmanPublicKey"/>.
        /// </summary>
        /// <value>
        /// The public key as an <see cref="ECDiffieHellmanPublicKey"/>.
        /// </value>
        ECDiffieHellmanPublicKey HellmanPublicKey { get; }

        /// <summary>
        /// Gets the <see cref="ECDiffieHellman"/> instance representing the ECDH key pair.
        /// </summary>
        /// <returns>
        /// An <see cref="ECDiffieHellman"/> instance used for cryptographic operations.
        /// </returns>
        ECDiffieHellman GetECDiffieHellman();
    }
}