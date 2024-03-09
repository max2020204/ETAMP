using ETAMPManagment.Models;

namespace ETAMPManagment.ETAMP.Encrypted.Interfaces
{
    public interface IEncryptToken
    {
        string EncryptETAMPToken(string jsonEtamp);

        ETAMPModel EncryptETAMP(string jsonEtamp);
    }
}