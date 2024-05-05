#region

using System.Security.Cryptography;
using ETAMPManagement.Models;
using ETAMPManagement.Validators;
using ETAMPManagement.Validators.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagementTests.Validators;

public class ETAMPValidatorTests
{
    private readonly ETAMPModel _etampModel;
    private readonly Mock<IJwtValidator> _jwtValidatorMock;
    private readonly Mock<ISignatureValidator> _signatureValidatorMock;
    private readonly Mock<IStructureValidator> _structureValidatorMock;
    private readonly ECDsaSecurityKey _tokenSecurityKey;
    private readonly ETAMPValidator _validator;

    public ETAMPValidatorTests()
    {
        _jwtValidatorMock = new Mock<IJwtValidator>();
        _structureValidatorMock = new Mock<IStructureValidator>();
        _signatureValidatorMock = new Mock<ISignatureValidator>();
        _validator = new ETAMPValidator(_jwtValidatorMock.Object, _structureValidatorMock.Object,
            _signatureValidatorMock.Object);
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
        _jwtValidatorMock.Setup(x => x.ValidateToken(_etampModel.Token, "audience", "issuer", _tokenSecurityKey))
            .ReturnsAsync(new ValidationResult(true));
        _structureValidatorMock.Setup(x => x.ValidateETAMPStructure(It.IsAny<ETAMPModel>()))
            .Returns(new ValidationResult(true));
        _signatureValidatorMock.Setup(v => v.ValidateToken(_etampModel.Token, _etampModel.SignatureToken))
            .Returns(true);
        _signatureValidatorMock.Setup(v => v.ValidateETAMPMessage(_etampModel))
            .Returns(true);

        var isValid = await _validator.ValidateETAMP(_etampModel, "audience", "issuer", _tokenSecurityKey);

        Assert.True(isValid);
    }

    [Fact]
    public async Task ValidateETAMP_ReturnsFalseWhenStructureValidationFails()
    {
        _structureValidatorMock.Setup(v => v.ValidateETAMPStructure(_etampModel))
            .Returns(new ValidationResult(false));
        _jwtValidatorMock.Setup(x => x.ValidateLifeTime(It.IsAny<string>(), _tokenSecurityKey))
            .ReturnsAsync(new ValidationResult(false));

        var result = await _validator.ValidateETAMP(_etampModel, _tokenSecurityKey);

        Assert.False(result);
    }

    [Fact]
    public async Task ValidateETAMP_ReturnsFalseWhenSignatureValidationFails()
    {
        _structureValidatorMock.Setup(v => v.ValidateETAMPStructure(_etampModel)).Returns(new ValidationResult(true));
        _jwtValidatorMock.Setup(v => v.ValidateLifeTime(_etampModel.Token, _tokenSecurityKey))
            .ReturnsAsync(new ValidationResult(true));
        _signatureValidatorMock.Setup(v => v.ValidateToken(_etampModel.Token, _etampModel.SignatureToken))
            .Returns(false);

        var isValid = await _validator.ValidateETAMP(_etampModel, _tokenSecurityKey);

        Assert.False(isValid);
    }

    [Fact]
    public async Task ValidateETAMPLite_ReturnsFalseWhenLifeTimeValidationFails()
    {
        _structureValidatorMock.Setup(v => v.ValidateETAMPStructureLite(_etampModel))
            .Returns(new ValidationResult(true));
        _jwtValidatorMock.Setup(v => v.ValidateLifeTime(_etampModel.Token, _tokenSecurityKey))
            .ReturnsAsync(new ValidationResult(false));

        var isValid = await _validator.ValidateETAMPLite(_etampModel, _tokenSecurityKey);

        Assert.False(isValid);
    }

    [Fact]
    public async Task ValidateETAMPLite_ReturnsTrueWhenValid()
    {
        _structureValidatorMock.Setup(v => v.ValidateETAMPStructureLite(_etampModel))
            .Returns(new ValidationResult(true));
        _jwtValidatorMock.Setup(v => v.ValidateLifeTime(_etampModel.Token, _tokenSecurityKey))
            .ReturnsAsync(new ValidationResult(true));

        var isValid = await _validator.ValidateETAMPLite(_etampModel, _tokenSecurityKey);

        Assert.True(isValid);
    }
}