using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace ETAMPManagment.ETAMP.Encrypted.Tests
{
    public class ETAMPEncryptedTests
    {
        private readonly Mock<IEciesEncryptionService> _encryptionServiceMock;
        private readonly Mock<ISigningCredentialsProvider> _signingCredentialsProviderMock;
        private readonly string _encryptedToken = "encrypted_token";
        private readonly string _updateType = "update";
        private readonly BasePayload _payload;
        private readonly ETAMPEncrypted _etampEncrypted;
        private readonly double _version = 1.0;

        public ETAMPEncryptedTests()
        {
            _payload = new BasePayload();
            _encryptionServiceMock = new Mock<IEciesEncryptionService>();
            _signingCredentialsProviderMock = new Mock<ISigningCredentialsProvider>();

            _encryptionServiceMock.Setup(e => e.Encrypt(It.IsAny<string>())).Returns(_encryptedToken);
            _etampEncrypted = new ETAMPEncrypted(_encryptionServiceMock.Object, _signingCredentialsProviderMock.Object);
        }

        [Fact]
        public void CreateEncryptETAMP_ShouldEncryptTokenAndSerializeModel()
        {
            var result = _etampEncrypted.CreateEncryptETAMP(_updateType, _payload, _version);

            _encryptionServiceMock.Verify(e => e.Encrypt(It.IsAny<string>()), Times.Once);
            Assert.Contains(_encryptedToken, result);

            var deserializedResult = JsonConvert.DeserializeObject<ETAMPModel>(result);
            Assert.NotNull(deserializedResult);
            Assert.Equal(_encryptedToken, deserializedResult.Token);
            Assert.Equal(_updateType, deserializedResult.UpdateType);
            Assert.Equal(_version, deserializedResult.Version);
        }

        [Fact]
        public void CreateEncryptETAMPModel_ShouldEncryptTokenAndReturnModel()
        {
            var result = _etampEncrypted.CreateEncryptETAMPModel(_updateType, _payload, _version);
            _encryptionServiceMock.Verify(e => e.Encrypt(It.IsAny<string>()), Times.Once);
            Assert.Equal(_encryptedToken, result.Token);
            Assert.Equal(_updateType, result.UpdateType);
            Assert.Equal(_version, result.Version);
        }
    }
}