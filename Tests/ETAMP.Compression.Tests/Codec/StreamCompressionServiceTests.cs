using System.IO.Pipelines;
using System.Text;
using ETAMP.Compression.Codec;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;

namespace ETAMP.Compression.Tests.Codec;

[TestSubject(typeof(StreamCompressionService))]
public class StreamCompressionServiceTests
{
    private readonly DeflateCompressionService _compressionService;
    private readonly Mock<ILogger<DeflateCompressionService>> _loggerMock = new();

    public StreamCompressionServiceTests()
    {
        _compressionService = new DeflateCompressionService(_loggerMock.Object);
    }

    [Fact]
    public async Task CompressAsync_CompressesAndDecompresses_Correctly()
    {
        // Arrange
        var originalText = "This is some sample text to be compressed!";
        var inputBytes = Encoding.UTF8.GetBytes(originalText);

        var inputPipe = new Pipe();
        var outputPipe = new Pipe();

        await inputPipe.Writer.WriteAsync(inputBytes);
        await inputPipe.Writer.CompleteAsync();

        // Act: compress
        await _compressionService.CompressAsync(inputPipe.Reader, outputPipe.Writer);
        var compressed = await ReadAllBytesFromPipeAsync(outputPipe.Reader);

        // Now decompress to verify data
        var compressedInput = new Pipe();
        var decompressedOutput = new Pipe();

        await compressedInput.Writer.WriteAsync(compressed);
        compressedInput.Writer.Complete();

        await _compressionService.DecompressAsync(compressedInput.Reader, decompressedOutput.Writer);
        var decompressed = await ReadAllBytesFromPipeAsync(decompressedOutput.Reader);

        var decompressedText = Encoding.UTF8.GetString(decompressed);

        // Assert
        Assert.Equal(originalText, decompressedText);
    }

    [Fact]
    public async Task CompressAsync_ThrowsInvalidOperationException_OnFault()
    {
        var faultyReader = new FaultyPipeReader();
        var output = new Pipe();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _compressionService.CompressAsync(faultyReader, output.Writer));

        Assert.Equal("Compression failed.", ex.Message);
    }

    [Fact]
    public async Task DecompressAsync_ThrowsInvalidOperationException_OnFault()
    {
        var faultyReader = new FaultyPipeReader();
        var output = new Pipe();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _compressionService.DecompressAsync(faultyReader, output.Writer));

        Assert.Equal("Decompression failed.", ex.Message);
    }

    private static async Task<byte[]> ReadAllBytesFromPipeAsync(PipeReader reader)
    {
        using var ms = new MemoryStream();
        while (true)
        {
            var result = await reader.ReadAsync();
            var buffer = result.Buffer;

            foreach (var segment in buffer)
                await ms.WriteAsync(segment);

            reader.AdvanceTo(buffer.End);

            if (result.IsCompleted)
                break;
        }

        await reader.CompleteAsync();
        return ms.ToArray();
    }
}