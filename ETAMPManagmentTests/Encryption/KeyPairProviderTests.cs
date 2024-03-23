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
        public void Constructor_WithInvalidPublicKey_ShouldThrowException()
        {
            byte[] invalidPublicKey = { 0x01, 0x02, 0x03 };

            var exception = Assert.Throws<CryptographicException>(() => new KeyPairProvider(invalidPublicKey));

            Assert.NotNull(exception);
            Assert.Contains("ASN1", exception.Message);
        }

        [Fact]
        public void Constructor_WithEmptyPublicKey_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new KeyPairProvider(publicKey: null));
        }

        [Fact]
        public void Constructor_WithECDiffieHellman_ShouldSetPropertiesCorrectly()
        {
            using var eCDiffieHellman = ECDiffieHellman.Create();
            var provider = new KeyPairProvider(eCDiffieHellman);

            Assert.NotNull(provider.PrivateKey);
            Assert.NotNull(provider.PublicKey);
        }

        [Fact]
        public void Constructor_WithECParameters_ShouldSetPropertiesCorrectly()
        {
            var parameters = ECDiffieHellman.Create().ExportParameters(true);
            var provider = new KeyPairProvider(parameters);

            Assert.NotNull(provider.PrivateKey);
            Assert.NotNull(provider.PublicKey);
        }

        [Fact]
        public void Constructor_WithPublicKey_ShouldSetPublicKeyProperty()
        {
            using var eCDiffieHellman = ECDiffieHellman.Create();
            var publicKeyInfo = eCDiffieHellman.ExportSubjectPublicKeyInfo();
            var provider = new KeyPairProvider(publicKeyInfo);

            Assert.NotNull(provider.PublicKey);
        }

        [Fact]
        public void Constructor_Default_InitializesKeys()
        {
            var provider = new KeyPairProvider();

            Assert.False(string.IsNullOrEmpty(provider.PrivateKey));
            Assert.False(string.IsNullOrEmpty(provider.PublicKey));
        }

        [Fact]
        public void Constructor_WithSpecificCurve_InitializesKeys()
        {
            var curve = ECCurve.NamedCurves.nistP256;
            var provider = new KeyPairProvider(curve);

            Assert.False(string.IsNullOrEmpty(provider.PrivateKey));
            Assert.False(string.IsNullOrEmpty(provider.PublicKey));
        }

        [Fact]
        public void GetECDiffieHellman_ReturnsInitializedInstance()
        {
            var provider = new KeyPairProvider();
            var eCDiffieHellman = provider.GetECDiffieHellman();

            Assert.NotNull(eCDiffieHellman);
        }

        [Fact]
        public void Dispose_ReleasesECDiffieHellmanResources()
        {
            var provider = new KeyPairProvider();
            var eCDiffieHellman = provider.GetECDiffieHellman();

            provider.Dispose();

            Assert.Throws<ObjectDisposedException>(() => eCDiffieHellman.GenerateKey(ECCurve.NamedCurves.nistP256));
        }
    }
}