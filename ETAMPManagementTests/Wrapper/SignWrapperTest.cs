using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Models;
using ETAMPManagement.Wrapper;
using ETAMPManagement.Wrapper.Interfaces;
using Moq;
using Xunit;

namespace ETAMPManagementTests.Wrapper;

public class SignWrapperTest
{
    private readonly ECDsa _fakeECDsa;
    private readonly Mock<IECDsaProvider> _mockECDsaProvider;
    private readonly Mock<ISignWrapper> _mockSignWrapper;
    private readonly SignWrapper _signWrapper;

    public SignWrapperTest()
    {
        _mockECDsaProvider = new Mock<IECDsaProvider>();
        _fakeECDsa = ECDsa.Create();
        _mockECDsaProvider.Setup(x => x.GetECDsa())
            .Returns(_fakeECDsa);
        _mockSignWrapper = new Mock<ISignWrapper>();
        _signWrapper = new SignWrapper();
        _signWrapper.Initialize(_mockECDsaProvider.Object,
            HashAlgorithmName.MD5);
    }

    [Fact]
    public void SignEtampModel_NullToken_ThrowsArgumentNullException()
    {
        // Arrange
        var etampModel = new ETAMPModel<Token> { Token = null };

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() =>
            _signWrapper.SignEtampModel(etampModel));
    }

    [Fact]
    public void SignEtampModel_ValidToken_SuccessfullySigned()
    {
        // Arrange
        var token = new Token();
        var etampModel = new ETAMPModel<Token> { Token = token };

        // Action
        var result = _signWrapper.SignEtampModel(etampModel);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.SignatureMessage);
    }
}