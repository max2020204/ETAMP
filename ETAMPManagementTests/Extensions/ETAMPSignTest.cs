using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Extensions;
using ETAMPManagement.Models;
using ETAMPManagement.Wrapper.Interfaces;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace ETAMPManagementTests.Extensions;

[TestSubject(typeof(ETAMPSign))]
public class ETAMPSignTest
{
    private readonly Mock<IECDsaProvider> _eCDsaProviderMock;
    private readonly Mock<ISignWrapper> _signWrapperMock;

    public ETAMPSignTest()
    {
        _signWrapperMock = new Mock<ISignWrapper>();
        _eCDsaProviderMock = new Mock<IECDsaProvider>();
        _signWrapperMock.Setup(s => s.Initialize(_eCDsaProviderMock.Object, HashAlgorithmName.SHA256));
    }

    [Fact]
    public void Sign_NullModel_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ((ETAMPModel<Token>?)null!).Sign(_signWrapperMock.Object));
    }

    [Fact]
    public void Sign_NullISignWrapper_ArgumentNullException()
    {
        var model = new ETAMPModel<Token>();
        Assert.Throws<ArgumentNullException>(() => model.Sign(null!));
    }

    [Fact]
    public void Sign_ModelAndISignWrapper_SignWrapperCalled()
    {
        var model = new ETAMPModel<Token>();
        var signedModel = model.Sign(_signWrapperMock.Object);
        _signWrapperMock.Verify(s => s.SignEtampModel(It.IsAny<ETAMPModel<Token>>()), Times.Once);
    }
}