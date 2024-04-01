using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using Moq;
using Xunit;

namespace ETAMPManagment.ETAMP.Encrypted.Tests
{
    public class ETAMPEncryptedTests
    {
        private readonly Mock<IEciesEncryptionService> _encryptionServiceMock;
        private readonly string _encryptedToken = "encrypted_token";
        private readonly string _updateType = "update";
        private readonly BasePayload _payload;
        private readonly ETAMPEncrypted _etampEncrypted;
        private readonly double _version = 1.0;

        public ETAMPEncryptedTests()
        {
            _encryptionServiceMock = new Mock<IEciesEncryptionService>();
            _payload = new BasePayload();
            _encryptionServiceMock.Setup(e => e.Encrypt(It.IsAny<string>())).Returns(_encryptedToken);
            _etampEncrypted = new ETAMPEncrypted(_encryptionServiceMock.Object, new Mock<ISigningCredentialsProvider>().Object);
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