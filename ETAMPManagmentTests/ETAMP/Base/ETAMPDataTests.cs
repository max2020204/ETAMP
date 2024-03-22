using ETAMPManagment.Models;
using ETAMPManagment.Services;
using ETAMPManagment.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.ETAMP.Base.Tests
{
    public class ETAMPDataTests
    {
        private Mock<ISigningCredentialsProvider> sign;
        private string messageId;
        private BasePayload payload;

        public ETAMPDataTests()
        {
            messageId = Guid.NewGuid().ToString();
            payload = new BasePayload();
            sign = new Mock<ISigningCredentialsProvider>();
        }

        private string GenerateToken(string ecdsaAlgorithm)
        {
            sign.Setup(x => x.CreateSigningCredentials()).Returns(new ECDsaSigningCredentialsProvider(ECDsa.Create(), ecdsaAlgorithm).CreateSigningCredentials());
            ETAMPData data = new ETAMPData(sign.Object);
            return data.CreateEtampData(messageId, payload);
        }

        [Theory]
        [InlineData("ES256", SecurityAlgorithms.EcdsaSha256Signature)]
        [InlineData("ES384", SecurityAlgorithms.EcdsaSha384Signature)]
        [InlineData("ES512", SecurityAlgorithms.EcdsaSha512Signature)]
        public void CreateEtampData_ShouldGenerateTokenWithCorrectAlgorithm(string securityAlgorithm, string ecdsaAlgorithm)
        {
            string token = GenerateToken(ecdsaAlgorithm);
            var handler = new JwtSecurityTokenHandler();
            var readToken = handler.ReadJwtToken(token);

            Assert.Equal(securityAlgorithm, readToken.Header.Alg);
        }

        [Fact]
        public void CreateEtampData_ShouldSetCorrectHeader()
        {
            string token = GenerateToken(SecurityAlgorithms.EcdsaSha256Signature);
            var handler = new JwtSecurityTokenHandler();
            var readToken = handler.ReadJwtToken(token);

            Assert.Equal("ES256", readToken.Header.Alg);
            Assert.Equal("ETAMP", readToken.Header.Typ);
        }

        [Fact]
        public void CreateEtampData_ShouldContainCorrectClaims()
        {
            string token = GenerateToken(SecurityAlgorithms.EcdsaSha256Signature);
            var handler = new JwtSecurityTokenHandler();
            var readToken = handler.ReadJwtToken(token);

            Assert.Contains(readToken.Claims, c => c.Type == "MessageId" && c.Value == messageId);
        }
    }
}