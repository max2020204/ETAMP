#region

using System.Security.Cryptography;
using ETAMP.Encryption.Base;

#endregion

namespace ETAMP.Encryption.ECDsaManager;

public class ECDsaRegistration : ECDsaRegistrationBase
{
    private readonly ECDsaProviderBase _provider;

    public ECDsaRegistration(ECDsaProviderBase provider)
    {
        _provider = provider;
    }

    public override (Guid Id, bool status) Registrar(ECDsa ecdsa)
    {
        _provider.ECDsa = ecdsa;
        var guid = Guid.NewGuid();
        var status = _registrationProviderGuid.TryAdd(guid, _provider);
        return (guid, status);
    }

    public override bool Registrar(ECDsa ecdsa, string name)
    {
        _provider.ECDsa = ecdsa;
        return _registrationProviderString.TryAdd(name, _provider);
    }
}