using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using ETAMPManagment.Wrapper;
using Moq;
using Newtonsoft.Json;
using System.Text;
using Xunit;

namespace ETAMPManagment.Tests
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
            _eciesEncryptionService.Setup(x => x.EcdhKeyWrapper).Returns(new EcdhKeyWrapper());
            var result = _encryption.EncryptETAMPToken(json);
            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void CreateEncryptETAMPToken_ReturnsEncryptedToken_WhenPayloadIsValid()
        {
            var updateType = "UpdateType";
            var payload = new BasePaylaod();
            var version = 1.0;
            var signToken = true;

            _eciesEncryptionService.Setup(s => s.Encrypt(It.IsAny<string>())).Returns("encrypted token");

            var result = _encryption.CreateEncryptETAMPToken(updateType, payload, signToken, version);

            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.Contains("encrypted token", result);
        }

        [Fact]
        public void EncryptETAMP_ReturnsEncryptedETAMP_WhenJsonIsValid()
        {
            string json = "{\"Id\":\"7fc2ce0c-834b-4569-b93f-891396f3e700\",\"Version\":1,\"Token\":\"b170beb5-5a39-4e60-a5f8-4bb81734aa89\",\"UpdateType\":\"Type2\",\"SignatureToken\":\"5e58df89-3aea-49f8-8dc9-ee5cb2f1566e\",\"SignatureMessage\":\"Message49\"}";

            _eciesEncryptionService.Setup(s => s.Encrypt(It.IsAny<string>())).Returns("encrypted message");
            _eciesEncryptionService.Setup(x => x.EcdhKeyWrapper).Returns(new EcdhKeyWrapper());

            var result = _encryption.EncryptETAMP(json);

            Assert.NotNull(result);
            Assert.IsType<ETAMPEncrypted>(result);
        }

        [Fact]
        public void CreateEncryptETAMPWithoutSignature_ReturnsEncryptedToken_WhenPayloadIsValid()
        {
            var updateType = "UpdateType1";
            var payload = new BasePaylaod();
            var version = 1.0;
            var expectedEncryptedToken = "encryptedToken";

            _eciesEncryptionService.Setup(s => s.Encrypt(It.IsAny<string>())).Returns(expectedEncryptedToken);

            var result = _encryption.CreateEncryptETAMPWithoutSignature(updateType, payload, signToken: false, version);
            ETAMPModel model = JsonConvert.DeserializeObject<ETAMPModel>(result);

            Assert.NotNull(model);
            Assert.Equal(model.Token, expectedEncryptedToken);
        }

        [Fact]
        public void CreateEncryptETAMPWithoutSignatureFull_ReturnsEncryptedETAMP_WhenPayloadIsValid()
        {
            var eciesMock = new Mock<IEciesEncryptionService>();
            var encryptionService = new ETAMPEncryption(eciesMock.Object);
            var updateType = "UpdateType2";
            var payload = new BasePaylaod();
            var version = 1.0;
            var expectedEncryptedToken = "encryptedToken";

            eciesMock.Setup(s => s.Encrypt(It.IsAny<string>())).Returns(expectedEncryptedToken);
            eciesMock.Setup(s => s.EcdhKeyWrapper).Returns(new EcdhKeyWrapper());

            var result = encryptionService.CreateEncryptETAMPWithoutSignatureFull(updateType, payload, signToken: false, version);

            Assert.NotNull(result);
            Assert.IsType<ETAMPEncrypted>(result);
        }

        [Fact]
        public void CreateEncryptETAMPFull_ReturnsEncryptedETAMPObject_WhenPayloadIsValid()
        {
            var updateType = "UpdateType1";
            var payload = new BasePaylaod();
            var version = 1.0;
            var signToken = true;

            var expectedEncryptedToken = "encryptedToken";
            var expectedPublicKey = "publicKey";
            var expectedPrivateKey = "privateKey";

            _eciesEncryptionService.Setup(s => s.Encrypt(It.IsAny<string>())).Returns(expectedEncryptedToken);
            _eciesEncryptionService.Setup(s => s.EcdhKeyWrapper.PublicKey).Returns(Convert.ToBase64String(Encoding.UTF8.GetBytes(expectedPublicKey)));
            _eciesEncryptionService.Setup(s => s.EcdhKeyWrapper.PrivateKey).Returns(Convert.ToBase64String(Encoding.UTF8.GetBytes(expectedPrivateKey)));

            var result = _encryption.CreateEncryptETAMPFull(updateType, payload, signToken, version);
            ETAMPModel model = JsonConvert.DeserializeObject<ETAMPModel>(result.ETAMP);

            Assert.NotNull(result);
            Assert.IsType<ETAMPEncrypted>(result);
            Assert.Equal(expectedEncryptedToken, model.Token);
            Assert.Equal(expectedPublicKey, Encoding.UTF8.GetString(Convert.FromBase64String(result.PublicKey)));
            Assert.Equal(expectedPrivateKey, Encoding.UTF8.GetString(Convert.FromBase64String(result.PrivateKey)));
        }
    }
}