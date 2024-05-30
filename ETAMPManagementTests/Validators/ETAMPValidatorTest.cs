using ETAMPManagement.Models;
using ETAMPManagement.Validators;
using ETAMPManagement.Validators.Base;
using ETAMPManagement.Validators.Interfaces;
using ETAMPManagement.Wrapper.Base;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace ETAMPManagementTests.Validators;

[TestSubject(typeof(ETAMPValidator))]
public class ETAMPValidatorTest
{
    private readonly ETAMPValidator _etampValidator;
    private readonly Mock<SignatureValidatorBaseDefaultCtor> _signatureValidatorBaseMock = new();
    private readonly Mock<IStructureValidator> _structureValidatorMock = new();
    private readonly Mock<ITokenValidator> _tokenValidatorMock = new();

    public ETAMPValidatorTest()
    {
        // Используем реальный подкласс SignatureValidatorBase с конструктором по умолчанию
        _signatureValidatorBaseMock = new Mock<SignatureValidatorBaseDefaultCtor> { CallBase = true };
        _etampValidator = new ETAMPValidator(_tokenValidatorMock.Object, _structureValidatorMock.Object,
            _signatureValidatorBaseMock.Object);
    }

    [Fact]
    public void ValidateETAMP_StructureInvalid_ReturnsStructureValidationResult()
    {
        // Arrange
        var etamp = new ETAMPModel<Token>
        {
            Id = Guid.NewGuid(),
            Version = 1.0,
            Token = new Token(),
            UpdateType = "Update Type 1",
            CompressionType = "Compression Type 1",
            SignatureMessage = "Signature Message 1"
        };
        var validateLite = false;
        var structureValidationResult = new ValidationResult(false, "Structure Invalid");
        _structureValidatorMock.Setup(v => v.ValidateETAMP(etamp, validateLite)).Returns(structureValidationResult);

        // Act
        var validationResult = _etampValidator.ValidateETAMP(etamp, validateLite);

        // Assert
        Assert.Same(structureValidationResult, validationResult);
    }

    [Fact]
    public void ValidateETAMP_TokenInvalid_ReturnsTokenValidationResult()
    {
        // Arrange
        var etamp = new ETAMPModel<Token>
        {
            Id = Guid.NewGuid(),
            Version = 1.0,
            Token = new Token(),
            UpdateType = "Update Type 1",
            CompressionType = "Compression Type 1",
            SignatureMessage = "Signature Message 1"
        };
        var validateLite = false;
        var structureValidationResult = new ValidationResult(true);
        var tokenValidationResult = new ValidationResult(false, "Token Invalid");
        _structureValidatorMock.Setup(v => v.ValidateETAMP(etamp, validateLite)).Returns(structureValidationResult);
        _tokenValidatorMock.Setup(v => v.ValidateToken(etamp)).Returns(tokenValidationResult);

        // Act
        var validationResult = _etampValidator.ValidateETAMP(etamp, validateLite);

        // Assert
        Assert.Same(tokenValidationResult, validationResult);
    }

    [Fact]
    public void ValidateETAMP_ValidETAMP_ReturnsValidValidationResult()
    {
        // Arrange
        var etamp = new ETAMPModel<Token>
        {
            Id = Guid.NewGuid(),
            Version = 1.0,
            Token = new Token(),
            UpdateType = "Update Type 1",
            CompressionType = "Compression Type 1",
            SignatureMessage = "Signature Message 1"
        };
        var validateLite = false;
        var structureValidationResult = new ValidationResult(true);
        var tokenValidationResult = new ValidationResult(true);
        _structureValidatorMock.Setup(v => v.ValidateETAMP(etamp, validateLite)).Returns(structureValidationResult);
        _tokenValidatorMock.Setup(v => v.ValidateToken(etamp)).Returns(tokenValidationResult);
        _signatureValidatorBaseMock.Setup(v => v.ValidateETAMPMessage(etamp)).Returns(new ValidationResult(true));

        // Act
        var validationResult = _etampValidator.ValidateETAMP(etamp, validateLite);

        // Assert
        Assert.True(validationResult.IsValid);
    }

    // Вспомогательный подкласс с конструктором по умолчанию для SignatureValidatorBase
}

public class SignatureValidatorBaseDefaultCtor : SignatureValidatorBase
{
    public SignatureValidatorBaseDefaultCtor() : base(new Mock<VerifyWrapperBase>().Object)
    {
    }

    public override ValidationResult ValidateETAMPMessage<T>(ETAMPModel<T> etamp)
    {
        // Реализация по умолчанию для тестов
        return new ValidationResult(true);
    }
}