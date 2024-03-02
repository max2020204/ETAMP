using ETAMPManagment.Services;
using ETAMPManagment.Wrapper.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace ETAMPManagmentTests.Services
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