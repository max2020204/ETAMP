#region

using System.Security.Cryptography;

#endregion

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

public interface IECDsaRegistrar
{
    (Guid Id, bool status) Registrar(ECDsa ecdsa);
    bool Registrar(ECDsa ecdsa, string name);
}