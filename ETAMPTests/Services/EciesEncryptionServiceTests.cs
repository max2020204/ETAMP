using ETAMP.Services.Interfaces;
using ETAMP.Wrapper.Interfaces;
using Moq;
using System.Text;
using Xunit;

namespace ETAMP.Services.Tests
{
    public class EciesEncryptionServiceTests
    {
        private readonly Mock<IEcdhKeyWrapper> _ecdhKeyWrapperMock;
        private readonly Mock<IEncryptionService> _encryptionServiceMock;
        private readonly Mock<IEncryptionServiceFactory> _encryptionServiceFactoryMock;
        private readonly EciesEncryptionService _eciesEncryptionService;

        public EciesEncryptionServiceTests()
        {
            _ecdhKeyWrapperMock = new Mock<IEcdhKeyWrapper>();
            _encryptionServiceFactoryMock = new Mock<IEncryptionServiceFactory>();
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _encryptionServiceFactoryMock.Setup(m => m.CreateEncryptionService("AES"))
                                        .Returns(_encryptionServiceMock.Object);
            _eciesEncryptionService = new EciesEncryptionService(_ecdhKeyWrapperMock.Object, _encryptionServiceFactoryMock.Object, "AES");
        }

        [Fact]
        public void Encrypt_ValidMessage_ReturnsEncryptedMessage()
        {
            byte[] secretKey = [1, 2, 3];
            byte[] encryptedData = Encoding.UTF8.GetBytes("encrypted");

            _ecdhKeyWrapperMock.Setup(m => m.KeyExchanger)
                .Returns(secretKey);

            _encryptionServiceMock.Setup(m => m.Encrypt(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(encryptedData);

            var result = _eciesEncryptionService.Encrypt("Test message");

            Assert.Equal(Convert.ToBase64String(encryptedData), result);
        }

        [Fact]
        public void Encrypt_EcdhKeyWrapperIsNull_ThrowInvalidOperationException()
        {
            _ecdhKeyWrapperMock.Setup(x => x.KeyExchanger).Returns(new byte[0]);
            Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Encrypt("test"));
        }

        [Fact]
        public void Decrypt_ValidEncryptedMessage_ReturnsDecryptedMessage()
        {
            byte[] secretKey = { 1, 2, 3 };
            byte[] decryptedData = Encoding.UTF8.GetBytes("Test message");
            byte[] publicKey = { 4, 5, 6 };
            string encryptedMessageBase64 = Convert.ToBase64String(decryptedData); // Имитация зашифрованного сообщения

            _ecdhKeyWrapperMock.Setup(m => m.KeyExchanger).Returns(secretKey);
            _ecdhKeyWrapperMock.Setup(m => m.DeriveKey(It.IsAny<byte[]>())).Returns(secretKey);
            _encryptionServiceMock.Setup(m => m.Decrypt(It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(decryptedData);

            var result = _eciesEncryptionService.Decrypt(encryptedMessageBase64, publicKey);

            Assert.Equal("Test message", result);
        }

        [Fact]
        public void Decrypt_WithInvalidBase64_ThrowsFormatException()
        {
            byte[] publicKey = { 4, 5, 6 };
            string invalidBase64 = "invalid_base64";
            byte[] secretKey = { 1, 2, 3 };
            _ecdhKeyWrapperMock.Setup(m => m.KeyExchanger).Returns(secretKey);
            Assert.Throws<FormatException>(() => _eciesEncryptionService.Decrypt(invalidBase64, publicKey));
        }

        [Fact]
        public void Decrypt_EcdhKeyWrapperNotInitialized_ThrowsInvalidOperationException()
        {
            _ecdhKeyWrapperMock.Setup(x => x.KeyExchanger).Returns(value: null);
            byte[] publicKey = { 4, 5, 6 };
            string encryptedMessageBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("dummy"));

            Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Decrypt(encryptedMessageBase64, publicKey));
        }

        [Fact]
        public void Decrypt_KeyExchangerIsEmpty_ThrowsInvalidOperationException()
        {
            _ecdhKeyWrapperMock.Setup(x => x.KeyExchanger).Returns(new byte[0]);
            byte[] publicKey = { 4, 5, 6 };
            string encryptedMessageBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("dummy"));

            Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Decrypt(encryptedMessageBase64, publicKey));
        }
    }
}