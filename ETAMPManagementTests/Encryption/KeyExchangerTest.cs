using System.Security.Cryptography;
using ETAMPManagement.Encryption;
using ETAMPManagement.Encryption.Base;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace ETAMPManagementTests.Encryption;

[TestSubject(typeof(KeyExchanger))]
public class KeyExchangerTest
{
    private readonly KeyExchanger _keyExchanger;
    private readonly Mock<KeyPairProviderBase> _mockKeyPairProvider;

    public KeyExchangerTest()
    {
        _mockKeyPairProvider = new Mock<KeyPairProviderBase>(MockBehavior.Strict);
        _mockKeyPairProvider.Setup(x => x.GetECDiffieHellman())
            .Returns(() => ECDiffieHellman.Create());
        _keyExchanger = new KeyExchanger();
        _keyExchanger.Initialize(_mockKeyPairProvider.Object);
    }

    [Fact]
    public void DeriveKeyHash_ValidInput_ReturnsDerivedKey()
    {
        // Arrange
        var publicKey = ECDiffieHellman.Create().PublicKey;
        var hashAlgorithm = HashAlgorithmName.SHA256;
        var secretPrepend = new byte[] { 1, 2, 3 };
        var secretAppend = new byte[] { 4, 5, 6 };
        var expectedKey = new byte[] { 7, 8, 9 };

        _mockKeyPairProvider
            .Setup(x => x.GetECDiffieHellman().DeriveKeyFromHash(publicKey, hashAlgorithm, secretPrepend, secretAppend))
            .Returns(expectedKey);

        // Act
        var derivedKey = _keyExchanger.DeriveKeyHash(publicKey, hashAlgorithm, secretPrepend, secretAppend);

        // Assert
        Assert.Equal(expectedKey, derivedKey);
    }

    [Fact]
    public void DeriveKey_ValidInput_ReturnsDerivedKey()
    {
        // Arrange
        var publicKey = ECDiffieHellman.Create().PublicKey;
        var expectedKey = new byte[] { 7, 8, 9 };

        _mockKeyPairProvider
            .Setup(x => x.GetECDiffieHellman().DeriveKeyMaterial(publicKey))
            .Returns(expectedKey);

        // Act
        var derivedKey = _keyExchanger.DeriveKey(publicKey);

        // Assert
        Assert.Equal(expectedKey, derivedKey);
    }

    [Fact]
    public void DeriveKey_ByteArrayValidInput_ReturnsDerivedKey()
    {
        // Arrange
        using var ecdhOtherParty = ECDiffieHellman.Create();
        var otherPartyPublicKey = ecdhOtherParty.ExportSubjectPublicKeyInfo(); // Экспортируем корректный публичный ключ
        var expectedKey = new byte[] { 7, 8, 9 };

        _mockKeyPairProvider
            .Setup(x => x.GetECDiffieHellman().DeriveKeyMaterial(It.IsAny<ECDiffieHellmanPublicKey>()))
            .Returns(expectedKey);

        // Act
        var derivedKey = _keyExchanger.DeriveKey(otherPartyPublicKey);

        // Assert
        Assert.Equal(expectedKey, derivedKey);
    }

    [Fact]
    public void DeriveKeyHmac_ValidInput_ReturnsDerivedKey()
    {
        // Arrange
        var publicKey = ECDiffieHellman.Create().PublicKey;
        var hashAlgorithm = HashAlgorithmName.SHA256;
        var hmacKey = new byte[] { 1, 2, 3 };
        var secretPrepend = new byte[] { 4, 5, 6 };
        var secretAppend = new byte[] { 7, 8, 9 };
        var expectedKey = new byte[] { 10, 11, 12 };

        _mockKeyPairProvider
            .Setup(x => x.GetECDiffieHellman()
                .DeriveKeyFromHmac(publicKey, hashAlgorithm, hmacKey, secretPrepend, secretAppend))
            .Returns(expectedKey);

        // Act
        var derivedKey = _keyExchanger.DeriveKeyHmac(publicKey, hashAlgorithm, hmacKey, secretPrepend, secretAppend);

        // Assert
        Assert.Equal(expectedKey, derivedKey);
    }

    [Fact]
    public void DeriveKeyHash_NullPublicKey_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _keyExchanger.DeriveKeyHash(null, HashAlgorithmName.SHA256, null, null));
    }

    [Fact]
    public void DeriveKey_NullPublicKey_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _keyExchanger.DeriveKey((ECDiffieHellmanPublicKey)null));
    }

    [Fact]
    public void DeriveKey_ByteArrayNullPublicKey_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _keyExchanger.DeriveKey((byte[])null));
    }

    [Fact]
    public void DeriveKeyHmac_NullPublicKey_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _keyExchanger.DeriveKeyHmac(null, HashAlgorithmName.SHA256, null, null, null));
    }
}