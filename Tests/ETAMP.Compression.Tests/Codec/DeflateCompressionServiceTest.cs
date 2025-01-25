#region

using System.Text;
using AutoFixture;
using ETAMP.Compression.Codec;
using ETAMP.Core.Utils;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;

#endregion

namespace ETAMP.Compression.Tests.Codec;

[TestSubject(typeof(DeflateCompressionServiceTests))]
public class DeflateCompressionServiceTests
{
    private readonly DeflateCompressionService _compressionService;
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<DeflateCompressionService>> _logger;

    public DeflateCompressionServiceTests()
    {
        _logger = new Mock<ILogger<DeflateCompressionService>>();
        _compressionService = new DeflateCompressionService(_logger.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CompressString_ShouldThrowArgumentNullException_WhenDataIsNull()
    {
        // Arrange
        string? data = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _compressionService.CompressString(data));
    }

    [Fact]
    public async Task CompressString_ShouldThrowArgumentException_WhenDataIsEmpty()
    {
        // Arrange
        var data = string.Empty;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _compressionService.CompressString(data));
    }

    [Fact]
    public async Task CompressString_ShouldReturnBase64EncodedCompressedString_WhenDataIsValid()
    {
        // Arrange
        var data = _fixture.Create<string>();

        // Act
        var compressedData = await _compressionService.CompressString(data);

        // Assert
        Assert.NotNull(compressedData);
        Assert.NotEmpty(compressedData);

        // Base64UrlEncoder.DecodeBytes should not throw an exception, verifying it is valid Base64 URL-encoded
        var decodedBytes = Base64UrlEncoder.DecodeBytes(compressedData);
        Assert.NotNull(decodedBytes);
        Assert.NotEmpty(decodedBytes);
    }

    [Fact]
    public async Task DecompressString_ShouldThrowArgumentNullException_WhenDataIsNull()
    {
        // Arrange
        string? base64CompressedData = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _compressionService.DecompressString(base64CompressedData));
    }

    [Fact]
    public async Task DecompressString_ShouldThrowArgumentException_WhenDataIsEmpty()
    {
        // Arrange
        var base64CompressedData = string.Empty;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _compressionService.DecompressString(base64CompressedData));
    }

    [Fact]
    public async Task DecompressString_ShouldReturnOriginalString_AfterCompressionAndDecompression()
    {
        // Arrange
        var originalString = _fixture.Create<string>();

        // Act
        var compressedData = await _compressionService.CompressString(originalString);
        var decompressedString = await _compressionService.DecompressString(compressedData);

        // Assert
        Assert.Equal(originalString, decompressedString);
    }

    [Fact]
    public async Task CompressAndDecompress_ShouldWorkCorrectly_ForLargeData()
    {
        // Arrange
        var largeData = new string('x', 10_000); // A string with 10,000 'x' characters

        // Act
        var compressedData = await _compressionService.CompressString(largeData);
        var decompressedData = await _compressionService.DecompressString(compressedData);

        // Assert
        Assert.Equal(largeData, decompressedData);
    }

    [Fact]
    public async Task CompressString_ShouldProduceSmallerOutputThanInput_ForLargeData()
    {
        // Arrange
        var largeData = new string('x', 10_000); // A string with 10,000 'x' characters

        // Act
        var compressedData = await _compressionService.CompressString(largeData);

        // Assert
        Assert.NotNull(compressedData);
        var compressedBytes = Base64UrlEncoder.DecodeBytes(compressedData);

        // The length of compressed data should be less than original data
        Assert.True(compressedBytes.Length < Encoding.UTF8.GetBytes(largeData).Length);
    }
}