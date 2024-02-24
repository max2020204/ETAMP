using ETAMP.Services.Interfaces;
using Moq;
using Xunit;

namespace ETAMP.Tests
{
    public class ETAMPEncryptionTests
    {
        private readonly ETAMPEncryption _encryption;
        private readonly Mock<IEciesEncryptionService> _eciesEncryptionService;

        public ETAMPEncryptionTests()
        {
            _eciesEncryptionService = new Mock<IEciesEncryptionService>();
            _encryption = new ETAMPEncryption(_eciesEncryptionService.Object);
        }

        [Fact]
        public void EncryptETAMPToken_ThrowsArgumentException_WhenJsonIsNull()
        {
            Assert.Throws<ArgumentException>(() => _encryption.EncryptETAMPToken(It.IsAny<string>()));
        }

        [Fact]
        public void EncryptETAMPTokenThrowsArgumentException_WhenJsonIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => _encryption.EncryptETAMPToken(string.Empty));
        }

        [Fact()]
        public void EncryptETAMPToken_ThrowsArgumentException_WhenJsonIsInvalid()
        {
            var invalidJson = "invalid json";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _encryption.EncryptETAMPToken(invalidJson));
        }

        [Fact()]
        public void EncryptETAMPToken_ThrowsInvalidOperationException_WhenJsonIsInvalid()
        {
            string json = $"{{\"Id\":\"{Guid.Empty}\",\"Version\":0,\"Token\":null,\"UpdateType\":null,\"SignatureToken\":null,\"SignatureMessage\":null}}";
            Assert.Throws<InvalidOperationException>(() => _encryption.EncryptETAMPToken(json));
        }

        [Fact()]
        public void EncryptETAMPToken_ReturnsETAMPModel_WhenJsonIsValid()
        {
            string json = "{\"Id\":\"b9d4279f-d44c-4cf0-8d6b-5cacaf38b47a\",\"Version\":2,\"Token\":\"ca7bc2f1-5d63-4c4e-b16f-612b19fb4ed4\",\"UpdateType\":\"Type5\",\"SignatureToken\":\"786cbb80-abd2-4f7d-88e9-5292cfbc8ec1\",\"SignatureMessage\":\"Message3\"}";
            var result = _encryption.EncryptETAMPToken(json);
            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }
    }
}