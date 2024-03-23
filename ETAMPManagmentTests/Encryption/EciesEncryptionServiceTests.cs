using ETAMPManagment.Encryption;
using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Factories.Interfaces;
using Moq;
using System.Text;
using Xunit;

namespace ETAMPManagment.Services.Tests
{
    public class EciesEncryptionServiceTests
    {
        private readonly Mock<IKeyExchanger> _keyExchangerMock;
        private readonly Mock<IEncryptionServiceFactory> _encryptionServiceFactoryMock;
        private readonly Mock<IEncryptionService> _encryptionServiceMock;
        private readonly EciesEncryptionService _eciesEncryptionService;

        public EciesEncryptionServiceTests()
        {
            _encryptionServiceFactoryMock = new Mock<IEncryptionServiceFactory>();
            _keyExchangerMock = new Mock<IKeyExchanger>();
            _encryptionServiceMock = new Mock<IEncryptionService>();

            _encryptionServiceFactoryMock.Setup(x => x.CreateEncryptionService("AES")).Returns(_encryptionServiceMock.Object);
            _eciesEncryptionService = new EciesEncryptionService(_keyExchangerMock.Object, _encryptionServiceFactoryMock.Object, "AES");
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

            Assert.Equal(Convert.ToBase64String(encryptedData), result);
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
            string encryptedMessage = Convert.ToBase64String(decryptedData);

            _keyExchangerMock.Setup(x => x.GetSharedSecret())
                .Returns(secretKey);
            _encryptionServiceMock.Setup(x => x.Decrypt(decryptedData, It.IsAny<byte[]>()))
                .Returns(decryptedData);

            var result = _eciesEncryptionService.Decrypt(encryptedMessage, [1, 2, 3]);

            Assert.Equal("Test message", result);
        }

        [Fact]
        public void Decrypt_WithInvalidBase64_ThrowsFormatException()
        {
            string invalidBase64 = "invalid_base64";
            byte[] publicKey = [1, 2, 3];
            _keyExchangerMock.Setup(x => x.GetSharedSecret())
                    .Returns(publicKey);
            var exception = Assert.Throws<FormatException>(() => _eciesEncryptionService.Decrypt(invalidBase64, publicKey));
            Assert.Contains("The encrypted message is not in a valid Base64 format.", exception.Message);
        }
    }
}