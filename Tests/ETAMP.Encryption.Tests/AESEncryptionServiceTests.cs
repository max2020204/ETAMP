#region

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoFixture;
using JetBrains.Annotations;
using Xunit;

#endregion

namespace ETAMP.Encryption.Tests;

[TestSubject(typeof(AESEncryptionService))]
public class AESEncryptionServiceTests
{
    private readonly AESEncryptionService _encryptionService;
    private readonly Fixture _fixture;

    public AESEncryptionServiceTests()
    {
        _fixture = new Fixture();
        _encryptionService = new AESEncryptionService();
    }

    [Fact]
    public async Task EncryptAsync_WhenInputStreamIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var key = _fixture.Create<byte[]>().Take(16).ToArray();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _encryptionService.EncryptAsync(null, key));
    }

    [Fact]
    public async Task EncryptAsync_WhenKeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var inputStream = new MemoryStream(_fixture.CreateMany<byte>(100).ToArray());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _encryptionService.EncryptAsync(inputStream, null));
    }

    [Theory]
    [InlineData(15)]
    [InlineData(17)]
    [InlineData(31)]
    public async Task EncryptAsync_WhenKeyLengthIsInvalid_ThrowsArgumentException(int keyLength)
    {
        // Arrange
        var key = _fixture.Create<byte[]>().Take(keyLength).ToArray();
        var inputStream = new MemoryStream(_fixture.CreateMany<byte>(100).ToArray());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _encryptionService.EncryptAsync(inputStream, key));
    }

    [Fact]
    public async Task EncryptAsync_EncryptsStreamCorrectly()
    {
        // Arrange
        var key = _fixture.CreateMany<byte>(16).ToArray();
        var inputData = _fixture.CreateMany<byte>(100).ToArray();
        var inputStream = new MemoryStream(inputData);

        // Act
        var encryptedStream = await _encryptionService.EncryptAsync(inputStream, key);

        // Assert
        Assert.NotNull(encryptedStream);
        Assert.NotEqual(0, encryptedStream.Length); // Encrypted stream should not be empty

        inputStream.Close();
        encryptedStream.Close();
    }

    [Fact]
    public async Task DecryptAsync_WhenInputStreamIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var key = _fixture.Create<byte[]>().Take(16).ToArray();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _encryptionService.DecryptAsync(null, key));
    }

    [Fact]
    public async Task DecryptAsync_WhenKeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var inputStream = new MemoryStream(_fixture.CreateMany<byte>(100).ToArray());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _encryptionService.DecryptAsync(inputStream, null));
    }

    [Theory]
    [InlineData(15)]
    [InlineData(17)]
    [InlineData(31)]
    public async Task DecryptAsync_WhenKeyLengthIsInvalid_ThrowsArgumentException(int keyLength)
    {
        // Arrange
        var key = _fixture.Create<byte[]>().Take(keyLength).ToArray();
        var inputStream = new MemoryStream(_fixture.CreateMany<byte>(100).ToArray());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _encryptionService.DecryptAsync(inputStream, key));
    }

    [Fact]
    public async Task DecryptAsync_WithInvalidIV_ThrowsCryptographicException()
    {
        // Arrange
        var key = _fixture.CreateMany<byte>(16).ToArray();
        // Create an invalid encrypted stream without IV
        var inputStream = new MemoryStream(_fixture.CreateMany<byte>(100).ToArray());

        // Act & Assert
        await Assert.ThrowsAsync<CryptographicException>(() =>
            _encryptionService.DecryptAsync(inputStream, key));
    }

    [Fact]
    public async Task EncryptAndDecryptAsync_ReturnsOriginalData()
    {
        // Arrange
        var key = _fixture.CreateMany<byte>(32).ToArray(); // Valid 256-bit key
        var originalData = _fixture.CreateMany<byte>(100).ToArray();
        var inputStream = new MemoryStream(originalData);

        // Act
        var encryptedStream = await _encryptionService.EncryptAsync(inputStream, key);
        var decryptedStream = await _encryptionService.DecryptAsync(encryptedStream, key);
        var decryptedData = new byte[decryptedStream.Length];
        await decryptedStream.ReadAsync(decryptedData);

        // Assert
        Assert.Equal(originalData, decryptedData);

        inputStream.Close();
        encryptedStream.Close();
        decryptedStream.Close();
    }

    [Fact]
    public async Task EncryptAsync_WhenInputStreamIsSeekable_ResetsPosition()
    {
        // Arrange
        var key = _fixture.CreateMany<byte>(16).ToArray();
        var inputData = _fixture.CreateMany<byte>(100).ToArray();
        using var inputStream = new MemoryStream(inputData);
        inputStream.Position = 10;

        // Act
        var encryptedStream = await _encryptionService.EncryptAsync(inputStream, key);

        // Assert
        Assert.Equal(0, encryptedStream.Position);
    }
}