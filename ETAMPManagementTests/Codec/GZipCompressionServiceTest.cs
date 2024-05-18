using ETAMPManagement.Codec;
using JetBrains.Annotations;
using Xunit;

namespace ETAMPManagementTests.Codec;

[TestSubject(typeof(GZipCompressionService))]
public class GZipCompressionServiceTest
{
    private readonly GZipCompressionService _gzipCompressionService;

    public GZipCompressionServiceTest()
    {
        _gzipCompressionService = new GZipCompressionService();
    }

    [Fact]
    public void CompressString_ShouldCompressCorrectly_GivenNonNullString()
    {
        var testString = "This is a test string to compress";
        var result = _gzipCompressionService.CompressString(testString);
        Assert.NotNull(result);
        Assert.NotEqual(testString, result);
    }

    [Fact]
    public void CompressString_ShouldThrowException_GivenNullString()
    {
        Assert.Throws<ArgumentNullException>(() => _gzipCompressionService.CompressString(null));
    }

    [Fact]
    public void CompressString_ShouldThrowException_GivenEmptyString()
    {
        Assert.Throws<ArgumentException>(() => _gzipCompressionService.CompressString(string.Empty));
    }

    [Fact]
    public void DecompressString_ShouldDecompressCorrectly_GivenValidInputString()
    {
        var testString = "This is a test string to decompress";
        var compressedString = _gzipCompressionService.CompressString(testString);
        var result = _gzipCompressionService.DecompressString(compressedString);
        Assert.Equal(testString, result);
    }

    [Fact]
    public void DecompressString_ShouldThrowException_GivenNullString()
    {
        Assert.Throws<ArgumentNullException>(() => _gzipCompressionService.DecompressString(null));
    }

    [Fact]
    public void DecompressString_ShouldThrowException_GivenEmptyString()
    {
        Assert.Throws<ArgumentException>(() => _gzipCompressionService.DecompressString(string.Empty));
    }

    [Fact]
    public void DecompressString_ShouldThrowException_GivenInvalidBase64String()
    {
        Assert.Throws<FormatException>(() => _gzipCompressionService.DecompressString("This is a test string"));
    }
}