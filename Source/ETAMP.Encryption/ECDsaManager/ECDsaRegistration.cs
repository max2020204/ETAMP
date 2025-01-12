#region

using System.Security.Cryptography;
using ETAMP.Encryption.Base;
using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.ECDsaManager;

public class ECDsaRegistration : ECDsaRegistrationBase
{
    private readonly ECDsaProviderBase _provider;
    public ECDsaRegistration(IECDsaStore store, ECDsaProviderBase provider) : base(store)
    {
        _provider = provider;
    }

    public override (Guid Id, bool status) Registrar(ECDsa ecdsa)
    {
        _provider.SetECDsa(ecdsa);
        var guid = Guid.NewGuid();
        var status = Store.Add(guid, _provider);
        return (guid, status);
    }

    public override bool Registrar(ECDsa ecdsa, string name)
    {
        _provider.SetECDsa(ecdsa);
        return Store.Add(name, _provider);
    }
}