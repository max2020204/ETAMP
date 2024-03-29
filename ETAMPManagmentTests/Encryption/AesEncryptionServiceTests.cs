﻿using ETAMPManagment.Encryption;
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
        public void Constructor_WhenInitializedWithIV_SetsIVProperty()
        {
            var serviceWithIV = new AesEncryptionService(_iv);

            Assert.Equal(_iv, serviceWithIV.IV);
        }

        [Fact]
        public void Encrypt_WithValidDataAndKey_ReturnsEncryptedData()
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes("Test data");

            byte[] encryptedData = _service.Encrypt(dataToEncrypt, _key);

            Assert.NotNull(encryptedData);
            Assert.NotEqual(dataToEncrypt, encryptedData);
        }

        [Fact]
        public void Encrypt_WhenDataIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _service.Encrypt(null, _key));
        }

        [Fact]
        public void Encrypt_WhenKeyIsNull_ThrowsArgumentNullException()
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes("Test data");

            Assert.Throws<ArgumentNullException>(() => _service.Encrypt(dataToEncrypt, null));
        }

        [Fact]
        public void Decrypt_WithEncryptedDataAndCorrectKey_ReturnsOriginalData()
        {
            byte[] originalData = Encoding.UTF8.GetBytes("TestData");

            var service = new AesEncryptionService(_iv);
            byte[] encryptedData = service.Encrypt(originalData, _key);

            byte[] decryptedData = service.Decrypt(encryptedData, _key);

            Assert.Equal(originalData, decryptedData);
        }

        [Fact]
        public void Decrypt_WhenDataIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _service.Decrypt(null, _key));
        }

        [Fact]
        public void Decrypt_WhenKeyIsNull_ThrowsArgumentNullException()
        {
            byte[] encryptedData = Encoding.UTF8.GetBytes("TestData");

            Assert.Throws<ArgumentNullException>(() => _service.Decrypt(encryptedData, null));
        }

        [Fact]
        public void Decrypt_WithInvalidIV_ThrowsInvalidOperationException()
        {
            AesEncryptionService service = new AesEncryptionService();
            byte[] encryptedData = Encoding.UTF8.GetBytes("TestData");

            Assert.Throws<InvalidOperationException>(() => service.Decrypt(encryptedData, _key));
        }
    }
}