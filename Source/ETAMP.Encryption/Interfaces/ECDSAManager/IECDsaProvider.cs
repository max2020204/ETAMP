#region

using System.Security.Cryptography;

#endregion

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

public interface IECDsaProvider
{
    ECDsa GetECDsa(Guid id);
    ECDsa GetECDsa(string name);
}