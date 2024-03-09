using Microsoft.IdentityModel.Tokens;

namespace ETAMPManagment.Validators.Interfaces
{
    public interface IETAMPValidator
    {
        Task<bool> ValidateETAMP(string etamp, string audience, string issuer, ECDsaSecurityKey tokenSecurityKey);

        Task<bool> ValidateETAMP(string etamp, ECDsaSecurityKey tokenSecurityKey);

        Task<bool> ValidateETAMPLite(string etamp, ECDsaSecurityKey tokenSecurityKey);
    }
}