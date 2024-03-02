using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Wrapper.Tests
{
    public class EcdhKeyWrapperTests
    {
        private readonly EcdhKeyWrapper _wrapper;

        public EcdhKeyWrapperTests()
        {
            _wrapper = new EcdhKeyWrapper();
        }

        [Fact]
        public void CreateKey_ReturnsNotNullPublicKey()
        {
            var publicKey = _wrapper.CreateKey();

            Assert.NotNull(publicKey);
        }

        [Fact]
        public void DeriveKey_ReturnsNotNull_WhenUsingValidPublicKey()
        {
            var otherWrapper = new EcdhKeyWrapper();
            var publicKey = otherWrapper.CreateKey();

            var derivedKey = _wrapper.DeriveKey(publicKey);

            Assert.NotNull(derivedKey);
            Assert.NotEmpty(derivedKey);
        }

        [Fact]
        public void DeriveKey_ThrowsArgumentNullException_WhenPublicKeyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _wrapper.DeriveKey((byte[])null!));
        }

        [Fact]
        public void Dispose_CallsDisposeOnEcdhInstance()
        {
            var ex = Record.Exception(() => _wrapper.Dispose());

            Assert.Null(ex);
        }

        [Fact]
        public void DeriveKeyHash_ReturnsNonNull_WhenUsingValidPublicKey()
        {
            var ecdh = ECDiffieHellman.Create();
            var otherEcdh = ECDiffieHellman.Create();

            var derivedKey = _wrapper.DeriveKeyHash(otherEcdh.PublicKey, HashAlgorithmName.SHA256, null, null);

            Assert.NotNull(derivedKey);
            Assert.NotEmpty(derivedKey);
        }

        [Fact]
        public void DeriveKeyHmac_ReturnsNonNull_WhenUsingValidPublicKeyAndHmacKey()
        {
            var ecdh = ECDiffieHellman.Create();
            var otherEcdh = ECDiffieHellman.Create();
            var hmacKey = new byte[] { 1, 2, 3 };

            var derivedKey = _wrapper.DeriveKeyHmac(otherEcdh.PublicKey, HashAlgorithmName.SHA256, hmacKey, null, null);

            Assert.NotNull(derivedKey);
            Assert.NotEmpty(derivedKey);
        }

        [Fact]
        public void DeriveKey_ReturnsNonNull_WhenUsingValidPublicKey()
        {
            var otherEcdh = ECDiffieHellman.Create();
            var wrapper = new EcdhKeyWrapper();
            var otherPartyPublicKey = otherEcdh.PublicKey.ToByteArray();

            var derivedKey = wrapper.DeriveKey(otherPartyPublicKey);

            Assert.NotNull(derivedKey);
            Assert.NotEmpty(derivedKey);
        }

        [Fact]
        public void Constructor_WithECParameters_InitializesKeysCorrectly()
        {
            using var ecdh = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);
            var parameters = ecdh.ExportParameters(true);

            var wrapper = new EcdhKeyWrapper(parameters);

            Assert.NotNull(wrapper.PrivateKey);
            Assert.NotNull(wrapper.PublicKey);
            Assert.StartsWith("-----BEGIN PRIVATE KEY-----", wrapper.PrivateKey);
            Assert.StartsWith("-----BEGIN PUBLIC KEY-----", wrapper.PublicKey);
        }

        [Fact]
        public void Constructor_WithECCurve_InitializesKeysCorrectly()
        {
            var curve = ECCurve.NamedCurves.nistP256;

            var wrapper = new EcdhKeyWrapper(curve);

            Assert.NotNull(wrapper.PrivateKey);
            Assert.NotNull(wrapper.PublicKey);
            Assert.StartsWith("-----BEGIN PRIVATE KEY-----", wrapper.PrivateKey);
            Assert.StartsWith("-----BEGIN PUBLIC KEY-----", wrapper.PublicKey);
        }
    }
}