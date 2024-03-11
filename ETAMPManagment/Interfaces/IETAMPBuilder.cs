using ETAMPManagment.Models;

namespace ETAMPManagment.Interfaces
{
    public interface IETAMPBuilder
    {
        ETAMPModel Build();

        ETAMPBuilder CreateEncryptedETAMP<T>(string updateType, T payload, double version = 1) where T : BasePaylaod;

        ETAMPBuilder CreateEncryptedSignETAMP<T>(string updateType, T payload, double version = 1) where T : BasePaylaod;

        ETAMPBuilder CreateETAMP<T>(string updateType, T payload, double version = 1) where T : BasePaylaod;

        ETAMPBuilder CreateSignETAMP<T>(string updateType, T payload, double version = 1) where T : BasePaylaod;
    }
}