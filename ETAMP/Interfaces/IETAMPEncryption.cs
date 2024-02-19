using ETAMP.Models;

namespace ETAMP.Interfaces
{
    public interface IETAMPEncryption : IETAMP
    {
        string EnryptETAMPToken(string jsonEtamp);

        ETAMPEncrypted EnryptETAMP(string jsonEtamp);

        string CreateEnryptETAMPToken<T>(string updateType, T payload, bool signToken = true, double version = 1) where T : BasePaylaod;

        ETAMPEncrypted CreateEnryptETAMP<T>(string updateType, T payload, bool signToken = true, double version = 1) where T : BasePaylaod;
    }
}