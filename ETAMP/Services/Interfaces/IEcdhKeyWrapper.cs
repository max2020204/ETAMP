using System.Security.Cryptography;

namespace ETAMP.Services.Interfaces
{
    public interface IEcdhKeyWrapper : IDisposable
    {
        string PrivateKey { get; }
        string PublicKey { get; }
        byte[]? KeyExchanger { get; }
        ECDiffieHellmanPublicKey HellmanPublicKey { get; }

        ECDiffieHellmanPublicKey CreateKey();

        byte[] DeriveKey(ECDiffieHellmanPublicKey key);

        byte[] DeriveKey(byte[] otherPartyPublicKey);

        byte[] DeriveKeyHash(ECDiffieHellmanPublicKey key, HashAlgorithmName hash, byte[]? secretPrepend, byte[]? secretAppend);

        byte[] DeriveKeyHmac(ECDiffieHellmanPublicKey key, HashAlgorithmName hash, byte[]? hmacKey, byte[]? secretPrepend, byte[]? secretAppend);
    }
}