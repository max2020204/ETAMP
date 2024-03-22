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
        public void Encrypt_SharedSecretNullOrEmpty_ThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Encrypt(""));
        }

        [Fact]
        public void Encrypt_ValidMessage_ReturnsEncryptedMessage()
        {
            byte[] secretKey = [1, 2, 3];
            byte[] encryptedData = Encoding.UTF8.GetBytes("encrypted");

            _keyExchangerMock.Setup(x => x.GetSharedSecret()).Returns(secretKey);
            _encryptionServiceMock.Setup(x => x.Encrypt(It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(encryptedData);

            var result = _eciesEncryptionService.Encrypt("encrypted");
            Assert.Equal(Convert.ToBase64String(encryptedData), result);
        }

        [Fact]
        public void Decrypt_SharedSecretNullOrEmpty_ThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Decrypt(It.IsAny<string>(), It.IsAny<byte[]>()));
        }

        [Fact]
        public void Decrypt_ValidEncryptedMessage_ReturnsDecryptedMessage()
        {
            byte[] secretKey = { 1, 2, 3 };
            byte[] decryptedData = Encoding.UTF8.GetBytes("Test message");
            string encryptedMessageBase64 = Convert.ToBase64String(decryptedData);

            _keyExchangerMock.Setup(x => x.GetSharedSecret()).Returns(secretKey);
            _keyExchangerMock.Setup(x => x.DeriveKey(It.IsAny<byte[]>())).Returns(secretKey);
            _encryptionServiceMock.Setup(x => x.Decrypt(It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(decryptedData);

            var result = _eciesEncryptionService.Decrypt(encryptedMessageBase64, It.IsAny<byte[]>());

            Assert.Equal("Test message", result);
        }

        [Fact]
        public void Decrypt_WithInvalidBase64_ThrowsFormatException()
        {
            byte[] publicKey = { 4, 5, 6 };
            string invalidBase64 = "invalid_base64";
            byte[] secretKey = { 1, 2, 3 };
            _keyExchangerMock.Setup(x => x.GetSharedSecret()).Returns(secretKey);
            Assert.Throws<FormatException>(() => _eciesEncryptionService.Decrypt(invalidBase64, publicKey));
        }
    }
}