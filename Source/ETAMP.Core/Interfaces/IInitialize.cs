#region

using System.Security.Cryptography;

#endregion

namespace ETAMP.Core.Interfaces;

public interface IInitialize : IDisposable
{
    void Initialize(ECDsa provider, HashAlgorithmName algorithmName);
}