using ETAMPManagement.Codec;
using JetBrains.Annotations;
using Xunit;

namespace ETAMPManagementTests.Codec;

[TestSubject(typeof(DeflateCompressionService))]
public class DeflateCompressionServiceTest
{
    private readonly DeflateCompressionService _deflateCompressionService;

    public DeflateCompressionServiceTest()
    {
        _deflateCompressionService = new DeflateCompressionService();
    }

    [Fact]
    public void CompressString_NullOrEmpty_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentNullException>(() => _deflateCompressionService.CompressString(null));
        Assert.Throws<ArgumentException>(() => _deflateCompressionService.CompressString(string.Empty));
    }

    [Theory]
    [InlineData("test string")]
    [InlineData("another test string")]
    public void CompressString_NonNullOrEmptyStrings_CompressedAndBase64Encoded(string data)
    {
        var result = _deflateCompressionService.CompressString(data);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void DecompressString_NullOrEmpty_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentNullException>(() => _deflateCompressionService.DecompressString(null));
        Assert.Throws<ArgumentException>(() => _deflateCompressionService.DecompressString(string.Empty));
    }

    [Theory]
    [InlineData("test string")]
    [InlineData("another test string")]
    public void DecompressString_CompressedAndBase64EncodedStrings_Decompressed(string data)
    {
        var compressedData = _deflateCompressionService.CompressString(data);
        var result = _deflateCompressionService.DecompressString(compressedData);
        Assert.Equal(data, result);
    }
}