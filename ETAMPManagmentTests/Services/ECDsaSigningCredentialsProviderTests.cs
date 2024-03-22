using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Services.Tests
{
    public class ECDsaSigningCredentialsProviderTests
    {
        [Fact]
        public void CreateSigningCredentials_ReturnsSigningCredentialsWithCorrectAlgorithm()
        {
            using var ecdsa = ECDsa.Create();
            string algorithm = "ES256";
            var provider = new ECDsaSigningCredentialsProvider(ecdsa, algorithm);

            var signingCredentials = provider.CreateSigningCredentials();
            Assert.NotNull(signingCredentials);
            Assert.IsType<ECDsaSecurityKey>(signingCredentials.Key);
            Assert.Equal(algorithm, signingCredentials.Algorithm);
            Assert.Same(ecdsa, ((ECDsaSecurityKey)signingCredentials.Key).ECDsa);
        }
    }
}