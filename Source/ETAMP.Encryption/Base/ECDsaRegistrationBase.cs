#region

using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.Base;

public abstract class ECDsaRegistrationBase : IECDsaRegistrar
{
    public ECDsaRegistrationBase()
    {
        _registrationProviderGuid = new Dictionary<Guid, ECDsaProviderBase>();
        _registrationProviderString = new Dictionary<string, ECDsaProviderBase>();
    }

    internal IDictionary<Guid, ECDsaProviderBase> _registrationProviderGuid { get; set; }
    internal IDictionary<string, ECDsaProviderBase> _registrationProviderString { get; set; }
    public abstract (Guid Id, bool status) Registrar(ECDsa ecdsa);
    public abstract bool Registrar(ECDsa ecdsa, string name);
}