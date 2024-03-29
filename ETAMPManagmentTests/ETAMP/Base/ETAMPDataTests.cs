﻿using ETAMPManagment.Models;
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
        private readonly Mock<ISigningCredentialsProvider> _signMock;
        private readonly string _messageId;
        private readonly BasePayload _payload;
        private readonly JwtSecurityTokenHandler _jwtHandler;

        public ETAMPDataTests()
        {
            _messageId = Guid.NewGuid().ToString();
            _payload = new BasePayload();
            _signMock = new Mock<ISigningCredentialsProvider>();
            _jwtHandler = new JwtSecurityTokenHandler();
        }

        private string GenerateToken(string ecdsaAlgorithm)
        {
            _signMock.Setup(x => x.CreateSigningCredentials())
                .Returns(new ECDsaSigningCredentialsProvider(ECDsa.Create(), ecdsaAlgorithm)
                .CreateSigningCredentials());

            ETAMPData data = new ETAMPData(_signMock.Object);
            return data.CreateEtampData(_messageId, _payload);
        }

        [Theory]
        [InlineData("ES256", SecurityAlgorithms.EcdsaSha256Signature)]
        [InlineData("ES384", SecurityAlgorithms.EcdsaSha384Signature)]
        [InlineData("ES512", SecurityAlgorithms.EcdsaSha512Signature)]
        public void CreateEtampData_ShouldGenerateTokenWithCorrectAlgorithm(string securityAlgorithm, string ecdsaAlgorithm)
        {
            string token = GenerateToken(ecdsaAlgorithm);
            var readToken = _jwtHandler.ReadJwtToken(token);

            Assert.Equal(securityAlgorithm, readToken.Header.Alg);
        }

        [Fact]
        public void CreateEtampData_ShouldSetCorrectHeaderAndContainCorrectClaims()
        {
            string token = GenerateToken(SecurityAlgorithms.EcdsaSha256Signature);
            var readToken = _jwtHandler.ReadJwtToken(token);

            Assert.Equal("ES256", readToken.Header.Alg);
            Assert.Equal("ETAMP", readToken.Header.Typ);

            Assert.Contains(readToken.Claims, c => c.Type == "MessageId" && c.Value == _messageId);
        }
    }
}