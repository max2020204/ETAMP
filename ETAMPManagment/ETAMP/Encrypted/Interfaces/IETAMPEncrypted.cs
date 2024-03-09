using ETAMPManagment.Models;

namespace ETAMPManagment.ETAMP.Encrypted.Interfaces
{
    public interface IETAMPEncrypted
    {
        string CreateEncryptETAMPToken<T>(string updateType, T payload, double version = 1) where T : BasePaylaod;

        ETAMPModel CreateEncryptETAMPTokenModel<T>(string updateType, T payload, double version = 1) where T : BasePaylaod;
    }
}