#region

using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.Base;

public abstract class ECDsaProviderBase : IECDsaProvider
{
    public ECDsaProviderBase(ECDsaRegistrationBase ecDsaRegistration)
    {
        ECDsaRegistration = ecDsaRegistration;
    }

    internal ECDsa ECDsa { get; set; }
    internal ECDsaRegistrationBase ECDsaRegistration { get; set; }

    public abstract ECDsa GetECDsa(Guid id);
    public abstract ECDsa GetECDsa(string name);
}