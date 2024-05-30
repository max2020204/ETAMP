using ETAMPManagement.Encryption;
using ETAMPManagement.Encryption.Base;
using ETAMPManagement.Encryption.Interfaces;
using Moq;
using Xunit;

namespace ETAMPManagementTests.Encryption;

public class ECIESEncryptionServiceTest
{
    private readonly ECIESEncryptionService _eciesEncryptionService;
    private readonly Mock<IEncryptionService> _mockEncryptionService;
    private readonly Mock<KeyExchangerBase> _mockKeyExchanger;

    public ECIESEncryptionServiceTest()
    {
        _eciesEncryptionService = new ECIESEncryptionService();
        _mockKeyExchanger = new Mock<KeyExchangerBase>();
        _mockEncryptionService = new Mock<IEncryptionService>();
    }

    [Fact]
    public void Encrypt_ThrowsException_WhenNotInitialized()
    {
        Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Encrypt("test message"));
    }

    [Fact]
    public void Decrypt_ThrowsException_WhenNotInitialized()
    {
        Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Decrypt("test encrypted message"));
    }

    [Fact]
    public void Encrypt_ThrowsException_WhenSharedSecretInvalid()
    {
        _eciesEncryptionService.Initialize(_mockKeyExchanger.Object, _mockEncryptionService.Object);
        _mockKeyExchanger.Setup(m => m.GetSharedSecret()).Returns((byte[])null);

        Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Encrypt("test message"));
    }

    [Fact]
    public void Decrypt_ThrowsException_WhenSharedSecretInvalid()
    {
        _eciesEncryptionService.Initialize(_mockKeyExchanger.Object, _mockEncryptionService.Object);
        _mockKeyExchanger.Setup(m => m.GetSharedSecret()).Returns((byte[])null);

        Assert.Throws<InvalidOperationException>(() => _eciesEncryptionService.Decrypt("test encrypted message"));
    }

    [Fact]
    public void Encrypt_ReturnsValidEncryptedString_WhenValidInput()
    {
        _eciesEncryptionService.Initialize(_mockKeyExchanger.Object, _mockEncryptionService.Object);
        _mockKeyExchanger.Setup(m => m.GetSharedSecret()).Returns(new byte[] { 1, 2, 3 });

        var encryptedMessageBytes = new byte[] { 1, 2, 3, 4, 5 };
        _mockEncryptionService.Setup(m => m.Encrypt(It.IsAny<byte[]>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(encryptedMessageBytes);

        var encryptedMessage = _eciesEncryptionService.Encrypt("test message");

        Assert.NotNull(encryptedMessage);
        Assert.NotEmpty(encryptedMessage);

        _mockEncryptionService.Verify(m => m.Encrypt(It.IsAny<byte[]>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()),
            Times.Once);
    }


    [Fact]
    public void Decrypt_ReturnsValidDecryptedString_WhenValidInput()
    {
        _eciesEncryptionService.Initialize(_mockKeyExchanger.Object, _mockEncryptionService.Object);
        _mockKeyExchanger.Setup(m => m.GetSharedSecret()).Returns(new byte[] { 1, 2, 3 });

        var encryptedMessageBase64 = "dGVzdCBtZXNzYWdl";

        var decryptedMessageBytes = "test message"u8.ToArray();
        _mockEncryptionService.Setup(m => m.Decrypt(It.IsAny<byte[]>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(decryptedMessageBytes);

        var decryptedMessage = _eciesEncryptionService.Decrypt(encryptedMessageBase64);

        Assert.NotNull(decryptedMessage);
        Assert.Equal("test message", decryptedMessage);

        _mockEncryptionService.Verify(m => m.Decrypt(It.IsAny<byte[]>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()),
            Times.Once);
    }
}