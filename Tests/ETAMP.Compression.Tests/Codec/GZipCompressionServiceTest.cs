#region

using System.IO.Compression;
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
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<GZipCompressionService>> _loggerMock;
    private readonly GZipCompressionService _sut;

    public GZipCompressionServiceTest()
    {
        _fixture = new Fixture();
        _loggerMock = new Mock<ILogger<GZipCompressionService>>();
        _sut = new GZipCompressionService(_loggerMock.Object);
    }

    [Fact]
    public async Task CompressStream_ValidInput_ReturnsCompressedStream()
    {
        // Arrange
        var inputStream = new MemoryStream(_fixture.Create<byte[]>());

        // Act
        var result = await _sut.CompressStream(inputStream);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Compressing data stream...")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Data stream compressed.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task CompressStream_NullOrUnreadableStream_ThrowsArgumentException()
    {
        // Arrange
        Stream nullStream = null;
        var unreadableStream = new Mock<Stream>();
        unreadableStream.Setup(s => s.CanRead).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CompressStream(nullStream));
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CompressStream(unreadableStream.Object));

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains("The input stream must not be null and must be readable.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task DecompressStream_ValidInput_ReturnsDecompressedStream()
    {
        // Arrange
        var inputData = _fixture.Create<byte[]>();
        var compressedStream = new MemoryStream();
        await using (var compressor = new GZipStream(compressedStream, CompressionMode.Compress, true))
        {
            await compressor.WriteAsync(inputData);
        }

        compressedStream.Position = 0;

        // Act
        var result = await _sut.DecompressStream(compressedStream);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(inputData.Length, result.Length);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Decompressing data stream...")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Data stream decompressed successfully.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task DecompressStream_NullOrUnreadableStream_ThrowsArgumentException()
    {
        // Arrange
        Stream nullStream = null;
        var unreadableStream = new Mock<Stream>();
        unreadableStream.Setup(s => s.CanRead).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.DecompressStream(nullStream));
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.DecompressStream(unreadableStream.Object));

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains("The input stream must not be null and must be readable.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Exactly(2));
    }


    [Fact]
    public async Task DecompressStream_EmptyDecompressedData_ThrowsInvalidOperationException()
    {
        // Arrange
        var emptyCompressedStream = new MemoryStream();
        using (var compressor = new DeflateStream(emptyCompressedStream, CompressionMode.Compress, true))
        {
            // Write nothing to the stream
        }

        emptyCompressedStream.Position = 0;

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.DecompressStream(emptyCompressedStream));

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("The decompressed data is empty.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task DecompressStream_UnexpectedError_ThrowsInvalidOperationException()
    {
        // Arrange
        var faultyStream = new FaultyStream();
        faultyStream.SetException(new Exception("Unexpected error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.DecompressStream(faultyStream));

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains("An unexpected error occurred during decompression.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}