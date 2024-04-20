using Xunit;

namespace ETAMPManagment.Services.Tests
{
    public class GZipCompressionServiceTests
    {
        private readonly GZipCompressionService _compressionService;

        public GZipCompressionServiceTests()
        {
            _compressionService = new GZipCompressionService();
        }

        [Fact]
        public void CompressString_ValidInput_ReturnsCompressedBase64UrlString()
        {
            var input = "Test string for compression";

            // Act
            var compressed = _compressionService.CompressString(input);

            // Assert
            Assert.NotNull(compressed);
            Assert.NotEqual(input, compressed);
        }

        [Fact]
        public void DecompressString_ValidCompressedInput_ReturnsOriginalString()
        {
            var input = "Test string for compression";
            var compressed = _compressionService.CompressString(input);

            // Act
            var decompressed = _compressionService.DecompressString(compressed);

            // Assert
            Assert.NotNull(decompressed);
            Assert.Equal(input, decompressed);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CompressString_InvalidInput_ThrowsArgumentException(string? invalidInput)
        {
            ArgumentException exception = invalidInput == ""
                ? Assert.Throws<ArgumentException>(() => _compressionService.CompressString(invalidInput))
                : Assert.Throws<ArgumentNullException>(() => _compressionService.CompressString(invalidInput));
            Assert.Equal("data", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DecompressString_InvalidInput_ThrowsArgumentException(string invalidInput)
        {
            ArgumentException exception = invalidInput == ""
                ? Assert.Throws<ArgumentException>(() => _compressionService.DecompressString(invalidInput))
                : Assert.Throws<ArgumentNullException>(() => _compressionService.DecompressString(invalidInput));

            Assert.Equal("base64CompressedData", exception.ParamName);
        }
    }
}