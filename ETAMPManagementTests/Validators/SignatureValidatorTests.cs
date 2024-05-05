#region

using ETAMPManagement.Models;
using ETAMPManagement.Validators;
using ETAMPManagement.Validators.Interfaces;
using ETAMPManagement.Wrapper.Interfaces;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagementTests.Validators;

public class SignatureValidatorTests
{
    private readonly SignatureValidator _signatureValidator;
    private readonly Mock<IStructureValidator> _structureValidatorMock;
    private readonly Mock<IVerifyWrapper> _verifyWrapperMock;

    public SignatureValidatorTests()
    {
        _verifyWrapperMock = new Mock<IVerifyWrapper>();
        _structureValidatorMock = new Mock<IStructureValidator>();
        _signatureValidator = new SignatureValidator(_verifyWrapperMock.Object, _structureValidatorMock.Object);
    }

    [Fact]
    public void ValidateETAMPMessage_WithString_ReturnsTrueIfValid()
    {
        var model = new ETAMPModel
        {
            Id = Guid.NewGuid(),
            Version = 1,
            Token = "abs",
            UpdateType = "update",
            SignatureToken = "signToken",
            SignatureMessage = "signature"
        };
        var etamp =
            "{\"Id\": \"123\", \"Version\": 1, \"Token\": \"abc\", \"UpdateType\": \"update\", \"SignatureToken\": \"signToken\", \"SignatureMessage\": \"signature\"}";
        _structureValidatorMock.Setup(v => v.IsValidEtampFormat(etamp)).Returns(model);
        _structureValidatorMock.Setup(v => v.ValidateETAMPStructure(It.IsAny<ETAMPModel>()))
            .Returns(new ValidationResult(true));

        _verifyWrapperMock.Setup(v =>
            v.VerifyData($"{model.Id}{model.Version}{model.Token}{model.UpdateType}{model.SignatureToken}",
                model.SignatureMessage)).Returns(true);

        var validator = new SignatureValidator(_verifyWrapperMock.Object, _structureValidatorMock.Object);
        var result = validator.ValidateETAMPMessage(etamp);

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

        var result = _signatureValidator.ValidateETAMPMessage(etampModel);

        Assert.True(result);
    }

    [Fact]
    public void ValidateToken_ReturnsTrueIfValid()
    {
        var token = "token";
        var tokenSignature = "signature";

        _verifyWrapperMock.Setup(v => v.VerifyData(token, tokenSignature)).Returns(true);

        var result = _signatureValidator.ValidateToken(token, tokenSignature);

        Assert.True(result);
    }
}