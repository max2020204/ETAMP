#region

using ETAMPManagment.Factory.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using Moq;
using Newtonsoft.Json;
using Xunit;

#endregion

namespace ETAMPManagment.Extensions.Tests;

public class CompressionTests
{
    private const string CompressionType = "GZip";
    private readonly Mock<ICompressionService> _compressionServiceMock;
    private readonly Mock<ICompressionServiceFactory> _factoryMock;

    public CompressionTests()
    {
        _factoryMock = new Mock<ICompressionServiceFactory>();
        _compressionServiceMock = new Mock<ICompressionService>();
        _factoryMock.Setup(f => f.Create(CompressionType)).Returns(_compressionServiceMock.Object);
    }

    [Fact]
    public void Compress_NullModel_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Compression.Compress(null, _factoryMock.Object, CompressionType));
    }

    [Fact]
    public void Compress_ValidModel_CompressesSuccessfully()
    {
        var model = new ETAMPModel { Id = Guid.NewGuid(), Version = 1.0, Token = "sampleToken" };
        var compressedString = "compressedData";
        _compressionServiceMock.Setup(c => c.CompressString(It.IsAny<string>())).Returns(compressedString);

        var result = model.Compress(_factoryMock.Object, CompressionType);

        Assert.Equal(compressedString, result);
        _compressionServiceMock.Verify(c => c.CompressString(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Decompress_NullJsonEtamp_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Compression.Decompress(null, _factoryMock.Object, CompressionType));
    }

    [Fact]
    public void Decompress_ValidJson_CompressedDecompressesSuccessfully()
    {
        var jsonEtamp = "compressedData";
        var decompressedJson = JsonConvert.SerializeObject(new ETAMPModel
            { Id = Guid.NewGuid(), Version = 1.0, Token = "sampleToken" });
        _compressionServiceMock.Setup(c => c.DecompressString(jsonEtamp)).Returns(decompressedJson);

        var result = jsonEtamp.Decompress(_factoryMock.Object, CompressionType);

        Assert.NotNull(result);
        Assert.IsType<ETAMPModel>(result);
        _compressionServiceMock.Verify(c => c.DecompressString(jsonEtamp), Times.Once);
    }

    [Fact]
    public void Decompress_InvalidJson_ThrowsInvalidOperationException()
    {
        var jsonEtamp = "invalidData";
        _compressionServiceMock.Setup(c => c.DecompressString(jsonEtamp)).Throws(new InvalidOperationException());

        Assert.Throws<InvalidOperationException>(() => jsonEtamp.Decompress(_factoryMock.Object, CompressionType));
    }

    [Fact]
    public void Decompress_InvalidJsonFormat_ThrowsArgumentException()
    {
        var jsonEtamp = "badJsonData";
        _compressionServiceMock.Setup(c => c.DecompressString(jsonEtamp)).Returns("invalidJson");

        Assert.Throws<ArgumentException>(() => jsonEtamp.Decompress(_factoryMock.Object, CompressionType));
    }
}