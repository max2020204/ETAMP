using ETAMPManagement.Encryption.Interfaces;
using ETAMPManagement.Extensions.Builder;
using ETAMPManagement.Models;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace ETAMPManagementTests.Extensions.Builder;

[TestSubject(typeof(ETAMPEncryption))]
public class ETAMPEncryptionTest
{
    private readonly Mock<IECIESEncryptionService> _mockEciesEncryptionService;
    private readonly ETAMPModel<Token> _model;
    private readonly string _toBeEncrypted = "TestString";

    public ETAMPEncryptionTest()
    {
        _mockEciesEncryptionService = new Mock<IECIESEncryptionService>();

        _model = new ETAMPModel<Token>
        {
            Token = new Token
            {
                Data = _toBeEncrypted
            }
        };
    }

    [Fact]
    public void EncryptData_WhenCalled_ShouldInvokeEncryptionService()
    {
        var encryptedString = "EncryptedString";
        _mockEciesEncryptionService.Setup(s => s.Encrypt(_toBeEncrypted))
            .Returns(encryptedString);

        var result = _model.EncryptData(_mockEciesEncryptionService.Object);

        Assert.Equal(encryptedString, result.Token.Data);
        _mockEciesEncryptionService.Verify(s => s.Encrypt(_toBeEncrypted), Times.Once);
    }

    [Fact]
    public void EncryptData_WhenModelTokenDataIsNull_ThrowsArgumentException()
    {
        _model.Token.Data = null;

        Assert.Throws<ArgumentNullException>(() => _model.EncryptData(_mockEciesEncryptionService.Object));
    }

    [Fact]
    public void EncryptData_WhenEciesEncryptionServiceIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _model.EncryptData(null));
    }
}