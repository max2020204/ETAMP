#region

using AutoFixture;
using ETAMP.Compression.Codec;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;

#endregion

namespace ETAMP.Compression.Tests.Codec;

[TestSubject(typeof(GZipCompressionService))]
public class GZipCompressionServiceTest
{
    private readonly GZipCompressionService _compressionService;
    private readonly Fixture _fixture;

    public GZipCompressionServiceTest()
    {
        var logger = new Mock<ILogger<GZipCompressionService>>();
        _compressionService = new GZipCompressionService(logger.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CompressString_ShouldCompressStringCorrectly()
    {
        // Arrange
        var originalString = _fixture.Create<string>();

        // Act
        var compressedString = await _compressionService.CompressString(originalString);

        // Assert
        Assert.NotNull(compressedString);
        Assert.NotEmpty(compressedString);
        Assert.NotEqual(originalString, compressedString); // Compressed should not match the original
    }

    [Fact]
    public async Task DecompressString_ShouldDecompressStringCorrectly()
    {
        // Arrange
        var originalString = _fixture.Create<string>();
        var compressedString = await _compressionService.CompressString(originalString);

        // Act
        var decompressedString = await _compressionService.DecompressString(compressedString);

        // Assert
        Assert.NotNull(decompressedString);
        Assert.NotEmpty(decompressedString);
        Assert.Equal(originalString, decompressedString); // Verify decompression restores original string
    }

    [Fact]
    public async Task CompressAndDecompress_ShouldBeBidirectional()
    {
        // Arrange
        var originalString = _fixture.Create<string>();

        // Act
        var compressedString = await _compressionService.CompressString(originalString);
        var decompressedString = await _compressionService.DecompressString(compressedString);

        // Assert
        Assert.Equal(originalString, decompressedString); // Input and output should match exactly
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public async Task CompressString_ShouldThrowArgumentException_WhenInputIsNullOrEmpty(string? input)
    {
        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ArgumentException>(() => _compressionService.CompressString(input));
        Assert.Contains("The input data must not be null, empty, or consist only of whitespace.",
            exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public async Task DecompressString_ShouldThrowArgumentException_WhenInputIsNullOrEmpty(string? input)
    {
        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ArgumentException>(() => _compressionService.DecompressString(input));
        Assert.Contains("The input data must not be null, empty, or consist only of whitespace.",
            exception.Message);
    }

    [Fact]
    public async Task DecompressString_ShouldThrowException_WhenInputIsInvalidBase64()
    {
        // Arrange
        var invalidBase64 = "not-valid-base64";

        // Act & Assert
        await Assert.ThrowsAsync<InvalidDataException>(() => _compressionService.DecompressString(invalidBase64));
    }
}