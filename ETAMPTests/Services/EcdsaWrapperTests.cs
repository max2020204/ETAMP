using ETAMP.Wrapper;
using System.Security.Cryptography;
using Xunit;

namespace ETAMP.Services.Tests
{
    public class EcdsaWrapperTests
    {
        private readonly EcdsaWrapper _wrapper;

        public EcdsaWrapperTests()
        {
            _wrapper = new EcdsaWrapper();
        }

        [Fact]
        public void CreateECDsaTest_ReturnNotNull()
        {
            var result = _wrapper.CreateECDsa();
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("nistP256", 256)]
        [InlineData("nistP384", 384)]
        [InlineData("nistP521", 521)]
        public void CreateECDsa_WithValidCurve_ReturnsECDsaInstance(string curve, int size)
        {
            ECCurve ecCurve = ECCurve.CreateFromFriendlyName(curve);
            using (var ecdsa = _wrapper.CreateECDsa(ecCurve))
            {
                Assert.NotNull(ecdsa);
                Assert.Equal(ecdsa.KeySize, size);
            }
        }

        [Fact]
        public void CreateECDsa_WithValidPublickey_ReturnSameResult()
        {
            using (ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP521))
            {
                string publicKey = ecdsa.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                                    .Replace("-----END PUBLIC KEY-----", "")
                                                                    .Replace("\n", "");

                var result = _wrapper.CreateECDsa(publicKey, ECCurve.NamedCurves.nistP521);

                Assert.NotNull(result);
                Assert.Equal(result.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                                    .Replace("-----END PUBLIC KEY-----", "")
                                                                    .Replace("\n", ""), publicKey);
                Assert.Equal(result.KeySize, 521);
            }
        }

        [Fact]
        public void CreateECDsa_WithValidPublickeyInbytes_ReturnSameResult()
        {
            using (ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP521))
            {
                string publicKey = ecdsa.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                                    .Replace("-----END PUBLIC KEY-----", "")
                                                                    .Replace("\n", "");

                var result = _wrapper.CreateECDsa(Convert.FromBase64String(publicKey), ECCurve.NamedCurves.nistP521);

                Assert.NotNull(result);
                Assert.Equal(result.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                                    .Replace("-----END PUBLIC KEY-----", "")
                                                                    .Replace("\n", ""), publicKey);
                Assert.Equal(result.KeySize, 521);
            }
        }
    }
}