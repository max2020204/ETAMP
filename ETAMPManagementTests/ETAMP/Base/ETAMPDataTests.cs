#region

using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.ETAMP.Base;
using ETAMPManagement.Models;
using ETAMPManagement.Services;
using ETAMPManagement.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagementTests.ETAMP.Base;

public class ETAMPDataTests
{
    private readonly JwtSecurityTokenHandler _jwtHandler;
    private readonly string _messageId;

    private readonly BasePayload _payload;

    private readonly Mock<IECDsaProvider> _providerMock;
    private readonly Mock<ISigningCredentialsProvider> _signMock;

    public ETAMPDataTests()
    {
        _messageId = Guid.NewGuid().ToString();
        _providerMock = new Mock<IECDsaProvider>();
        _providerMock.Setup(x => x.GetECDsa()).Returns(ECDsa.Create());
        _payload = new BasePayload();
        _signMock = new Mock<ISigningCredentialsProvider>();
        _jwtHandler = new JwtSecurityTokenHandler();
    }

    private string GenerateToken(string ecdsaAlgorithm)
    {
        var ecdsa = new ECDsaSigningCredentialsProvider();
        ecdsa.Initialize(_providerMock.Object);
        ecdsa.SecurityAlgorithm = ecdsaAlgorithm;
        _signMock.Setup(x => x.CreateSigningCredentials(false))
            .Returns(ecdsa.CreateSigningCredentials());

        var data = new ETAMPData(_signMock.Object);
        return data.CreateEtampData(_messageId, _payload);
    }

    [Theory]
    [InlineData("ES256", SecurityAlgorithms.EcdsaSha256Signature)]
    [InlineData("ES384", SecurityAlgorithms.EcdsaSha384Signature)]
    [InlineData("ES512", SecurityAlgorithms.EcdsaSha512Signature)]
    public void CreateEtampData_ShouldGenerateTokenWithCorrectAlgorithm(string securityAlgorithm, string ecdsaAlgorithm)
    {
        var token = GenerateToken(ecdsaAlgorithm);

        var readToken = _jwtHandler.ReadJwtToken(token);

        Assert.Equal(securityAlgorithm, readToken.Header.Alg);
    }

    [Fact]
    public void CreateEtampData_ShouldSetCorrectHeaderAndContainCorrectClaims()
    {
        var token = GenerateToken(SecurityAlgorithms.EcdsaSha256Signature);
        var readToken = _jwtHandler.ReadJwtToken(token);

        Assert.Equal("ES256", readToken.Header.Alg);
        Assert.Equal("ETAMP", readToken.Header.Typ);

        Assert.Contains(readToken.Claims, c => c.Type == "MessageId" && c.Value == _messageId);
    }
}