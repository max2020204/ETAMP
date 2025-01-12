using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces.ECDSAManager;

namespace ETAMP.Encryption.Base;

public abstract class ECDsaRegistrationBase : IECDsaRegistrar
{
    public abstract (Guid Id, bool status) Registrar(ECDsa ecdsa);
    public abstract bool Registrar(ECDsa ecdsa, string name);
    protected IECDsaStore Store { get; }

    protected ECDsaRegistrationBase(IECDsaStore store)
    {
        Store = store;
    }
    

}