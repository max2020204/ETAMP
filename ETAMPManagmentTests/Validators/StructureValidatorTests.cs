using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Moq;
using Xunit;

namespace ETAMPManagment.Validators.Tests
{
    public class StructureValidatorTests
    {
        private StructureValidator _validator;
        private Mock<IJwtValidator> _jwtValidatorMock;

        public StructureValidatorTests()
        {
            _validator = new StructureValidator();
            _jwtValidatorMock = new Mock<IJwtValidator>();
        }

        [Fact]
        public void IsValidEtampFormat_WithValidJson_ReturnsTrueAndModel()
        {
            var validJson = $"{{\"Id\":\"{Guid.NewGuid()}\",\"Token\":\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0..\",\"UpdateType\":\"Update\",\"SignatureToken\":\"Signature\"}}";

            var model = _validator.IsValidEtampFormat(validJson);

            Assert.NotNull(model);
        }

        [Fact]
        public void IsValidEtampFormat_WithInvalidJson_ThrowsArgumentException()
        {
            var invalidJson = "invalid json";

            Assert.Throws<ArgumentException>(() => _validator.IsValidEtampFormat(invalidJson));
        }

        [Fact]
        public void ValidateETAMPStructure_WithValidData_ReturnsTrue()
        {
            var etamp = $"{{\"Id\":\"{Guid.NewGuid()}\",\"Token\":\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0..\",\"UpdateType\":\"Update\",\"SignatureToken\":\"Signature\",\"SignatureMessage\":\"Message\"}}";

            var isValid = _validator.ValidateETAMPStructure(etamp);

            Assert.True(isValid.IsValid);
        }

        [Fact]
        public void ValidateETAMPStructure_WithInvalidData_ThrowsInvalidOperationException()
        {
            var invalidEtamp = $"{{\"Id\":\"{Guid.NewGuid()}\",\"Token\":\"\",\"UpdateType\":\"Update\",\"SignatureToken\":\"Signature\",\"SignatureMessage\":\"Message\"}}";

            Assert.Throws<InvalidOperationException>(() => _validator.ValidateETAMPStructure(invalidEtamp));
        }

        [Fact]
        public void ValidateIdConsistency_WithConsistentId_ReturnsTrue()
        {
            var jwtValidatorMock = new Mock<IJwtValidator>();
            jwtValidatorMock.Setup(v => v.IsValidJwtToken(It.IsAny<string>())).Returns(new ValidationResult(true));

            var structureValidator = new StructureValidator(jwtValidatorMock.Object);
            var etamp = $"{{\"Id\":\"9C9719B5-0D8E-4F69-9878-A3510CDC26F0\",\"Token\":\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0.eyJNZXNzYWdlSWQiOiI5Qzk3MTlCNS0wRDhFLTRGNjktOTg3OC1BMzUxMENEQzI2RjAiLCJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.8pCZwNn46YgdpbK8ur4egT_18oGwqYeGmb2umI6Y2Dk\",\"UpdateType\":\"Update\",\"SignatureToken\":\"Signature\",\"SignatureMessage\":\"Message\"}}";

            var isValid = structureValidator.ValidateIdConsistency(etamp);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidateIdConsistency_WithInconsistentId_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _validator.ValidateIdConsistency(""));
        }

