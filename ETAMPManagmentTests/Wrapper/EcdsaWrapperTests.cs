using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Wrapper.Tests
{
    public class EcdsaWrapperTests
    {
        private readonly EcdsaWrapper _wrapper;

        public EcdsaWrapperTests()
        {
            _wrapper = new EcdsaWrapper();
        }

        [Fact]
        public void CreateDefaultECDsa_ShouldReturnNonNullInstance()
        {
            var result = _wrapper.CreateECDsa();
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("nistP256", 256)]
        [InlineData("nistP384", 384)]
        [InlineData("nistP521", 521)]
        public void CreateECDsa_UsingSpecifiedCurve_ShouldReturnInstanceWithCorrectKeySize(string curve, int size)
        {
            ECCurve ecCurve = ECCurve.CreateFromFriendlyName(curve);
            using var ecdsa = _wrapper.CreateECDsa(ecCurve);
            Assert.NotNull(ecdsa);
            Assert.Equal(ecdsa.KeySize, size);
        }

        [Fact]
        public void CreateECDsa_FromPemPublicKey_ShouldCorrectlyImportPublicKey()
        {
            using ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP521);
            string publicKey = ecdsa.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                                .Replace("-----END PUBLIC KEY-----", "")
                                                                .Replace("\n", "");

            var result = _wrapper.CreateECDsa(publicKey, ECCurve.NamedCurves.nistP521);

            Assert.NotNull(result);
            Assert.Equal(result.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                                .Replace("-----END PUBLIC KEY-----", "")
                                                                .Replace("\n", ""), publicKey);
            Assert.Equal(521, result.KeySize);
        }

        [Fact]
        public void CreateECDsa_FromPublicKeyBytes_ShouldCorrectlyImportPublicKey()
        {
            using ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP521);
            string publicKey = ecdsa.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                                .Replace("-----END PUBLIC KEY-----", "")
                                                                .Replace("\n", "");

            var result = _wrapper.CreateECDsa(Convert.FromBase64String(publicKey), ECCurve.NamedCurves.nistP521);

            Assert.NotNull(result);
            Assert.Equal(result.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                                .Replace("-----END PUBLIC KEY-----", "")
                                                                .Replace("\n", ""), publicKey);

            Assert.Equal(521, result.KeySize);
        }

        [Fact]
        public void StripPEMFormatting_FromPrivateKey_ShouldReturnBase64String()
        {
            ECDsa ecdsa = ECDsa.Create();
            string key = ecdsa.ExportECPrivateKeyPem();
            string clearKey = key.Replace("-----BEGIN PRIVATE KEY-----", "")
                             .Replace("-----END PRIVATE KEY-----", "")
                             .Replace("\n", "")
                             .Replace("\r", "");
            string result = _wrapper.ClearPEMPrivateKey(key);

            Assert.Equal(clearKey, result);
        }

        [Fact]
        public void StripPEMFormatting_FromPublicKey_ShouldReturnBase64String()
        {
            ECDsa ecdsa = ECDsa.Create();
            string key = ecdsa.ExportSubjectPublicKeyInfoPem();
            string clearKey = key.Replace("-----BEGIN PUBLIC KEY-----", "")
                            .Replace("-----END PUBLIC KEY-----", "")
                            .Replace("\n", "")
                            .Replace("\r", "");
            string result = _wrapper.ClearPEMPublicKey(key);

            Assert.Equal(clearKey, result);
        }

        [Fact]
        public void ImportECDsa_FromPemPrivateKey_ShouldCorrectlyImportPrivateKey()
        {
            ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP521);
            string key = ecdsa.ExportPkcs8PrivateKeyPem();
            ECDsa result = _wrapper.ImportECDsa(key, ECCurve.NamedCurves.nistP521);

            Assert.Equal(ecdsa.ExportECPrivateKeyPem(), result.ExportECPrivateKeyPem());
        }

        [Fact]
        public void CreateECDsa_FromBase64PublicKey_ShouldCorrectlyImportPublicKey()
        {
            ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP521);
            string key = ecdsa.ExportSubjectPublicKeyInfoPem();
            ECDsa result = _wrapper.CreateECDsa(key, ECCurve.NamedCurves.nistP521);

            Assert.Equal(ecdsa.ExportSubjectPublicKeyInfoPem(), result.ExportSubjectPublicKeyInfoPem());
        }

        [Fact]
        public void ImportECDsa_ReturnsECDsaInstanceWithGivenPrivateKeyAndCurve()
        {
            var curve = ECCurve.NamedCurves.nistP256;
            ECDsa ecdsa = ECDsa.Create();
            byte[] key = ecdsa.ExportPkcs8PrivateKey();
            var result = _wrapper.ImportECDsa(key, curve);
            Assert.NotNull(result);
            Assert.Equal(result.ExportPkcs8PrivateKey(), ecdsa.ExportPkcs8PrivateKey());
        }
    }
}