using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Validators.Tests
{
    public class ETAMPValidatorTests
    {
        private readonly Mock<IJwtValidator> _jwtValidator;
        private readonly Mock<IStructureValidator> _structureValidator;
        private readonly Mock<ISignatureValidator> _signatureValidator;
        private readonly ETAMPValidator _validator;
        private readonly ECDsaSecurityKey _tokenSecurityKey;
        private readonly ETAMPModel _etampModel;

        public ETAMPValidatorTests()
        {
            _jwtValidator = new Mock<IJwtValidator>();
            _structureValidator = new Mock<IStructureValidator>();
            _signatureValidator = new Mock<ISignatureValidator>();
            _validator = new ETAMPValidator(_jwtValidator.Object, _structureValidator.Object, _signatureValidator.Object);
            _tokenSecurityKey = new ECDsaSecurityKey(ECDsa.Create());
            _etampModel = new ETAMPModel
            {
                Token = "dummyToken",
                SignatureToken = "dummySignature",
                UpdateType = "Update",
                Id = Guid.NewGuid()
            };
        }

        [Fact]
        public async Task ValidateETAMP_FullValidation_ReturnsTrueIfValid()
        {
            _jwtValidator.Setup(x => x.ValidateToken(_etampModel.Token, "audience", "issuer", _tokenSecurityKey))
                .ReturnsAsync(new ValidationResult(true));
            _signatureValidator.Setup(v => v.ValidateToken(_etampModel.Token, _etampModel.SignatureToken))
                .Returns(true);
            _signatureValidator.Setup(v => v.ValidateETAMPMessage(_etampModel))
                .Returns(true);

            var isValid = await _validator.ValidateETAMP(_etampModel, "audience", "issuer", _tokenSecurityKey);

            Assert.True(isValid);
        }

        [Fact]
        public async Task ValidateETAMP_ReturnsFalseWhenStructureValidationFails()
        {
            _structureValidator.Setup(v => v.ValidateETAMPStructure(_etampModel)).Returns(new ValidationResult(false));

            var result = await _validator.ValidateETAMP(_etampModel, _tokenSecurityKey);

            Assert.False(result);
        }

        [Fact]
        public async Task ValidateETAMP_ReturnsFalseWhenSignatureValidationFails()
        {
            _structureValidator.Setup(v => v.ValidateETAMPStructure(_etampModel)).Returns(new ValidationResult(true));
            _jwtValidator.Setup(v => v.ValidateLifeTime(_etampModel.Token, _tokenSecurityKey)).ReturnsAsync(new ValidationResult(true));
            _signatureValidator.Setup(v => v.ValidateToken(_etampModel.Token, _etampModel.SignatureToken)).Returns(false);

            var isValid = await _validator.ValidateETAMP(_etampModel, _tokenSecurityKey);

            Assert.False(isValid);
        }

        [Fact]
        public async Task ValidateETAMPLite_ReturnsFalseWhenLifeTimeValidationFails()
        {
            _structureValidator.Setup(v => v.ValidateETAMPStructureLite(_etampModel)).Returns(new ValidationResult(true));
            _jwtValidator.Setup(v => v.ValidateLifeTime(_etampModel.Token, _tokenSecurityKey)).ReturnsAsync(new ValidationResult(false));

            var isValid = await _validator.ValidateETAMPLite(_etampModel, _tokenSecurityKey);

            Assert.False(isValid);
        }

        [Fact]
        public async Task ValidateETAMPLite_ReturnsTrueWhenValid()
        {
            _structureValidator.Setup(v => v.ValidateETAMPStructureLite(_etampModel)).Returns(new ValidationResult(true));
            _jwtValidator.Setup(v => v.ValidateLifeTime(_etampModel.Token, _tokenSecurityKey)).ReturnsAsync(new ValidationResult(true));

            var isValid = await _validator.ValidateETAMPLite(_etampModel, _tokenSecurityKey);

            Assert.True(isValid);
        }
    }
}