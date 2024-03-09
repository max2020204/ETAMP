using ETAMPManagment.Models;

namespace ETAMPManagment.ETAMP.Base.Interfaces
{
    public interface IETAMPBase
    {
        string CreateETAMP<T>(string updateType, T payload, double version = 1) where T : BasePaylaod;

        ETAMPModel CreateETAMPModel<T>(string updateType, T payload, double version = 1) where T : BasePaylaod;
    }
}