using ETAMPManagement.Codec.Interfaces;
using ETAMPManagement.Extensions.Builder;
using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Management;
using ETAMPManagement.Models;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace ETAMPManagementTests.Extensions.Builder;

[TestSubject(typeof(ETAMPBuilder))]
public class ETAMPBuildTest
{
    private readonly Mock<ICompressionService> _mockCompressionService;
    private readonly Mock<ICompressionServiceFactory> _mockCompressionServiceFactory;

    public ETAMPBuildTest()
    {
        _mockCompressionServiceFactory = new Mock<ICompressionServiceFactory>();
        _mockCompressionService = new Mock<ICompressionService>();
    }

    [Fact]
    public void Build_Null_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => ETAMPBuilder.Build<Token>(null, _mockCompressionServiceFactory.Object));
    }

    [Fact]
    public void Build_ValidInput_GeneratesCompressedString()
    {
        var testModel = new ETAMPModel<Token>
        {
            Token = new Token(),
            CompressionType = CompressionNames.Deflate
        };
        _mockCompressionService.Setup(s => s.CompressString(It.IsAny<string>())).Returns("compressed");
        _mockCompressionServiceFactory.Setup(f => f.Create(It.IsAny<string>())).Returns(_mockCompressionService.Object);

        var result = testModel.Build(_mockCompressionServiceFactory.Object);

        Assert.NotNull(result);
        _mockCompressionService.Verify(s => s.CompressString(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void DecompressETAMP_Null_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ETAMPBuilder.DeconstructETAMP<Token>(null, _mockCompressionServiceFactory.Object));
    }

    [Fact]
    public void DecompressETAMP_NotJsonString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            "NotJson".DeconstructETAMP<Token>(_mockCompressionServiceFactory.Object));
    }

    [Fact]
    public void DecompressETAMP_ValidInput_GeneratesDecompressedETAMPModel()
    {
        var jsonEtamp =
            "{\"Id\":\"85C75FB7-CA2F-478A-B16B-B585B58F9986\",\"Version\":1,\"Token\":\"compressed\",\"UpdateType\":\"update\",\"CompressionType\":\"type\",\"SignatureMessage\":\"message\"}";
        _mockCompressionService.Setup(s => s.DecompressString(It.IsAny<string>()))
            .Returns("{\"Id\":\"85C75FB7-CA2F-478A-B16B-B585B58F9986\"}");
        _mockCompressionServiceFactory.Setup(f => f.Create(It.IsAny<string>())).Returns(_mockCompressionService.Object);

        var result = jsonEtamp.DeconstructETAMP<Token>(_mockCompressionServiceFactory.Object);

        Assert.NotNull(result);
        _mockCompressionService.Verify(s => s.DecompressString(It.IsAny<string>()), Times.Once);
    }
}