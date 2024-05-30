using ETAMPManagement.Models;
using ETAMPManagement.Validators;
using ETAMPManagement.Validators.Interfaces;
using ETAMPManagement.Wrapper.Base;
using Moq;
using Xunit;

namespace ETAMPManagementTests.Validators;

public class SignatureValidatorTest
{
    private readonly Mock<IStructureValidator> _mockStructureValidator;
    private readonly Mock<VerifyWrapperBase> _mockVerifyWrapper;
    private readonly SignatureValidator _signatureValidator;

    public SignatureValidatorTest()
    {
        _mockStructureValidator = new Mock<IStructureValidator>();
        _mockVerifyWrapper = new Mock<VerifyWrapperBase>();

        _signatureValidator = new SignatureValidator(_mockVerifyWrapper.Object, _mockStructureValidator.Object);
    }

    [Fact]
    public void ValidateETAMPMessage_ShouldFail_WhenETAMPModelIsNull()
    {
        ETAMPModel<Token> etamp = null;

        Assert.Throws<ArgumentNullException>(() => _signatureValidator.ValidateETAMPMessage(etamp));
    }

    [Fact]
    public void ValidateETAMPMessage_ShouldFail_WhenETAMPStructureIsInvalid()
    {
        var etamp = new ETAMPModel<Token>
        {
            Id = Guid.NewGuid(),
            Version = 1.0,
            Token = new Token()
        };
        _mockStructureValidator.Setup(m => m.ValidateETAMP(etamp, false)).Returns(new ValidationResult(false));

        var result = _signatureValidator.ValidateETAMPMessage(etamp);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateETAMPMessage_ShouldFail_WhenSignatureMessageIsNull()
    {
        var etamp = new ETAMPModel<Token>
        {
            Id = Guid.NewGuid(),
            Version = 1.0,
            Token = new Token(),
            SignatureMessage = null
        };
        _mockStructureValidator.Setup(m => m.ValidateETAMP(etamp, false)).Returns(new ValidationResult(true));

        var result = _signatureValidator.ValidateETAMPMessage(etamp);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateETAMPMessage_ShouldFail_WhenDataVerificationFails()
    {
        var etamp = new ETAMPModel<Token>
        {
            Id = Guid.NewGuid(),
            Version = 1.0,
            Token = new Token(),
            SignatureMessage = "test"
        };
        _mockStructureValidator.Setup(m => m.ValidateETAMP(etamp, false)).Returns(new ValidationResult(true));
        _mockVerifyWrapper.Setup(m => m.VerifyData(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        var result = _signatureValidator.ValidateETAMPMessage(etamp);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateETAMPMessage_ShouldPass_WhenDataVerificationSucceeds()
    {
        var etamp = new ETAMPModel<Token>
        {
            Id = Guid.NewGuid(),
            Version = 1.0,
            Token = new Token(),
            SignatureMessage = "test"
        };
        _mockStructureValidator.Setup(m => m.ValidateETAMP(etamp, false)).Returns(new ValidationResult(true));
        _mockVerifyWrapper.Setup(m => m.VerifyData(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var result = _signatureValidator.ValidateETAMPMessage(etamp);

        Assert.True(result.IsValid);
    }
}