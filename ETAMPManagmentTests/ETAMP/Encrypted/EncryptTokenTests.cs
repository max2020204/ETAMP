#region

using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagment.ETAMP.Encrypted.Tests;

public class EncryptTokenTests
{
    private readonly Mock<IEciesEncryptionService> _eciesEncryptionServiceMock;
    private readonly EncryptToken _encryptToken;
    private readonly ETAMPModel _expectedModel;
    private readonly string _jsonEtampInvalid;
    private readonly string _jsonEtampValid;
    private readonly Mock<IStructureValidator> _structureValidatorMock;

    public EncryptTokenTests()
    {
        _structureValidatorMock = new Mock<IStructureValidator>();
        _eciesEncryptionServiceMock = new Mock<IEciesEncryptionService>();
        _encryptToken = new EncryptToken(_structureValidatorMock.Object, _eciesEncryptionServiceMock.Object);

        _jsonEtampValid = "{\"token\":\"example\"}";
        _jsonEtampInvalid = "{\"invalid\":\"data\"}";
        _expectedModel = new ETAMPModel { Token = "encryptedToken" };
        var expectedEncryptedToken = "encryptedToken";

        _structureValidatorMock.Setup(m => m.IsValidEtampFormat(_jsonEtampValid))
            .Returns(new ETAMPModel { Token = "example" });
        _structureValidatorMock.Setup(m => m.IsValidEtampFormat(_jsonEtampInvalid))
            .Returns((ETAMPModel model) => model);
        _eciesEncryptionServiceMock.Setup(m => m.Encrypt(It.IsAny<string>()))
            .Returns(expectedEncryptedToken);
    }

    [Fact]
    public void EncryptETAMP_WithValidToken_ReturnsEncryptedModel()
    {
        var result = _encryptToken.EncryptETAMP(_jsonEtampValid);

        Assert.Equal(_expectedModel.Token, result.Token);
    }

    [Fact]
    public void EncryptETAMP_WithInvalidToken_ThrowsInvalidOperationException()
    {
        Assert.Throws<ArgumentException>(() => _encryptToken.EncryptETAMP(_jsonEtampInvalid));
    }
}