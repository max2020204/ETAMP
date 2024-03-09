using ETAMPManagment.Encryption.Interfaces;
using System.Security.Cryptography;

namespace ETAMPManagment.Encryption
{
    /// <summary>
    /// Manages key exchange and key derivation operations using Elliptic Curve Diffie-Hellman (ECDH) algorithm.
    /// </summary>
    public class KeyExchanger : IKeyExchanger
    {
        private byte[] _sharedSecret;

        /// <summary>
        /// Provides access to the key pair used in the ECDH algorithm.
        /// </summary>
        private readonly IKeyPairProvider _keyProvider;

        /// <summary>
        /// Initializes a new instance of the KeyExchanger class with a specified key pair provider.
        /// </summary>
        /// <param name="keyPairProvider">The provider for ECDH key pair.</param>
        public KeyExchanger(IKeyPairProvider keyPairProvider)
        {
            _keyProvider = keyPairProvider ?? throw new ArgumentNullException(nameof(keyPairProvider));
        }

        /// <summary>
        /// Derives a key using a hash function from the given public key and optional prepended or appended data.
        /// </summary>
        /// <param name="publicKey">The public key of the other party.</param>
        /// <param name="hash">The hash algorithm to use for key derivation.</param>
        /// <param name="secretPrepend">Optional data to prepend to the derived secret before hashing.</param>
        /// <param name="secretAppend">Optional data to append to the derived secret before hashing.</param>
        /// <returns>A byte array representing the derived key.</returns>
        public virtual byte[] DeriveKeyHash(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash, byte[]? secretPrepend, byte[]? secretAppend)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }
            return _sharedSecret = _keyProvider.GetECDiffieHellman().DeriveKeyFromHash(publicKey, hash, secretPrepend, secretAppend);
        }

        /// <summary>
        /// Derives a key directly from the given public key using the ECDH algorithm.
        /// </summary>
        /// <param name="publicKey">The public key of the other party.</param>
        /// <returns>A byte array representing the derived key material.</returns>
        public virtual byte[] DeriveKey(ECDiffieHellmanPublicKey publicKey)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }
            return _sharedSecret = _keyProvider.GetECDiffieHellman().DeriveKeyMaterial(publicKey);
        }

        /// <summary>
        /// Derives a key using HMAC from the given public key, hash algorithm, and optional data.
        /// </summary>
        /// <param name="publicKey">The public key of the other party.</param>
        /// <param name="hash">The hash algorithm to use for HMAC.</param>
        /// <param name="hmacKey">The key to use for HMAC.</param>
        /// <param name="secretPrepend">Optional data to prepend to the derived secret before applying HMAC.</param>
        /// <param name="secretAppend">Optional data to append to the derived secret before applying HMAC.</param>
        /// <returns>A byte array representing the derived key.</returns>
        public virtual byte[] DeriveKeyHmac(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash, byte[]? hmacKey, byte[]? secretPrepend, byte[]? secretAppend)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }
            return _sharedSecret = _keyProvider.GetECDiffieHellman().DeriveKeyFromHmac(publicKey, hash, hmacKey, secretPrepend, secretAppend);
        }

        /// <summary>
        /// Derives a key material from a raw byte array representing the other party's public key.
        /// </summary>
        /// <param name="otherPartyPublicKey">The raw byte array of the other party's public key.</param>
        /// <returns>A byte array representing the derived key material.</returns>
        public virtual byte[] DeriveKey(byte[] otherPartyPublicKey)
        {
            if (otherPartyPublicKey == null)
            {
                throw new ArgumentNullException(nameof(otherPartyPublicKey), "Public key cannot be null.");
            }

            using ECDiffieHellmanPublicKey otherPartyEcdh = ECDiffieHellmanCngPublicKey.FromByteArray(otherPartyPublicKey, CngKeyBlobFormat.EccPublicBlob);
            return _sharedSecret = _keyProvider.GetECDiffieHellman().DeriveKeyMaterial(otherPartyEcdh);
        }

        /// <summary>
        /// Retrieves the shared secret generated from the key exchange process.
        /// </summary>
        /// <returns>A byte array representing the shared secret.</returns>
        public virtual byte[] GetSharedSecret()
        {
            return _sharedSecret;
        }
    }
}