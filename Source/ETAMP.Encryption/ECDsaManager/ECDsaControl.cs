#region

using ETAMP.Encryption.Base;
using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.ECDsaManager;

public class ECDsaControl : IECDsaControl
{
    private readonly IECDsaStore _store;

    public ECDsaControl(IECDsaStore store)
    {
        _store = store;
    }

    public bool Remove(Guid id)
    {
        return _store.Remove(id);
    }

    public bool Remove(string name)
    {
        return _store.Remove(name);
    }
}