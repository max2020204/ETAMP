using System.Security.Cryptography;

namespace ETAMPManagment.Encryption.Interfaces
{
    public interface IKeyPairProvider : IDisposable
    {
        string PrivateKey { get; }
        string PublicKey { get; }

        ECDiffieHellmanPublicKey HellmanPublicKey { get; }

        ECDiffieHellman GetECDiffieHellman();
    }
}