using System.Security.Cryptography;

namespace ETAMPManagment.Encryption.Interfaces
{
    public interface IKeyExchanger
    {
        byte[] GetSharedSecret();

        byte[] DeriveKeyHash(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash, byte[]? secretPrepend, byte[]? secretAppend);

        byte[] DeriveKeyHmac(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash, byte[]? hmacKey, byte[]? secretPrepend, byte[]? secretAppend);

        byte[] DeriveKey(ECDiffieHellmanPublicKey publicKey);

        byte[] DeriveKey(byte[] otherPartyPublicKey);
    }
}