using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;
using Moq;
using Xunit;

namespace ETAMPManagment.ETAMP.Encrypted.Tests
{
    public class ETAMPEncryptedSignedTests
    {
        private readonly Mock<ISignWrapper> _signWrapperMock = new Mock<ISignWrapper>();
        private readonly Mock<IEciesEncryptionService> _eciesMock = new Mock<IEciesEncryptionService>();
        private readonly Mock<ISigningCredentialsProvider> _signingCredentialsProviderMock = new Mock<ISigningCredentialsProvider>();
        private readonly ETAMPEncryptedSigned _encryptedSigned;
        private readonly BasePayload _payload;
        private readonly string _updateType = "UpdateType";
        private readonly double _version = 1;

        public ETAMPEncryptedSignedTests()
        {
            _signWrapperMock.Setup(sw => sw.SignEtampModel(It.IsAny<ETAMPModel>()))
                            .Returns((ETAMPModel model) => model);
            _encryptedSigned = new ETAMPEncryptedSigned(_signWrapperMock.Object, _eciesMock.Object, _signingCredentialsProviderMock.Object);
            _payload = new BasePayload();
        }

        [Fact]
        public void CreateEncryptETAMP_ReturnsEncryptedAndSignedTokenAsString()
        {
            string result = _encryptedSigned.CreateEncryptETAMP(_updateType, _payload, _version);
            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void CreateEncryptETAMPModel_ReturnsEncryptedAndSignedETAMPModel()
        {
            var result = _encryptedSigned.CreateEncryptETAMPModel(_updateType, _payload, _version);

            Assert.NotNull(result);
            Assert.IsType<ETAMPModel>(result);
        }
    }
}