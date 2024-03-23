using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace ETAMPManagment.ETAMP.Encrypted.Tests
{
    public class EncryptTokenTests
    {
        private readonly Mock<IStructureValidator> _mockStructureValidator;
        private readonly Mock<IEciesEncryptionService> _mockEciesEncryptionService;
        private readonly EncryptToken _encryptToken;
        private readonly string _jsonEtampValid;
        private readonly string _jsonEtampInvalid;
        private readonly ETAMPModel _expectedModel;
        private readonly string _expectedEncryptedToken;

        public EncryptTokenTests()
        {
            _mockStructureValidator = new Mock<IStructureValidator>();
            _mockEciesEncryptionService = new Mock<IEciesEncryptionService>();
            _encryptToken = new EncryptToken(_mockStructureValidator.Object, _mockEciesEncryptionService.Object);

            _jsonEtampValid = "{\"token\":\"example\"}";
            _jsonEtampInvalid = "{\"invalid\":\"data\"}";
            _expectedModel = new ETAMPModel { Token = "encryptedToken" };
            _expectedEncryptedToken = "encryptedToken";

            _mockStructureValidator.Setup(m => m.IsValidEtampFormat(_jsonEtampValid))
                                   .Returns(new ETAMPModel() { Token = "example" });
            _mockStructureValidator.Setup(m => m.IsValidEtampFormat(_jsonEtampInvalid))
                                   .Returns((ETAMPModel model) => model);
            _mockEciesEncryptionService.Setup(m => m.Encrypt(It.IsAny<string>()))
                                       .Returns(_expectedEncryptedToken);
        }

        [Fact]
        public void EncryptETAMP_WithValidToken_ReturnsEncryptedModel()
        {
            var result = _encryptToken.EncryptETAMP(_jsonEtampValid);

            Assert.Equal(_expectedModel.Token, result.Token);
        }

        [Fact]
        public void EncryptETAMP_WithInvalidToken_ThrowsInvalidOperationException()
        {
            Assert.Throws<ArgumentException>(() => _encryptToken.EncryptETAMP(_jsonEtampInvalid));
        }

        [Fact]
        public void EncryptETAMPToken_WithValidToken_ReturnsEncryptedString()
        {
            var result = JsonConvert.DeserializeObject<ETAMPModel>(_encryptToken.EncryptETAMPToken(_jsonEtampValid));

            Assert.Equal(_expectedEncryptedToken, result.Token);
        }
    }
}