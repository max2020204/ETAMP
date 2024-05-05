#region

using ETAMPManagement.Extensions;
using ETAMPManagement.Models;
using ETAMPManagement.Wrapper.Interfaces;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagementTests.Extensions;

public class ETAMPSignTests
{
    private readonly ETAMPModel _etampModel;
    private readonly Mock<ISignWrapper> _signWrapperMock;

    public ETAMPSignTests()
    {
        _signWrapperMock = new Mock<ISignWrapper>();
        _etampModel = new ETAMPModel
        {
            Id = Guid.NewGuid(),
            Version = 1.0,
            Token = "sampleToken",
            UpdateType = "Update",
            SignatureToken = "",
            SignatureMessage = ""
        };
    }

    [Fact]
    public void Sign_NullModel_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ETAMPSign.Sign(null, _signWrapperMock.Object));
    }

    [Fact]
    public void Sign_NullSignWrapper_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _etampModel.Sign(null));
    }

    [Fact]
    public void Sign_ValidInputs_ReturnsSignedModel()
    {
        var signedModel = new ETAMPModel
        {
            Id = _etampModel.Id,
            Version = _etampModel.Version,
            Token = _etampModel.Token,
            UpdateType = _etampModel.UpdateType,
            SignatureToken = "signedToken",
            SignatureMessage = "signedMessage"
        };

        _signWrapperMock.Setup(sw => sw.SignEtampModel(_etampModel)).Returns(signedModel);

        var result = _etampModel.Sign(_signWrapperMock.Object);

        Assert.NotNull(result);
        Assert.Equal("signedToken", result.SignatureToken);
        Assert.Equal("signedMessage", result.SignatureMessage);
        _signWrapperMock.Verify(sw => sw.SignEtampModel(_etampModel), Times.Once);
    }
}