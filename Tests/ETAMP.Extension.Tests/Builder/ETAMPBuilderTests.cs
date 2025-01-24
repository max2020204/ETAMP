#region

using System;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using ETAMP.Compression.Interfaces;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Models;
using ETAMP.Extension.Builder;
using JetBrains.Annotations;
using Moq;
using Xunit;

#endregion

namespace ETAMP.Extension.Tests.Builder;

[TestSubject(typeof(ETAMPBuilder))]
public class ETAMPBuilderTests
{
    private readonly Fixture _fixture;

    public ETAMPBuilderTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task BuildAsync_ValidInput_ReturnsSerializedResult()
    {
        // Arrange
        var model = _fixture.Create<ETAMPModel<TestToken>>();
        var compressedToken = _fixture.Create<string>();

        var compressionServiceMock = new Mock<ICompressionService>();
        compressionServiceMock
            .Setup(s => s.CompressString(It.IsAny<string>()))
            .ReturnsAsync(compressedToken);

        var compressionServiceFactoryMock = new Mock<ICompressionServiceFactory>();
        compressionServiceFactoryMock
            .Setup(f => f.Create(model.CompressionType))
            .Returns(compressionServiceMock.Object);

        // Act
        var result = await model.BuildAsync(compressionServiceFactoryMock.Object);

        // Assert
        Assert.NotNull(result); // Ensure the result is not null
        compressionServiceMock.Verify(s => s.CompressString(model.Token.ToJson()),
            Times.Once); // Ensure compression is called
        compressionServiceFactoryMock.Verify(f => f.Create(model.CompressionType),
            Times.Once); // Ensure factory is called

        var deserializedResult = JsonSerializer.Deserialize<ETAMPModelBuilder>(result);

        Assert.NotNull(deserializedResult); // Ensure the output is a valid serialized string
        Assert.Equal(model.Id, deserializedResult.Id);
        Assert.Equal(compressedToken, deserializedResult.Token);
    }

    [Fact]
    public async Task BuildAsync_NullModel_ThrowsArgumentNullException()
    {
        // Arrange
        ETAMPModel<TestToken> model = null;
        var compressionServiceFactoryMock = new Mock<ICompressionServiceFactory>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await model.BuildAsync(compressionServiceFactoryMock.Object));
    }


    [Fact]
    public async Task BuildAsync_NullCompressionServiceFactory_ThrowsArgumentNullException()
    {
        // Arrange
        var model = _fixture.Create<ETAMPModel<TestToken>>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await model.BuildAsync(null));
    }

    [Fact]
    public async Task BuildAsync_NullOrWhitespaceCompressionType_ThrowsArgumentException()
    {
        // Arrange
        var model = _fixture.Create<ETAMPModel<TestToken>>();
        model.CompressionType = " ";

        var compressionServiceFactoryMock = new Mock<ICompressionServiceFactory>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await model.BuildAsync(compressionServiceFactoryMock.Object));
    }

    [Fact]
    public async Task DeconstructETAMPAsync_ValidInput_ReturnsDeserializedModel()
    {
        // Arrange
        var builder = _fixture.Create<ETAMPModelBuilder>();
        var json = JsonSerializer.Serialize(builder);
        var decompressedToken = _fixture.Create<string>();
        var tokenObject = _fixture.Create<TestToken>();

        var compressionServiceMock = new Mock<ICompressionService>();
        compressionServiceMock
            .Setup(s => s.DecompressString(builder.Token))
            .ReturnsAsync(JsonSerializer.Serialize(tokenObject));

        var compressionServiceFactoryMock = new Mock<ICompressionServiceFactory>();
        compressionServiceFactoryMock
            .Setup(f => f.Create(builder.CompressionType))
            .Returns(compressionServiceMock.Object);

        // Act
        var result = await json.DeconstructETAMPAsync<TestToken>(compressionServiceFactoryMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(builder.Id, result.Id);
        Assert.Equal(builder.Version, result.Version);
        Assert.NotNull(result.Token);
        Assert.Equal(tokenObject.SomeProperty,
            result.Token.SomeProperty); // Assuming TestToken has a property to validate

        compressionServiceFactoryMock.Verify(f => f.Create(builder.CompressionType), Times.Once);
        compressionServiceMock.Verify(s => s.DecompressString(builder.Token), Times.Once);
    }

    [Fact]
    public async Task DeconstructETAMPAsync_NullOrEmptyJson_ThrowsArgumentException()
    {
        // Arrange
        var compressionServiceFactoryMock = new Mock<ICompressionServiceFactory>();
        string invalidJson = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await invalidJson.DeconstructETAMPAsync<TestToken>(compressionServiceFactoryMock.Object));
    }

    [Fact]
    public async Task DeconstructETAMPAsync_NonJsonString_ThrowsArgumentException()
    {
        // Arrange
        const string nonJson = "This is not JSON!";
        var compressionServiceFactoryMock = new Mock<ICompressionServiceFactory>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await nonJson.DeconstructETAMPAsync<TestToken>(compressionServiceFactoryMock.Object));
    }

    [Fact]
    public async Task DeconstructETAMPAsync_NullCompressionServiceFactory_ThrowsArgumentNullException()
    {
        // Arrange
        var json = "{}";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await json.DeconstructETAMPAsync<TestToken>(null));
    }
}

public class TestToken : Token
{
    public string SomeProperty { get; set; }
}