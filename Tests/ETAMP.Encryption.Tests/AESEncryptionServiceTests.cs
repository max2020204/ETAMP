#region

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
        _encryptionService = new AESEncryptionService();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task EncryptAsync_ShouldThrowArgumentNullException_WhenInputStreamIsNull()
    {
        // Arrange
        Stream? inputStream = null;
        var key = _fixture.Create<byte[]>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _encryptionService.EncryptAsync(inputStream, key));
    }

    [Fact]
    public async Task EncryptAsync_ShouldThrowArgumentNullException_WhenKeyIsNull()
    {
        // Arrange
        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("Test data"));
        byte[]? key = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _encryptionService.EncryptAsync(inputStream, key));
    }

    [Fact]
    public async Task DecryptAsync_ShouldThrowArgumentNullException_WhenInputStreamIsNull()
    {
        // Arrange
        Stream? inputStream = null;
        var key = _fixture.Create<byte[]>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _encryptionService.DecryptAsync(inputStream, key));
    }

    [Fact]
    public async Task DecryptAsync_ShouldThrowArgumentNullException_WhenKeyIsNull()
    {
        // Arrange
        var inputStream = new MemoryStream(_fixture.Create<byte[]>());
        byte[]? key = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _encryptionService.DecryptAsync(inputStream, key));
    }

    [Fact]
    public async Task EncryptAsync_ShouldReturnEncryptedStream_WithIV()
    {
        // Arrange
        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("Test data"));
        var key = Aes.Create().Key; // Generate a valid AES key

        // Act
        var encryptedStream = await _encryptionService.EncryptAsync(inputStream, key);

        // Assert
        Assert.NotNull(encryptedStream);
        Assert.True(encryptedStream.Length > 0);

        // Ensure that the initialization vector (IV) was written
        encryptedStream.Position = 0;
        var iv = new byte[16]; // IV is 128 bits (16 bytes) for AES
        var bytesRead = await encryptedStream.ReadAsync(iv);
        Assert.Equal(16, bytesRead);
    }

    [Fact]
    public async Task DecryptAsync_ShouldReturnOriginalData_AfterEncryptionAndDecryption()
    {
        // Arrange
        var originalData = "Test data";
        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(originalData));
        var key = Aes.Create().Key; // Generate a valid AES key

        // Act
        var encryptedStream = await _encryptionService.EncryptAsync(inputStream, key);
        var decryptedStream = await _encryptionService.DecryptAsync(encryptedStream, key);

        // Assert
        Assert.NotNull(decryptedStream);

        decryptedStream.Position = 0;
        using var reader = new StreamReader(decryptedStream);
        var decryptedData = await reader.ReadToEndAsync();

        Assert.Equal(originalData, decryptedData);
    }

    [Fact]
    public async Task DecryptAsync_ShouldThrowCryptographicException_IfIVIsCorrupted()
    {
        // Arrange
        var originalData = "Test data";
        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(originalData));
        var key = Aes.Create().Key; // Generate a valid AES key

        // Encrypt the data
        var encryptedStream = await _encryptionService.EncryptAsync(inputStream, key);

        // Corrupt the IV (first 16 bytes)
        encryptedStream.Position = 0;
        var corruptedData = new byte[encryptedStream.Length];
        await encryptedStream.ReadAsync(corruptedData);
        corruptedData[0] = (byte)(corruptedData[0] ^ 0xff); // Corrupt the 1st byte of the IV
        var corruptedStream = new MemoryStream(corruptedData);

        // Act & Assert
        await Assert.ThrowsAsync<CryptographicException>(() => _encryptionService.DecryptAsync(corruptedStream, key));
    }

    [Fact]
    public async Task DecryptAsync_ShouldThrowCryptographicException_IfIncorrectKeyIsUsed()
    {
        // Arrange
        var originalData = "Test data";
        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(originalData));
        var correctKey = Aes.Create().Key; // Generate a valid AES key
        var incorrectKey = Aes.Create().Key; // Generate another valid key

        // Encrypt the data
        var encryptedStream = await _encryptionService.EncryptAsync(inputStream, correctKey);

        // Act & Assert
        await Assert.ThrowsAsync<CryptographicException>(() =>
            _encryptionService.DecryptAsync(encryptedStream, incorrectKey));
    }
}