#region

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using ETAMP.Encryption.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

#endregion

namespace ETAMP.Encryption.Tests;

[TestSubject(typeof(ECIESEncryptionService))]
public class ECIESEncryptionServiceTests
{
    private readonly ECIESEncryptionService _eciesEncryptionService;
    private readonly IFixture _fixture;
    private readonly Mock<IEncryptionService> _mockEncryptionService;

    public ECIESEncryptionServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _mockEncryptionService = _fixture.Freeze<Mock<IEncryptionService>>();
        var logger = new Mock<ILogger<ECIESEncryptionService>>();
        _eciesEncryptionService = new ECIESEncryptionService(_mockEncryptionService.Object, logger.Object);
    }

    [Fact]
    public async Task EncryptAsync_UsingPublicKey_StreamIsEncrypted()
    {
        // Arrange
        var message = new MemoryStream(_fixture.Create<byte[]>());
        var privateKey = ECDiffieHellman.Create();
        var publicKey = privateKey.PublicKey;
        var sharedSecret = _fixture.Create<byte[]>();
        var encryptedStream = new MemoryStream();

        _mockEncryptionService
            .Setup(e => e.EncryptAsync(It.IsAny<Stream>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(encryptedStream);

        // Act
        var result = await _eciesEncryptionService.EncryptAsync(message, privateKey, publicKey);

        // Assert
        Assert.Equal(encryptedStream, result);
        _mockEncryptionService.Verify(
            e => e.EncryptAsync(It.IsAny<Stream>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DecryptAsync_UsingPublicKey_StreamIsDecrypted()
    {
        // Arrange
        var encryptedStream = new MemoryStream(_fixture.Create<byte[]>());
        var privateKey = ECDiffieHellman.Create();
        var publicKey = privateKey.PublicKey;
        var sharedSecret = _fixture.Create<byte[]>();
        var decryptedStream = new MemoryStream();

        _mockEncryptionService
            .Setup(e => e.DecryptAsync(It.IsAny<Stream>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(decryptedStream);

        // Act
        var result = await _eciesEncryptionService.DecryptAsync(encryptedStream, privateKey, publicKey);

        // Assert
        Assert.Equal(decryptedStream, result);
        _mockEncryptionService.Verify(
            e => e.DecryptAsync(It.IsAny<Stream>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task EncryptAsync_ThrowsArgumentNullExceptionForNullMessage_Stream()
    {
        // Arrange
        Stream message = null!;
        var privateKey = ECDiffieHellman.Create();
        var publicKey = privateKey.PublicKey;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _eciesEncryptionService.EncryptAsync(message, privateKey, publicKey));
    }

    [Fact]
    public async Task DecryptAsync_ThrowsArgumentNullExceptionForNullMessage_Stream()
    {
        // Arrange
        Stream encryptedMessage = null!;
        var privateKey = ECDiffieHellman.Create();
        var publicKey = privateKey.PublicKey;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _eciesEncryptionService.DecryptAsync(encryptedMessage, privateKey, publicKey));
    }
}