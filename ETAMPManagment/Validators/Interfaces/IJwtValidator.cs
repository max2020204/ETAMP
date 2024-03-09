using Microsoft.IdentityModel.Tokens;

namespace ETAMPManagment.Validators.Interfaces
{
    public interface IJwtValidator
    {
        Task<bool> ValidateLifeTime(string token, ECDsaSecurityKey securityKey);

        bool IsValidJwtToken(string token);

        Task<bool> ValidateToken(string token, string audience, string issuer, ECDsaSecurityKey securityKey);
    }
}