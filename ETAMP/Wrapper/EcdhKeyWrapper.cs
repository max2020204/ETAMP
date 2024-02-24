using ETAMP.Wrapper.Interfaces;
using System.Security.Cryptography;

namespace ETAMP.Wrapper
{
    /// <summary>
    /// Provides a wrapper for Elliptic Curve Diffie-Hellman (ECDH) key exchange functionality.
    /// This class allows generating ECDH key pairs, exchanging keys, and deriving shared secret keys.
    /// </summary>
    public class EcdhKeyWrapper : IEcdhKeyWrapper
    {
        private readonly ECDiffieHellman _ecdh;
        private byte[]? _keyExchanger;

        /// <summary>
        /// Gets the last exchanged or derived key material.
        /// </summary>
        /// <value>The byte array of the last key exchange or derived material.</value>
        public byte[]? KeyExchanger
        {
            get { return _keyExchanger; }
        }

        private ECDiffieHellmanPublicKey _ecdhPublicKey;

        /// <summary>
        /// Gets the public key of the current ECDH key pair.
        /// </summary>
        /// <value>The public key used in the ECDH key exchange.</value>
        public ECDiffieHellmanPublicKey HellmanPublicKey
        {
            get { return _ecdhPublicKey; }
        }

        private string _privateKey;

        /// <summary>
        /// Gets the PEM-encoded private key.
        /// </summary>
        /// <value>The PEM-encoded private key string.</value>
        public string PrivateKey
        {
            get { return _privateKey; }
        }

        private string _publicKey;

        /// <summary>
        /// Gets the PEM-encoded public key.
        /// </summary>
        /// <value>The PEM-encoded public key string.</value>
        public string PublicKey
        {
            get { return _publicKey; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcdhKeyWrapper"/> class using the default algorithm.
        /// This constructor generates a new ECDH key pair using the default elliptic curve and exports
        /// the keys in PEM format.
        /// </summary>
        public EcdhKeyWrapper()
        {
            _ecdh = ECDiffieHellman.Create();
            _privateKey = _ecdh.ExportECPrivateKeyPem();
            _publicKey = _ecdh.ExportSubjectPublicKeyInfoPem();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcdhKeyWrapper"/> class using a specified elliptic curve.
        /// This constructor generates a new ECDH key pair based on the specified curve.
        /// the keys in PEM format.
        /// </summary>
        /// <param name="curve">The elliptic curve to use.</param>
        public EcdhKeyWrapper(ECCurve curve)
        {
            _ecdh = ECDiffieHellman.Create(curve);
            _privateKey = _ecdh.ExportECPrivateKeyPem();
            _publicKey = _ecdh.ExportSubjectPublicKeyInfoPem();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcdhKeyWrapper"/> class using specified EC parameters.
        /// This constructor generates a new ECDH key pair based on the provided parameters.
        /// the keys in PEM format.
        /// </summary>
        /// <param name="parameters">The elliptic curve parameters.</param>
        public EcdhKeyWrapper(ECParameters parameters)
        {
            _ecdh = ECDiffieHellman.Create(parameters);
            _privateKey = _ecdh.ExportECPrivateKeyPem();
            _publicKey = _ecdh.ExportSubjectPublicKeyInfoPem();
        }

        /// <summary>
        /// Creates a new ECDH public key.
        /// </summary>
        /// <returns>The newly created ECDH public key.</returns>
        public virtual ECDiffieHellmanPublicKey CreateKey()
        {
            _ecdhPublicKey = _ecdh.PublicKey;
            return _ecdhPublicKey;
        }

        /// <summary>
        /// Derives a shared secret key material using the specified public key.
        /// </summary>
        /// <param name="key">The public key to derive the shared secret with.</param>
        /// <returns>The derived shared secret key material.</returns>
        public virtual byte[] DeriveKey(ECDiffieHellmanPublicKey key)
        {
            _keyExchanger = _ecdh.DeriveKeyMaterial(key);
            return _keyExchanger;
        }

        /// <summary>
        /// Derives a shared secret key material using the specified public key and applies a hash function.
        /// </summary>
        /// <param name="key">The public key to derive the shared secret with.</param>
        /// <param name="hash">The hash algorithm to use.</param>
        /// <param name="secretPrepend">Optional data to prepend to the secret before hashing.</param>
        /// <param name="secretAppend">Optional data to append to the secret before hashing.</param>
        /// <returns>The hashed derived secret key material.</returns>
        public virtual byte[] DeriveKeyHash(ECDiffieHellmanPublicKey key, HashAlgorithmName hash, byte[]? secretPrepend, byte[]? secretAppend)
        {
            _keyExchanger = _ecdh.DeriveKeyFromHash(key, hash, secretPrepend, secretAppend);
            return _keyExchanger;
        }

        /// <summary>
        /// Derives a shared secret key material using the specified public key and computes HMAC.
        /// </summary>
        /// <param name="key">The public key to derive the shared secret with.</param>
        /// <param name="hash">The hash algorithm to use for HMAC.</param>
        /// <param name="hmacKey">The key for HMAC. If null, the default key will be used.</param>
        /// <param name="secretPrepend">Optional data to prepend to the secret before computing HMAC.</param>
        /// <param name="secretAppend">Optional data to append to the secret before computing HMAC.</param>
        /// <returns>The HMAC of the derived secret key material.</returns>
        public virtual byte[] DeriveKeyHmac(ECDiffieHellmanPublicKey key, HashAlgorithmName hash, byte[]? hmacKey, byte[]? secretPrepend, byte[]? secretAppend)
        {
            _keyExchanger = _ecdh.DeriveKeyFromHmac(key, hash, hmacKey, secretPrepend, secretAppend);
            return _keyExchanger;
        }

        /// <summary>
        /// Derives a shared secret key material using the public key of another party.
        /// </summary>
        /// <param name="otherPartyPublicKey">The byte array containing the other party's public key.
        /// Cannot be null.</param>
        /// <returns>The derived shared secret key material.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided public key is null.</exception>
        public virtual byte[] DeriveKey(byte[] otherPartyPublicKey)
        {
            if (otherPartyPublicKey == null)
            {
                throw new ArgumentNullException(nameof(otherPartyPublicKey), "Public key cannot be null.");
            }

            using (var otherPartyEcdh = ECDiffieHellmanCngPublicKey.FromByteArray(otherPartyPublicKey, CngKeyBlobFormat.EccPublicBlob))
            {
                return _ecdh.DeriveKeyMaterial(otherPartyEcdh);
            }
        }

        /// <summary>
        /// Disposes the ECDH instance and releases all resources used by the <see cref="EcdhKeyWrapper"/>.
        /// </summary>
        public virtual void Dispose()
        {
            _ecdh.Dispose();
        }
    }
}