using ETAMPManagment.Models;

namespace ETAMPManagment.Interfaces
{
    public interface IETAMPBuilder<Type> where Type : struct, Enum
    {
        ETAMPModel Build();

        ETAMPBuilder CreateETAMP<T>(Type type, string updateType, T payload, double version = 1) where T : BasePayload;
    }
}