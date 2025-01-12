using ETAMP.Encryption.Base;
using ETAMP.Encryption.Interfaces.ECDSAManager;

namespace ETAMP.Encryption.ECDsaManager;

public class ECDsaStore : IECDsaStore
{
    private readonly Dictionary<Guid, ECDsaProviderBase> _guidStore;
    private readonly Dictionary<string, ECDsaProviderBase> _nameStore;

    public ECDsaStore()
    {
        _guidStore = new Dictionary<Guid, ECDsaProviderBase>();
        _nameStore = new Dictionary<string, ECDsaProviderBase>();
    }

    public bool Add(Guid id, ECDsaProviderBase provider) 
    {
        return _guidStore.TryAdd(id, provider);
    }

    public bool Add(string name, ECDsaProviderBase provider)
    {
        return _nameStore.TryAdd(name, provider);
    }

    public bool Remove(Guid id)
    {
        return _guidStore.Remove(id);
    }

    public bool Remove(string name)
    {
        return _nameStore.Remove(name);
    }

    public ECDsaProviderBase? Get(Guid id)
    {
        return _guidStore.TryGetValue(id, out var provider) ? provider : null;
    }

    public ECDsaProviderBase? Get(string name)
    {
        return _nameStore.TryGetValue(name, out var provider) ? provider : null;
    }
}