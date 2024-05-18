using ETAMPManagement.Codec.Interfaces;
using ETAMPManagement.Extensions;
using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Models;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace ETAMPManagementTests.Extensions;

[TestSubject(typeof(Compression))]
public class CompressionTest
{
    [Fact]
    public void CompressTest_NullModel_ThrowsArgumentNullException()
    {
        var mockFactory = new Mock<ICompressionServiceFactory>();
        var mockService = new Mock<ICompressionService>();

        mockFactory.Setup(x => x.Create(It.IsAny<string>())).Returns(mockService.Object);

        Assert.Throws<ArgumentNullException>(() => ((ETAMPModel<Token>)null!).Compress(mockFactory.Object, "mock"));
    }

    [Fact]
    public void CompressTest_NullFactory_ThrowsArgumentNullException()
    {
        var model = new ETAMPModel<Token>();

        Assert.Throws<ArgumentNullException>(() => model.Compress(null!, "mock"));
    }

    [Fact]
    public void CompressTest_NullCompressionType_ThrowsArgumentException()
    {
        var model = new ETAMPModel<Token>();
        var mockFactory = new Mock<ICompressionServiceFactory>();

        Assert.Throws<ArgumentNullException>(() => model.Compress(mockFactory.Object, null!));
    }

    [Fact]
    public void DecompressTest_NullJsonEtamp_ThrowsArgumentException()
    {
        var mockFactory = new Mock<ICompressionServiceFactory>();

        Assert.Throws<ArgumentNullException>(() => ((string)null!).Decompress<Token>(mockFactory.Object, "mock"));
    }

    [Fact]
    public void DecompressTest_NullFactory_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => "".Decompress<Token>(null!, "mock"));
    }

    [Fact]
    public void DecompressTest_NullCompressionType_ThrowsArgumentException()
    {
        var mockFactory = new Mock<ICompressionServiceFactory>();

        Assert.Throws<ArgumentException>(() => "".Decompress<Token>(mockFactory.Object, null!));
    }

    [Fact]
    public void CompressTest_ValidInput_ReturnsExpectedResult()
    {
        var model = new ETAMPModel<Token>();
        var mockFactory = new Mock<ICompressionServiceFactory>();
        var mockService = new Mock<ICompressionService>();

        mockService.Setup(x => x.CompressString(It.IsAny<string>())).Returns("mockResult");
        mockFactory.Setup(x => x.Create(It.IsAny<string>())).Returns(mockService.Object);

        var result = model.Compress(mockFactory.Object, "mock");

        Assert.Equal("mockResult", result);
    }

    [Fact]
    public void DecompressTest_ValidInput_ReturnsExpectedResult()
    {
        var jsonEtamp = "{\"Data\": \"mock\"}";
        var mockFactory = new Mock<ICompressionServiceFactory>();
        var mockService = new Mock<ICompressionService>();
        var expectedModel = new ETAMPModel<Token>();

        mockService.Setup(x => x.DecompressString(It.IsAny<string>())).Returns(jsonEtamp);
        mockFactory.Setup(x => x.Create(It.IsAny<string>())).Returns(mockService.Object);

        var result = jsonEtamp.Decompress<Token>(mockFactory.Object, "mock");

        Assert.Equal(expectedModel, result);
    }
}