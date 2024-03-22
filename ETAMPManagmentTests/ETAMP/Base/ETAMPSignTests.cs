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
        [Fact]
        public void Constructor_WithSignWrapperAndSigningCredential_InitializesCorrectly()
        {
            var etampSign = new ETAMPSign(new Mock<ISignWrapper>().Object, new Mock<ISigningCredentialsProvider>().Object);

            Assert.NotNull(etampSign);
        }

        [Fact]
        public void CreateETAMPModel_SignsToken_WithValidSignatureFields()
        {
            var mockSignWrapper = new Mock<ISignWrapper>();
            var etampSign = new ETAMPSign(mockSignWrapper.Object, new Mock<ISigningCredentialsProvider>().Object);
            mockSignWrapper.Setup(sw => sw.SignEtampModel(It.IsAny<ETAMPModel>()))
                         .Returns((ETAMPModel model) =>
                         {
                             model.SignatureMessage = "SignatureMessage";
                             model.SignatureToken = "SignatureToken";
                             return model;
                         });
            var token = etampSign.CreateETAMPModel("update", new BasePayload(), 1);

            Assert.Contains("update", token.UpdateType);
            Assert.Equal("SignatureMessage", token.SignatureMessage);
            Assert.Equal("SignatureToken", token.SignatureToken);
        }

        [Fact]
        public void CreateETAMP_SignsToken_WithValidSignatureFields()
        {
            var mockSignWrapper = new Mock<ISignWrapper>();
            var etampSign = new ETAMPSign(mockSignWrapper.Object, new Mock<ISigningCredentialsProvider>().Object);
            mockSignWrapper.Setup(sw => sw.SignEtampModel(It.IsAny<ETAMPModel>()))
                         .Returns((ETAMPModel model) =>
                         {
                             model.SignatureMessage = "SignatureMessage";
                             model.SignatureToken = "SignatureToken";
                             return model;
                         });
            var token = etampSign.CreateETAMP("update", new BasePayload(), 1);
            var result = JsonConvert.DeserializeObject<ETAMPModel>(token);

            Assert.Contains("update", result.UpdateType);
            Assert.Equal("SignatureMessage", result.SignatureMessage);
            Assert.Equal("SignatureToken", result.SignatureToken);
        }
    }
}