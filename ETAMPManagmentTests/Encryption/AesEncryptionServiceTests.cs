using ETAMPManagment.Encryption;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace ETAMPManagmentTests.Encryption
{
    public class AesEncryptionServiceTests
    {
        private readonly AesEncryptionService _service;
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public AesEncryptionServiceTests()
        {
            _service = new AesEncryptionService();
            _key = new byte[32];
            new Random().NextBytes(_key);
            _iv = new byte[16];
            new Random().NextBytes(_iv);
        }

        [Fact]
        public void Encrypt_WithValidDataAndKey_ReturnsEncryptedData()
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes("Test data");

            byte[] encryptedData = _service.Encrypt(dataToEncrypt, _key, null);

            Assert.NotNull(encryptedData);
            Assert.NotEqual(dataToEncrypt, encryptedData);
        }

        [Fact]
        public void Encrypt_WhenDataIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _service.Encrypt(null, _key, null));
        }

        [Fact]
        public void Encrypt_WhenKeyIsNull_ThrowsArgumentNullException()
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes("Test data");

            Assert.Throws<ArgumentNullException>(() => _service.Encrypt(dataToEncrypt, null, null));
        }

        [Fact]
        public void Decrypt_WithEncryptedDataAndCorrectKey_ReturnsOriginalData()
        {
            byte[] originalData = Encoding.UTF8.GetBytes("TestData");

            var service = new AesEncryptionService();
            byte[] encryptedData = service.Encrypt(originalData, _key, null);

            byte[] decryptedData = service.Decrypt(encryptedData, _key, service.IV);

            Assert.Equal(originalData, decryptedData);
        }

        [Fact]
        public void Decrypt_WhenDataIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _service.Decrypt(null, _key, _service.IV));
        }

        [Fact]
        public void Decrypt_WhenKeyIsNull_ThrowsArgumentNullException()
        {
            byte[] encryptedData = Encoding.UTF8.GetBytes("TestData");

            Assert.Throws<ArgumentNullException>(() => _service.Decrypt(encryptedData, null, _service.IV));
        }

        [Fact]
        public void Decrypt_WithWrongKey_Fails()
        {
            byte[] originalData = Encoding.UTF8.GetBytes("Sensitive data");
            byte[] wrongKey = new byte[32];
            new Random().NextBytes(wrongKey);

            var service = new AesEncryptionService();
            byte[] encryptedData = service.Encrypt(originalData, _key, null);

            Assert.Throws<CryptographicException>(() => service.Decrypt(encryptedData, wrongKey, service.IV));
        }

        [Fact]
        public void EncryptDecrypt_LargeData_RetainsDataIntegrity()
        {
            byte[] largeData = new byte[10 * 1024 * 1024];
            new Random().NextBytes(largeData);

            var service = new AesEncryptionService();
            byte[] encryptedData = service.Encrypt(largeData, _key, null);
            byte[] decryptedData = service.Decrypt(encryptedData, _key, service.IV);

            Assert.Equal(largeData, decryptedData);
        }
    }
}