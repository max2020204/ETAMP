using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;
using Moq;
using Xunit;

namespace ETAMPManagment.Validators.Tests
{
    public class SignatureValidatorTests
    {
        private readonly Mock<IVerifyWrapper> _verifyWrapperMock;
        private readonly Mock<IStructureValidator> _structureValidatorMock;
        private readonly SignatureValidator _signatureValidator;

        public SignatureValidatorTests()
        {
            _verifyWrapperMock = new Mock<IVerifyWrapper>();
            _structureValidatorMock = new Mock<IStructureValidator>();
            _signatureValidator = new SignatureValidator(_verifyWrapperMock.Object);
        }

        [Fact]
        public void ValidateETAMPMessage_WithString_ReturnsTrueIfValid()
        {
            ETAMPModel model = new ETAMPModel()
            {
                Id = Guid.NewGuid(),
                Version = 1,
                Token = "abs",
                UpdateType = "update",
                SignatureToken = "signToken",
                SignatureMessage = "signature"
            };
            string etamp = "{\"Id\": \"123\", \"Version\": 1, \"Token\": \"abc\", \"UpdateType\": \"update\", \"SignatureToken\": \"signToken\", \"SignatureMessage\": \"signature\"}";
            _structureValidatorMock.Setup(v => v.IsValidEtampFormat(etamp)).Returns(model);
            _structureValidatorMock.Setup(v => v.ValidateETAMPStructure(It.IsAny<ETAMPModel>())).Returns(new ValidationResult(true));

            _verifyWrapperMock.Setup(v => v.VerifyData($"{model.Id}{model.Version}{model.Token}{model.UpdateType}{model.SignatureToken}", model.SignatureMessage)).Returns(true);

            var validator = new SignatureValidator(_verifyWrapperMock.Object, _structureValidatorMock.Object);
            bool result = validator.ValidateETAMPMessage(etamp);

            Assert.True(result);
        }

        [Fact]
        public void ValidateETAMPMessage_WithModel_ReturnsTrueIfValid()
        {
            var etampModel = new ETAMPModel
            {
                Id = Guid.NewGuid(),
                Version = 1,
                Token = "abc",
                UpdateType = "update",
                SignatureToken = "sigToken",
                SignatureMessage = "signature"
            };

            _verifyWrapperMock.Setup(v => v.VerifyData(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            bool result = _signatureValidator.ValidateETAMPMessage(etampModel);

            Assert.True(result);
        }

        [Fact]
        public void ValidateToken_ReturnsTrueIfValid()
        {
            string token = "token";
            string tokenSignature = "signature";

            _verifyWrapperMock.Setup(v => v.VerifyData(token, tokenSignature)).Returns(true);

            bool result = _signatureValidator.ValidateToken(token, tokenSignature);

            Assert.True(result);
        }

        [Fact]
        public void ValidateETAMPMessageTest()
        {
            SignatureValidator signature = new SignatureValidator(_verifyWrapperMock.Object);
            Assert.Throws<InvalidOperationException>(() => signature.ValidateETAMPMessage(""));
        }

        [Fact]
        public void ValidateETAMPMessageTest1()
        {
            _structureValidatorMock.Setup(v => v.IsValidEtampFormat("")).Returns(new ETAMPModel());
            SignatureValidator signatureValidator = new SignatureValidator(_verifyWrapperMock.Object, _structureValidatorMock.Object);
            Assert.False(signatureValidator.ValidateETAMPMessage(""));
        }
    }
}