        [Fact]
        public void IsValidEtampFormat_InputDataNull_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _validator.IsValidEtampFormat(""));
        }

        [Fact]
        public void IsValidEtampFormat_ModelIsNull_ThrowArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => _validator.IsValidEtampFormat("{}"));
            Assert.Contains("Failed to deserialize ETAMP to model", exception.Message);
        }

        [Fact]
        public void ValidateETAMPStructureLite_WithValidData_ReturnsTrue()
        {
            var etamp = $"{{\"Id\":\"{Guid.NewGuid()}\",\"Token\":\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0..\",\"UpdateType\":\"Update\"}}";

            var isValid = _validator.ValidateETAMPStructureLite(etamp);

            Assert.True(isValid.IsValid);
        }

        [Fact]
        public void ValidateETAMPStructureLite_WithInvalidData_ThrowsInvalidOperationException()
        {
            var invalidEtamp = $"{{\"Id\":\"{Guid.NewGuid()}\",\"Token\":\"\",\"UpdateType\":\"\"}}";

            var exception = Assert.Throws<InvalidOperationException>(() => _validator.ValidateETAMPStructureLite(invalidEtamp));
            Assert.Contains("Deserialized ETAMP model is invalid", exception.Message);
        }

        [Fact]
        public void ValidateETAMPStructureLite_InputDataNull_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _validator.ValidateETAMPStructureLite(""));
        }

        [Fact]
        public void ValidateETAMPStructureLite_ModelIsNull_ThrowArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => _validator.ValidateETAMPStructureLite("{}"));
            Assert.Contains("Failed to deserialize ETAMP to model", exception.Message);
        }

        [Fact]
        public void ValidateETAMPStructure_WithValidModel_ReturnsTrue()
        {
            var model = new ETAMPModel
            {
                Id = Guid.NewGuid(),
                Token = "ValidToken",
                UpdateType = "Update",
                SignatureToken = "Signature",
                SignatureMessage = "Message"
            };

            var result = _validator.ValidateETAMPStructure(model);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidateETAMPStructure_WithInvalidModel_ThrowsInvalidOperationException()
        {
            var model = new ETAMPModel(); // Empty or invalid data

            Assert.Throws<InvalidOperationException>(() => _validator.ValidateETAMPStructure(model));
        }

        [Fact]
        public void ValidateETAMPStructureLite_WithValidModel_ReturnsTrue()
        {
            var model = new ETAMPModel
            {
                Id = Guid.NewGuid(),
                Token = "ValidToken",
                UpdateType = "Update"
            };

            var result = _validator.ValidateETAMPStructureLite(model);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidateETAMPStructureLite_WithInvalidModel_ThrowsInvalidOperationException()
        {
            var model = new ETAMPModel(); // Missing essential fields like Token or UpdateType

            Assert.Throws<InvalidOperationException>(() => _validator.ValidateETAMPStructureLite(model));
        }

        [Fact]
        public void ValidateIdConsistency_WithInconsistentId_ReturnsFalse()
        {
            _jwtValidatorMock.Setup(j => j.IsValidJwtToken(It.IsAny<string>())).Returns(new ValidationResult(true));
            _validator = new StructureValidator(_jwtValidatorMock.Object);

            var etamp = $"{{\"Id\":\"9C9719B5-0D8E-4F69-9878-A3510CDC26F1\",\"Token\":\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0.eyJNZXNzYWdlSWQiOiI5Qzk3MTlCNS0wRDhFLTRGNjktOTg3OC1BMzUxMENEQzI2RjAiLCJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.8pCZwNn46YgdpbK8ur4egT_18oGwqYeGmb2umI6Y2Dk\",\"UpdateType\":\"Update\",\"SignatureToken\":\"Signature\",\"SignatureMessage\":\"Message\"}}";

            var result = _validator.ValidateIdConsistency(etamp);

            Assert.False(result);
        }

        [Fact]
        public void ValidateIdConsistency_IsValidJwtTokenIsFalse_ReturnFalse()
        {
            _jwtValidatorMock.Setup(j => j.IsValidJwtToken(It.IsAny<string>())).Returns(new ValidationResult(false));
            var etamp = $"{{\"Id\":\"9C9719B5-0D8E-4F69-9878-A3510CDC26F0\",\"Token\":\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0.eyJNZXNzYWdlSWQiOiI5Qzk3MTlCNS0wRDhFLTRGNjktOTg3OC1BMzUxMENEQzI2RjAiLCJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.8pCZwNn46YgdpbK8ur4egT_18oGwqYeGmb2umI6Y2Dk\",\"UpdateType\":\"Update\",\"SignatureToken\":\"Signature\",\"SignatureMessage\":\"Message\"}}";
            _validator = new StructureValidator(_jwtValidatorMock.Object);
            var result = _validator.ValidateIdConsistency(etamp);

            Assert.False(result);
        }
    }
}