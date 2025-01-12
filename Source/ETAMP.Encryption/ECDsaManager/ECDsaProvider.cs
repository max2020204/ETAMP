#region

using System.Security.Cryptography;
using ETAMP.Encryption.Base;

#endregion

namespace ETAMP.Encryption.ECDsaManager;

public class ECDsaProvider : ECDsaProviderBase
{
    public ECDsaProvider(ECDsaRegistrationBase ecDsaRegistration) : base(ecDsaRegistration)
    {
    }

    public override ECDsa? GetECDsa(Guid id)
    {
        ECDsaRegistration._registrationProviderGuid.TryGetValue(id, out var result);
        return result != null ? result.ECDsa : null;
    }

    public override ECDsa GetECDsa(string name)
    {
        ECDsaRegistration._registrationProviderString.TryGetValue(name, out var result);
        return result != null ? result.ECDsa : null;
    }

   
}