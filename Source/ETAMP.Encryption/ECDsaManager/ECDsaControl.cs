#region

using ETAMP.Encryption.Base;
using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.ECDsaManager;

public class ECDsaControl : IECDsaControl
{
    private readonly ECDsaRegistrationBase _ecdsaRegistration;

    public ECDsaControl(ECDsaRegistrationBase ecdsaRegistration)
    {
        _ecdsaRegistration = ecdsaRegistration;
    }

    public bool Remove(Guid id)
    {
        return _ecdsaRegistration._registrationProviderGuid.Remove(id);
    }

    public bool Remove(string name)
    {
        return _ecdsaRegistration._registrationProviderString.Remove(name);
    }
}