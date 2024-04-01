using ETAMPManagment.Encryption;
using ETAMPManagment.Encryption.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Text;
using Xunit;

namespace ETAMPManagment.Services.Tests
{
    public class EciesEncryptionServiceTests
    {
        private readonly Mock<IKeyExchanger> _keyExchangerMock;
        private readonly Mock<IEncryptionService> _encryptionServiceMock;
        private readonly EciesEncryptionService _eciesEncryptionService;

        public EciesEncryptionServiceTests()
        {
            _keyExchangerMock = new Mock<IKeyExchanger>();
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _eciesEncryptionService = new EciesEncryptionService(_keyExchangerMock.Object, _encryptionServiceMock.Object);
        }

        [Fact]
        public void Encrypt_SharedSecretNullOrEmpty_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Encrypt(""));
        }

        [Fact]
        public void Encrypt_ValidMessage_ReturnsEncryptedMessage()
        {
            byte[] secretKey = { 1, 2, 3 };
            byte[] encryptedData = Encoding.UTF8.GetBytes("encrypted");
            string message = "encrypted";

            _keyExchangerMock.Setup(x => x.GetSharedSecret()).Returns(secretKey);
            _encryptionServiceMock.Setup(x => x.Encrypt(It.IsAny<byte[]>(), secretKey)).Returns(encryptedData);

            var result = _eciesEncryptionService.Encrypt(message);

            Assert.Equal(Base64UrlEncoder.Encode(encryptedData), result);
        }

        [Fact]
        public void Decrypt_SharedSecretNullOrEmpty_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Decrypt("", new byte[] { 1, 2, 3 }));
        }

        [Fact]
        public void Decrypt_ValidEncryptedMessage_ReturnsDecryptedMessage()
        {
            byte[] secretKey = { 1, 2, 3 };
            byte[] decryptedData = Encoding.UTF8.GetBytes("Test message");
            string encryptedMessage = Base64UrlEncoder.Encode(decryptedData);

            _keyExchangerMock.Setup(x => x.GetSharedSecret())
                .Returns(secretKey);
            _encryptionServiceMock.Setup(x => x.Decrypt(decryptedData, It.IsAny<byte[]>()))
                .Returns(decryptedData);

            var result = _eciesEncryptionService.Decrypt(encryptedMessage, [1, 2, 3]);

            Assert.Equal("Test message", result);
        }

        [Fact]
        public void Decrypt_CallsDeriveKeyWithCorrectArguments()
        {
            byte[] publicKey = { 4, 5, 6 };
            byte[] secretKey = { 1, 2, 3 };
            byte[] encryptedData = Encoding.UTF8.GetBytes("encrypted message");
            string base64EncryptedMessage = Base64UrlEncoder.Encode(encryptedData);

            _keyExchangerMock.Setup(x => x.GetSharedSecret()).Returns(secretKey);
            _encryptionServiceMock.Setup(x => x.Decrypt(encryptedData, secretKey)).Returns(Encoding.UTF8.GetBytes("decrypted message"));

            _eciesEncryptionService.Decrypt(base64EncryptedMessage, publicKey);

            _keyExchangerMock.Verify(x => x.DeriveKey(publicKey), Times.Once);
        }
    }
}