using Moq;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace ETAMPManagment.Services.Tests
{
    public class AesEncryptionServiceTests
    {
        private readonly AesEncryptionService _service;

        public AesEncryptionServiceTests()
        {
            _service = new AesEncryptionService();
        }

        [Fact]
        public void Constructor_WithIV_SetsIVProperty()
        {
            byte[] iv = new byte[16];
            new Random().NextBytes(iv);

            var aes = new AesEncryptionService(iv);

            Assert.Equal(iv, aes.IV);
        }

        [Fact]
        public void Encrypt_WithDataAndKey_ReturnsEncryptedData()
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes("Test data");
            byte[] key = new byte[32];
            new Random().NextBytes(key);

            byte[] encryptedData = _service.Encrypt(dataToEncrypt, key);

            Assert.NotNull(encryptedData);
            Assert.NotEqual(dataToEncrypt, encryptedData);
        }

        [Fact]
        public void Encrypt_DataNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _service.Encrypt(It.IsAny<byte[]>(), It.IsAny<byte[]>()));
        }

        [Fact]
        public void Encrypt_KeyNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _service.Encrypt(new byte[16], It.IsAny<byte[]>()));
        }

        [Fact]
        public void Decrypt_WithEncryptedDataAndCorrectKeyAndIV_ReturnsOriginalData()
        {
            Aes aes = Aes.Create();
            byte[] data = Encoding.UTF8.GetBytes("TestData");
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] encypt = encryptor.TransformFinalBlock(data, 0, data.Length);

            AesEncryptionService service = new AesEncryptionService(aes.IV);
            byte[] result = service.Decrypt(encypt, aes.Key);

            Assert.Equal(data, result);
        }

        [Fact]
        public void Decrypt_DataNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _service.Decrypt(It.IsAny<byte[]>(), It.IsAny<byte[]>()));
        }

        [Fact]
        public void Decrypt_KeyNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _service.Decrypt(new byte[16], It.IsAny<byte[]>()));
        }

        [Fact]
        public void Decrypt_IVNull_ThrowsArgumentNullException()
        {
            Assert.Throws<InvalidOperationException>(() => _service.Decrypt(new byte[16], new byte[16]));
        }
    }
}