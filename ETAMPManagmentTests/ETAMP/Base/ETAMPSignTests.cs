using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace ETAMPManagment.ETAMP.Base.Tests
{
    public class ETAMPSignTests
    {
        private readonly Mock<ISignWrapper> _mockSignWrapper;
        private readonly ETAMPSign _etampSign;

        public ETAMPSignTests()
        {
            _mockSignWrapper = new Mock<ISignWrapper>();
            _etampSign = new ETAMPSign(_mockSignWrapper.Object, new Mock<ISigningCredentialsProvider>().Object);

            _mockSignWrapper.Setup(sw => sw.SignEtampModel(It.IsAny<ETAMPModel>()))
                .Returns((ETAMPModel model) =>
                {
                    model.SignatureMessage = "SignatureMessage";
                    model.SignatureToken = "SignatureToken";
                    return model;
                });
        }

        [Fact]
        public void Constructor_WithSignWrapperAndSigningCredential_InitializesCorrectly()
        {
            Assert.NotNull(_etampSign);
        }

        [Fact]
        public void CreateETAMPModel_SignsToken_WithValidSignatureFields()
        {
            var token = _etampSign.CreateETAMPModel("update", new BasePayload(), 1);

            Assert.Contains("update", token.UpdateType);
            Assert.Equal("SignatureMessage", token.SignatureMessage);
            Assert.Equal("SignatureToken", token.SignatureToken);
        }

        [Fact]
        public void CreateETAMP_SignsToken_WithValidSignatureFields()
        {
            var tokenJson = _etampSign.CreateETAMP("update", new BasePayload(), 1);
            var result = JsonConvert.DeserializeObject<ETAMPModel>(tokenJson);

            Assert.Contains("update", result.UpdateType);
            Assert.Equal("SignatureMessage", result.SignatureMessage);
            Assert.Equal("SignatureToken", result.SignatureToken);
        }
    }
}