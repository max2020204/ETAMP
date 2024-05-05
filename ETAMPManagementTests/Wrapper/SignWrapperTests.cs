#region

using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Models;
using ETAMPManagement.Wrapper;
using Moq;
using Newtonsoft.Json;
using Xunit;

#endregion

namespace ETAMPManagementTests.Wrapper;

public class SignWrapperTests
{
    private readonly Mock<IECDsaProvider> _providerMock;
    private readonly SignWrapper _signWrapper;
    
    public SignWrapperTests()
    {
        _providerMock = new Mock<IECDsaProvider>();
        _providerMock.Setup(x => x.GetECDsa()).Returns(ECDsa.Create());
        _signWrapper = new SignWrapper();
        _signWrapper.Initialize(_providerMock.Object, HashAlgorithmName.SHA256);
    }

    [Fact]
    public void SignEtamp_WithValidJson_ReturnsUpdatedEtampModel()
    {
        var etampModel = new ETAMPModel
        {
            Id = Guid.NewGuid(),
            Token = "someToken"
        };
        var jsonEtamp = JsonConvert.SerializeObject(etampModel);

        var signedJson = _signWrapper.SignEtamp(jsonEtamp);
        var signedEtampModel = JsonConvert.DeserializeObject<ETAMPModel>(signedJson);

        Assert.NotNull(signedEtampModel);
        Assert.NotNull(signedEtampModel.SignatureToken);
        Assert.NotNull(signedEtampModel.SignatureMessage);

        Assert.NotEmpty(signedEtampModel.SignatureToken);
        Assert.NotEmpty(signedEtampModel.SignatureMessage);
    }

    [Fact]
    public void SignEtamp_WithNullInput_ThrowsArgumentNullException()
    {
        var ecdsa = ECDsa.Create();
        var signWrapper = new SignWrapper();
        signWrapper.Initialize(_providerMock.Object, HashAlgorithmName.SHA256);
        var exception = Assert.Throws<ArgumentNullException>(() => signWrapper.SignEtamp(""));
        Assert.Equal("etamp", exception.ParamName);
    }

    [Fact]
    public void SignEtampModel_UpdatesSignatureFieldsCorrectly()
    {
        var ecdsa = ECDsa.Create();
        var signWrapper = new SignWrapper();
        signWrapper.Initialize(_providerMock.Object, HashAlgorithmName.SHA256);
        var etampModel = new ETAMPModel
        {
            Id = Guid.NewGuid(),
            Version = 1.0,
            Token = "testToken",
            UpdateType = "updateType"
        };

        var signedJson = signWrapper.SignEtamp(etampModel);
        var updatedEtampModel = JsonConvert.DeserializeObject<ETAMPModel>(signedJson);

        Assert.NotNull(updatedEtampModel);
        Assert.NotNull(updatedEtampModel.SignatureToken);
        Assert.NotNull(updatedEtampModel.SignatureMessage);
    }

    [Fact]
    public void SignEtampModel_UpdatesSignatureFields()
    {
        var etamp = new ETAMPModel
        {
            Id = Guid.NewGuid(),
            Version = 1.0,
            Token = "token",
            UpdateType = "update",
            SignatureMessage = "",
            SignatureToken = ""
        };

        var signedEtamp = _signWrapper.SignEtampModel(etamp);

        Assert.NotNull(signedEtamp);
        Assert.NotNull(signedEtamp.SignatureToken);
        Assert.NotNull(signedEtamp.SignatureMessage);
        Assert.NotEmpty(signedEtamp.SignatureToken);
        Assert.NotEmpty(signedEtamp.SignatureMessage);
    }
}