#region

using System.Security.Cryptography;
using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagment.Services.Tests;

public class ECDsaSigningCredentialsProviderTests
{
    [Fact]
    public void CreateSigningCredentials_ReturnsSigningCredentialsWithCorrectAlgorithm()
    {
        var algorithm = "ES256";
        var providerMock = new Mock<IECDsaProvider>();
        providerMock.Setup(x => x.GetECDsa()).Returns(ECDsa.Create());
        var provider = new ECDsaSigningCredentialsProvider(providerMock.Object);
        provider.SecurityAlgorithm = algorithm;

        var signingCredentials = provider.CreateSigningCredentials();
        Assert.NotNull(signingCredentials);
        Assert.IsType<ECDsaSecurityKey>(signingCredentials.Key);
        Assert.Equal(algorithm, signingCredentials.Algorithm);
    }
}