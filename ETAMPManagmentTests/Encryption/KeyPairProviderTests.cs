using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Encryption.Tests
{
    public class KeyPairProviderTests
    {
        [Fact]
        public void HellmanPublicKey_ShouldMatchPublicKeyFromECDiffieHellman()
        {
            using var eCDiffieHellman = ECDiffieHellman.Create();
            var provider = new KeyPairProvider(eCDiffieHellman);

            Assert.Equal(eCDiffieHellman.PublicKey.ExportSubjectPublicKeyInfo(), provider.HellmanPublicKey.ExportSubjectPublicKeyInfo());
        }

        [Fact]
        public void Constructor_WithECDiffieHellman_ShouldSetPropertiesCorrectly()
        {
            using var eCDiffieHellman = ECDiffieHellman.Create();
            var provider = new KeyPairProvider(eCDiffieHellman);

            Assert.NotNull(provider.KeyModelProvider.PrivateKey);
            Assert.NotNull(provider.KeyModelProvider.PublicKey);
        }
    }
}