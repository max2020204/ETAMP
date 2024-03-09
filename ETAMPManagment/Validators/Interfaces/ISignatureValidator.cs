using ETAMPManagment.Models;

namespace ETAMPManagment.Validators.Interfaces
{
    public interface ISignatureValidator
    {
        bool ValidateToken(string token, string tokenSignature);

        bool ValidateETAMPMessage(string etamp);

        bool ValidateETAMPMessage(ETAMPModel etamp);
    }
}