using Xunit;

namespace ETAMPManagment.Encryption.ECDsaManager.Tests
{
    public class PemKeyCleanerTests
    {
        [Fact]
        public void ClearPEMPrivateKey_ShouldRemovePEMFormatting()
        {
            var cleaner = new PemKeyCleaner();
            string privateKeyWithPEM = "-----BEGIN PRIVATE KEY-----\nMIIEvAIBADANBgkqh...\r-----END PRIVATE KEY-----";

            cleaner.ClearPEMPrivateKey(privateKeyWithPEM);

            Assert.DoesNotContain("-----BEGIN PRIVATE KEY-----", cleaner.KeyModelProvider.PrivateKey);
            Assert.DoesNotContain("-----END PRIVATE KEY-----", cleaner.KeyModelProvider.PrivateKey);
            Assert.DoesNotContain("\n", cleaner.KeyModelProvider.PrivateKey);
            Assert.DoesNotContain("\r", cleaner.KeyModelProvider.PrivateKey);
        }

        [Fact]
        public void ClearPEMPublicKey_ShouldRemovePEMFormatting()
        {
            var cleaner = new PemKeyCleaner();
            string publicKeyWithPEM = "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8A...\r-----END PUBLIC KEY-----";

            cleaner.ClearPEMPublicKey(publicKeyWithPEM);

            Assert.DoesNotContain("-----BEGIN PUBLIC KEY-----", cleaner.KeyModelProvider.PublicKey);
            Assert.DoesNotContain("-----END PUBLIC KEY-----", cleaner.KeyModelProvider.PublicKey);
            Assert.DoesNotContain("\n", cleaner.KeyModelProvider.PublicKey);
            Assert.DoesNotContain("\r", cleaner.KeyModelProvider.PublicKey);
        }

        [Fact]
        public void ClearPEMPrivateKey_ShouldReturnSelfForChaining()
        {
            var cleaner = new PemKeyCleaner();
            string privateKeyWithPEM = "some key";

            var result = cleaner.ClearPEMPrivateKey(privateKeyWithPEM);

            Assert.Same(cleaner, result);
        }

        [Fact]
        public void ClearPEMPublicKey_ShouldReturnSelfForChaining()
        {
            var cleaner = new PemKeyCleaner();
            string publicKeyWithPEM = "some key";

            var result = cleaner.ClearPEMPublicKey(publicKeyWithPEM);

            Assert.Same(cleaner, result);
        }
    }
}