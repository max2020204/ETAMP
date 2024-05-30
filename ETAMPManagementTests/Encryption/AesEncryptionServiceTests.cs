using System.Security.Cryptography;
using System.Text;
using ETAMPManagement.Encryption;
using Xunit;

namespace ETAMPManagementTests.Encryption;

public class AesEncryptionServiceTests
{
    private readonly byte[] _iv;
    private readonly byte[] _key;
    private readonly AESEncryptionService _service;

    public AesEncryptionServiceTests()
    {
        _service = new AESEncryptionService();
        _key = new byte[32];
        new Random().NextBytes(_key);
        _iv = new byte[16];
        new Random().NextBytes(_iv);
    }

    [Fact]
    public void Encrypt_WithValidDataAndKey_ReturnsEncryptedData()
    {
        var dataToEncrypt = Encoding.UTF8.GetBytes("Test data");

        var encryptedData = _service.Encrypt(dataToEncrypt, _key, null);

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
        var dataToEncrypt = Encoding.UTF8.GetBytes("Test data");

        Assert.Throws<ArgumentNullException>(() => _service.Encrypt(dataToEncrypt, null, null));
    }

    [Fact]
    public void Decrypt_WithEncryptedDataAndCorrectKey_ReturnsOriginalData()
    {
        var originalData = Encoding.UTF8.GetBytes("TestData");

        var service = new AESEncryptionService();
        var encryptedData = service.Encrypt(originalData, _key, null);

        var decryptedData = service.Decrypt(encryptedData, _key, service.IV);

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
        var encryptedData = Encoding.UTF8.GetBytes("TestData");

        Assert.Throws<ArgumentNullException>(() => _service.Decrypt(encryptedData, null, _service.IV));
    }

    [Fact]
    public void Decrypt_WithWrongKey_Fails()
    {
        var originalData = Encoding.UTF8.GetBytes("Sensitive data");
        var wrongKey = new byte[32];
        new Random().NextBytes(wrongKey);

        var service = new AESEncryptionService();
        var encryptedData = service.Encrypt(originalData, _key, null);

        Assert.Throws<CryptographicException>(() => service.Decrypt(encryptedData, wrongKey, service.IV));
    }

    [Fact]
    public void EncryptDecrypt_LargeData_RetainsDataIntegrity()
    {
        var largeData = new byte[10 * 1024 * 1024];
        new Random().NextBytes(largeData);

        var service = new AESEncryptionService();
        var encryptedData = service.Encrypt(largeData, _key, null);
        var decryptedData = service.Decrypt(encryptedData, _key, service.IV);

        Assert.Equal(largeData, decryptedData);
    }
}