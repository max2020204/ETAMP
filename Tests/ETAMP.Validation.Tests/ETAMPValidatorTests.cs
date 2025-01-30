#region

using System.Security.Cryptography;
using AutoFixture;
using ETAMP.Core.Models;
using ETAMP.Validation.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;

#endregion

namespace ETAMP.Validation.Tests;

[TestSubject(typeof(ETAMPValidator))]
public class ETAMPValidatorTests
{
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<ETAMPValidator>> _loggerMock;
    private readonly Mock<ISignatureValidator> _signatureValidatorMock;
    private readonly Mock<IStructureValidator> _structureValidatorMock;
    private readonly ETAMPValidator _sut;
    private readonly Mock<ITokenValidator> _tokenValidatorMock;

    public ETAMPValidatorTests()
    {
        _fixture = new Fixture();
        _tokenValidatorMock = new Mock<ITokenValidator>();
        _structureValidatorMock = new Mock<IStructureValidator>();
        _signatureValidatorMock = new Mock<ISignatureValidator>();
        _loggerMock = new Mock<ILogger<ETAMPValidator>>();
        _fixture.Customize(new SupportMutableValueTypesCustomization());
        _sut = new ETAMPValidator(
            _tokenValidatorMock.Object,
            _structureValidatorMock.Object,
            _signatureValidatorMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task ValidateETAMPAsync_StructureInvalid_ReturnsStructureResult()
    {
        // Arrange
        var expectedResult = new ValidationResult(false, "Structure error");
        var etampModel = _fixture.Create<ETAMPModel<Token>>();

        _structureValidatorMock
            .Setup(v => v.ValidateETAMP(etampModel, It.IsAny<bool>()))
            .Returns(expectedResult);

        // Act
        var result = await _sut.ValidateETAMPAsync(etampModel, true);

        // Assert
        Assert.Equal(expectedResult, result);
        _loggerMock.VerifyLog(LogLevel.Error, "Structure error", Times.Once());
        _tokenValidatorMock.VerifyNoOtherCalls();
        _signatureValidatorMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ValidateETAMPAsync_TokenInvalid_ReturnsTokenResult()
    {
        // Arrange
        var structureResult = new ValidationResult(true);
        var tokenResult = new ValidationResult(false, "Token error");
        var etampModel = _fixture.Create<ETAMPModel<Token>>();

        _structureValidatorMock
            .Setup(v => v.ValidateETAMP(etampModel, It.IsAny<bool>()))
            .Returns(structureResult);

        _tokenValidatorMock
            .Setup(v => v.ValidateToken(etampModel))
            .Returns(tokenResult);

        // Act
        var result = await _sut.ValidateETAMPAsync(etampModel, true);

        // Assert
        Assert.Equal(tokenResult, result);
        _loggerMock.VerifyLog(LogLevel.Error, "Token error", Times.Once());
        _signatureValidatorMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ValidateETAMPAsync_SignatureInvalid_ReturnsSignatureResult()
    {
        // Arrange
        var structureResult = new ValidationResult(true);
        var tokenResult = new ValidationResult(true);
        var signatureResult = new ValidationResult(false, "Signature error");
        var etampModel = _fixture.Create<ETAMPModel<Token>>();

        _structureValidatorMock
            .Setup(v => v.ValidateETAMP(etampModel, It.IsAny<bool>()))
            .Returns(structureResult);

        _tokenValidatorMock
            .Setup(v => v.ValidateToken(etampModel))
            .Returns(tokenResult);

        _signatureValidatorMock
            .Setup(v => v.ValidateETAMPMessageAsync(etampModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(signatureResult);

        // Act
        var result = await _sut.ValidateETAMPAsync(etampModel, true);

        // Assert
        Assert.Equal(signatureResult, result);
        _loggerMock.VerifyLog(LogLevel.Information, "Signature is invalid", Times.Once());
    }

    [Fact]
    public async Task ValidateETAMPAsync_AllValid_ReturnsSuccess()
    {
        // Arrange
        var structureResult = new ValidationResult(true);
        var tokenResult = new ValidationResult(true);
        var signatureResult = new ValidationResult(true);
        var etampModel = _fixture.Create<ETAMPModel<Token>>();

        _structureValidatorMock
            .Setup(v => v.ValidateETAMP(etampModel, It.IsAny<bool>()))
            .Returns(structureResult);

        _tokenValidatorMock
            .Setup(v => v.ValidateToken(etampModel))
            .Returns(tokenResult);

        _signatureValidatorMock
            .Setup(v => v.ValidateETAMPMessageAsync(etampModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(signatureResult);

        // Act
        var result = await _sut.ValidateETAMPAsync(etampModel, true);

        // Assert
        Assert.True(result.IsValid);
        _loggerMock.VerifyLog(LogLevel.Information, "Signature is valid", Times.Once());
    }

    [Fact]
    public void Dispose_CallsSignatureDispose()
    {
        // Act
        _sut.Dispose();

        // Assert
        _signatureValidatorMock.Verify(v => v.Dispose(), Times.Once);
    }

    [Fact]
    public void Initialize_CallsSignatureInitialize()
    {
        // Arrange
        using var ecdsa = ECDsa.Create();
        var algorithmName = HashAlgorithmName.SHA256;

        // Act
        _sut.Initialize(ecdsa, algorithmName);

        // Assert
        _signatureValidatorMock.Verify(v =>
            v.Initialize(ecdsa, algorithmName), Times.Once);
    }
}