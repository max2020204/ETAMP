using ETAMPManagment.Encryption.Interfaces;
using Moq;
using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Encryption.Tests
{
    public class KeyExchangerTests
    {
        private readonly Mock<IKeyPairProvider> _keyProviderMock;
        private readonly KeyExchanger _exchanger;
        private Mock<ECDiffieHellman> _mockECDiffieHellman;

        public KeyExchangerTests()
        {
            _keyProviderMock = new Mock<IKeyPairProvider>();
            _mockECDiffieHellman = new Mock<ECDiffieHellman>();
            _keyProviderMock.Setup(x => x.GetECDiffieHellman()).Returns(_mockECDiffieHellman.Object);
            _exchanger = new KeyExchanger(_keyProviderMock.Object);
        }

        [Fact()]
        public void DeriveKeyHashTest()
        {
        }

        [Fact()]
        public void DeriveKeyHash_ThrowsArgumentNullException_WhenPublicKeyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _exchanger.DeriveKeyHash(null, HashAlgorithmName.SHA256, null, null));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenKeyPairProviderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new KeyExchanger(null));
        }

        [Fact]
        public void DeriveKey_FromRawPublicKey_ThrowsArgumentNullException_WhenPublicKeyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _exchanger.DeriveKey(otherPartyPublicKey: null));
        }

        [Fact]
        public void DeriveKey_ThrowsArgumentNullException_WhenPublicKeyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _exchanger.DeriveKey(publicKey: null));
        }

        [Fact]
        public void DeriveKeyHmac_ThrowsArgumentNullException_WhenPublicKeyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _exchanger.DeriveKeyHmac(null, HashAlgorithmName.SHA256, new byte[0], null, null));
        }

        [Fact()]
        public void DeriveKey_WithValidPublicKey_ReturnsExpectedDerivedKey()
        {
            byte[] data = { 1, 2, 3 };
            Mock<ECDiffieHellmanPublicKey> mp = new Mock<ECDiffieHellmanPublicKey>();

            ECDiffieHellmanPublicKey publicKey = mp.Object;
            _mockECDiffieHellman.Setup(x => x.DeriveKeyMaterial(publicKey)).Returns(data);
            var result = _exchanger.DeriveKey(publicKey);

            Assert.Equal(data, result);
        }

        [Fact]
        public void DeriveKeyHash_ReturnsCorrectDerivedKey()
        {
            var dummyPublicKey = new Mock<ECDiffieHellmanPublicKey>().Object;
            byte[] expectedKey = { 1, 2, 3, 4 };
            byte[] prependData = { 5, 6 };
            byte[] appendData = { 7, 8 };
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

            _mockECDiffieHellman.Setup(m => m.DeriveKeyFromHash(dummyPublicKey, hashAlgorithm, prependData, appendData)).Returns(expectedKey);

            var result = _exchanger.DeriveKeyHash(dummyPublicKey, hashAlgorithm, prependData, appendData);

            Assert.Equal(expectedKey, result);
        }

        [Fact]
        public void DeriveKeyHmac_ReturnsCorrectDerivedKey()
        {
            var dummyPublicKey = new Mock<ECDiffieHellmanPublicKey>().Object;
            byte[] expectedKey = { 9, 10, 11, 12 };
            byte[] hmacKey = { 13, 14 };
            byte[] prependData = { 15, 16 };
            byte[] appendData = { 17, 18 };
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

            _mockECDiffieHellman.Setup(m => m.DeriveKeyFromHmac(dummyPublicKey, hashAlgorithm, hmacKey, prependData, appendData)).Returns(expectedKey);

            var result = _exchanger.DeriveKeyHmac(dummyPublicKey, hashAlgorithm, hmacKey, prependData, appendData);

            Assert.Equal(expectedKey, result);
        }

        [Fact]
        public void GetSharedSecret_ReturnsExpectedSecret_AfterKeyDerivation()
        {
            byte[] expectedSecret = { 19, 20, 21, 22 };
            var dummyPublicKey = new Mock<ECDiffieHellmanPublicKey>().Object;
            _mockECDiffieHellman.Setup(m => m.DeriveKeyMaterial(dummyPublicKey)).Returns(expectedSecret);

            // Perform a key derivation to set the _sharedSecret
            _exchanger.DeriveKey(dummyPublicKey);

            var sharedSecret = _exchanger.GetSharedSecret();

            Assert.Equal(expectedSecret, sharedSecret);
        }

        [Fact]
        public void DeriveKey_WithValidPublicKeyArray_CallsDeriveKeyMaterial()
        {
            // Arrange
            byte[] expectedDerivedKeyMaterial = { 1, 2, 3, 4 };

            ECDiffieHellman eC = ECDiffieHellman.Create();
            byte[] rawPublicKey = Convert.FromBase64String(eC.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                             .Replace("-----END PUBLIC KEY-----", "")
                                                             .Replace("\n", "")
                                                             .Replace("\r", ""));
            _mockECDiffieHellman.Setup(ecdh => ecdh.DeriveKeyMaterial(It.IsAny<ECDiffieHellmanPublicKey>()))
                    .Returns(expectedDerivedKeyMaterial);
            var derivedKeyMaterial = _exchanger.DeriveKey(rawPublicKey);

            Assert.Equal(expectedDerivedKeyMaterial, derivedKeyMaterial);
        }
    }
}