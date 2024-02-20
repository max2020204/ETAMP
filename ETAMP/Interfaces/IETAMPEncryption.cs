using ETAMP.Models;

namespace ETAMP.Interfaces
{
    public interface IETAMPEncryption : IETAMP
    {
        string EncryptETAMPToken(string jsonEtamp);

        ETAMPEncrypted EncryptETAMP(string jsonEtamp);

        string CreateEncryptETAMPToken<T>(string updateType, T payload, bool signToken = true, double version = 1) where T : BasePaylaod;

        ETAMPEncrypted CreateEncryptETAMP<T>(string updateType, T payload, bool signToken = true, double version = 1) where T : BasePaylaod;
    }
}