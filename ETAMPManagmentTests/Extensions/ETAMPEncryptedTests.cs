#region

using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Models;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagment.Extensions.Tests;

public class ETAMPEncryptedTests
{
    private readonly string _encryptedToken = "encrypted_token";
    private readonly Mock<IEciesEncryptionService> _encryptionServiceMock;
    private readonly ETAMPModel _model;

    public ETAMPEncryptedTests()
    {
        _encryptionServiceMock = new Mock<IEciesEncryptionService>();
        _encryptionServiceMock.Setup(e => e.Encrypt(It.IsAny<string>())).Returns(_encryptedToken);

        _model = new ETAMPModel
        {
            Token = "original_token",
            UpdateType = "update",
            Version = 1.0
        };
    }

    [Fact]
    public void EncryptToken_ShouldEncryptTokenAndReturnModel()
    {
        var result = _model.EncryptToken(_encryptionServiceMock.Object);

        _encryptionServiceMock.Verify(e => e.Encrypt("original_token"), Times.Once);
        Assert.Equal(_encryptedToken, result.Token);
        Assert.Equal("update", result.UpdateType);
        Assert.Equal(1.0, result.Version);
    }

    [Fact]
    public void EncryptToken_WithNullToken_ShouldThrowArgumentException()
    {
        _model.Token = null;
        Assert.Throws<ArgumentNullException>(() => _model.EncryptToken(_encryptionServiceMock.Object));
    }

    [Fact]
    public void EncryptToken_WithNullService_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _model.EncryptToken(null));
    }
}