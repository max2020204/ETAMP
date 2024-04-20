using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Encryption.ECDsaManager.Tests
{
    public class ECDsaProviderTests
    {
        [Fact]
        public void RegisterEcdsa_ShouldSetEcdsaInstance()
        {
            var provider = new ECDsaProvider();
            var ecdsa = ECDsa.Create();

            var returnedProvider = provider.RegisterEcdsa(ecdsa);

            Assert.Same(ecdsa, provider.GetECDsa());
            Assert.Same(provider, returnedProvider);
        }

        [Fact]
        public void GetECDsa_ShouldReturnNull_WhenNotRegistered()
        {
            var provider = new ECDsaProvider();

            Assert.Null(provider.GetECDsa());
        }
    }
}