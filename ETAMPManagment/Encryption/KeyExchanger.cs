using ETAMPManagment.Encryption.Interfaces;
using System.Security.Cryptography;

namespace ETAMPManagment.Encryption
{
    public class KeyExchanger : IKeyExchanger
    {
        private byte[] _sharedSecret;

        private readonly IKeyPairProvider _keyProvider;

        public KeyExchanger(IKeyPairProvider keyPairProvider)
        {
            _keyProvider = keyPairProvider ?? throw new ArgumentNullException(nameof(keyPairProvider));
        }

        public virtual byte[] DeriveKeyHash(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash, byte[]? secretPrepend, byte[]? secretAppend)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }
            return _sharedSecret = _keyProvider.GetECDiffieHellman().DeriveKeyFromHash(publicKey, hash, secretPrepend, secretAppend);
        }

        public virtual byte[] DeriveKey(ECDiffieHellmanPublicKey publicKey)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }
            return _sharedSecret = _keyProvider.GetECDiffieHellman().DeriveKeyMaterial(publicKey);
        }

        public virtual byte[] DeriveKeyHmac(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash, byte[]? hmacKey, byte[]? secretPrepend, byte[]? secretAppend)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }
            return _sharedSecret = _keyProvider.GetECDiffieHellman().DeriveKeyFromHmac(publicKey, hash, hmacKey, secretPrepend, secretAppend);
        }

        public virtual byte[] DeriveKey(byte[] otherPartyPublicKey)
        {
            if (otherPartyPublicKey == null)
            {
                throw new ArgumentNullException(nameof(otherPartyPublicKey), "Public key cannot be null.");
            }

            using ECDiffieHellmanPublicKey otherPartyEcdh = ECDiffieHellmanCngPublicKey.FromByteArray(otherPartyPublicKey, CngKeyBlobFormat.EccPublicBlob);
            return _sharedSecret = _keyProvider.GetECDiffieHellman().DeriveKeyMaterial(otherPartyEcdh);
        }

        public virtual byte[] GetSharedSecret()
        {
            return _sharedSecret;
        }
    }
}