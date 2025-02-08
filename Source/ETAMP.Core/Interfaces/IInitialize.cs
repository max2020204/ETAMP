using System.Security.Cryptography;

namespace ETAMP.Core.Interfaces;

public interface IInitialize : IDisposable
{
    void Initialize(ECDsa? provider, HashAlgorithmName algorithmName);
}