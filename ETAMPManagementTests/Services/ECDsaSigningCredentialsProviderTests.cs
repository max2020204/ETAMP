#region

using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Services;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagementTests.Services;

public class ECDsaSigningCredentialsProviderTests
{
    [Fact]
    public void CreateSigningCredentials_ReturnsSigningCredentialsWithCorrectAlgorithm()
    {
        var algorithm = "ES256";
        var providerMock = new Mock<IECDsaProvider>();
        providerMock.Setup(x => x.GetECDsa()).Returns(ECDsa.Create());
        var provider = new ECDsaSigningCredentialsProvider();
        provider.Initialize(providerMock.Object);
        provider.SecurityAlgorithm = algorithm;

        var signingCredentials = provider.CreateSigningCredentials();
        Assert.NotNull(signingCredentials);
        Assert.IsType<ECDsaSecurityKey>(signingCredentials.Key);
        Assert.Equal(algorithm, signingCredentials.Algorithm);
    }
}