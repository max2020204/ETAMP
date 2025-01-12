using ETAMP.Encryption.Base;

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

public interface IECDsaStore
{
    bool Add(Guid id, ECDsaProviderBase provider);
    bool Add(string name, ECDsaProviderBase provider);
    bool Remove(Guid id);
    bool Remove(string name);
    ECDsaProviderBase? Get(Guid id);
    ECDsaProviderBase? Get(string name);
}