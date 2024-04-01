using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Reflection;
using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Services.Tests
{
    public class ECDsaSigningCredentialsProviderTests
    {
        [Fact]
        public void CreateSigningCredentials_ReturnsSigningCredentialsWithCorrectAlgorithm()
        {
            string algorithm = "ES256";
            var providerMock = new Mock<IECDsaProvider>();
            providerMock.Setup(x => x.GetECDsa()).Returns(ECDsa.Create());
            var provider = new ECDsaSigningCredentialsProvider(providerMock.Object, algorithm);

            var signingCredentials = provider.CreateSigningCredentials();
            Assert.NotNull(signingCredentials);
            Assert.IsType<ECDsaSecurityKey>(signingCredentials.Key);
            Assert.Equal(algorithm, signingCredentials.Algorithm);
        }
    }
}