using ETAMPManagment.Models;

namespace ETAMPManagment.Interfaces
{
    public interface IETAMPEncryption : IETAMP
    {
        string EncryptETAMPToken(string jsonEtamp);

        ETAMPEncrypted EncryptETAMP(string jsonEtamp);

        string CreateEncryptETAMPToken<T>(string updateType, T payload, bool signToken = true, double version = 1) where T : BasePaylaod;

        ETAMPEncrypted CreateEncryptETAMPFull<T>(string updateType, T payload, bool signToken = true, double version = 1) where T : BasePaylaod;

        string CreateEncryptETAMPWithoutSignature<T>(string updateType, T payload, bool signToken = true, double version = 1.0) where T : BasePaylaod;

        ETAMPEncrypted CreateEncryptETAMPWithoutSignatureFull<T>(string updateType, T payload, bool signToken = true, double version = 1.0) where T : BasePaylaod;
    }
}