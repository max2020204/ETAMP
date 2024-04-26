using System.Text;
using ETAMPManagment.Encryption;
using ETAMPManagment.Encryption.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace ETAMPManagment.Services.Tests;

public class EciesEncryptionServiceTests
{
    private readonly Mock<IEncryptionService> _encryptionServiceMock;
    private readonly Mock<IKeyExchanger> _keyExchangerMock;
    private readonly EciesEncryptionService _eciesEncryptionService;

    public EciesEncryptionServiceTests()
    {
        _keyExchangerMock = new Mock<IKeyExchanger>();
        _encryptionServiceMock = new Mock<IEncryptionService>();
        _eciesEncryptionService = new EciesEncryptionService();
        _eciesEncryptionService.Initialize(_keyExchangerMock.Object, _encryptionServiceMock.Object);
    }

    [Fact]
    public void Encrypt_ShouldThrowInvalidOperationException_WhenSharedSecretIsEmpty()
    {
        // Arrange
        _keyExchangerMock.Setup(x => x.GetSharedSecret()).Returns(Array.Empty<byte>());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Encrypt("Hello World"));
    }

    [Fact]
    public void Encrypt_ShouldReturnEncryptedMessage()
    {
        // Arrange
        var message = "Hello World";
        var sharedSecret = Encoding.UTF8.GetBytes("secret");
        var encryptedData = Encoding.UTF8.GetBytes("encrypted");
        _keyExchangerMock.Setup(x => x.GetSharedSecret()).Returns(sharedSecret);
        _encryptionServiceMock.Setup(x => x.Encrypt(It.IsAny<byte[]>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(encryptedData);
        _encryptionServiceMock.Setup(x => x.IV).Returns(new byte[16]); // Initialization vector

        // Act
        var result = _eciesEncryptionService.Encrypt(message);

        // Assert
        Assert.Equal(Base64UrlEncoder.Encode(encryptedData), result);
    }

    [Fact]
    public void Decrypt_ShouldThrowInvalidOperationException_WhenSharedSecretIsEmpty()
    {
        // Arrange
        _keyExchangerMock.Setup(x => x.GetSharedSecret()).Returns(Array.Empty<byte>());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Decrypt("invalid_base64"));
    }

    [Fact]
    public void Decrypt_ShouldReturnDecryptedMessage()
    {
        var message = Encoding.UTF8.GetBytes("encrypted_message");
        var encryptedMessageBase64 = Base64UrlEncoder.Encode(message);
        var sharedSecret = Encoding.UTF8.GetBytes("secret");
        var data = "Hello World";
        var decryptedData = Encoding.UTF8.GetBytes(data);

        _keyExchangerMock.Setup(x => x.GetSharedSecret()).Returns(sharedSecret);
        _encryptionServiceMock.Setup(x => x.IV).Returns(new byte[16]);
        _encryptionServiceMock.Setup(x => x.Decrypt(message, sharedSecret, new byte[16]))
            .Returns(decryptedData);

        var result = _eciesEncryptionService.Decrypt(encryptedMessageBase64);

        Assert.Equal(data, result);
    }
}