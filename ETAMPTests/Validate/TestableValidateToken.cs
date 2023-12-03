using ETAMP.Services.Interfaces;
using ETAMP.Validate;
using System.IdentityModel.Tokens.Jwt;

namespace ETAMPTests.Validate
{
    internal class TestableValidateToken : ValidateToken
    {
        public TestableValidateToken(IVerifyWrapper verifyWrapper) : base(verifyWrapper)
        {
        }

        public TestableValidateToken(IVerifyWrapper verifyWrapper, JwtSecurityTokenHandler jwtSecurityToken) : base(verifyWrapper, jwtSecurityToken)
        {
        }

        public override bool VerifyETAMP(string etamp)
        {
            return true;
        }
    }
}