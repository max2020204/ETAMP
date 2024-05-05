#region

using System.Security.Cryptography;
using System.Text;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Wrapper;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagementTests.Wrapper;

public class VerifyWrapperTests
{
    private readonly string _data = "testdata";
    private readonly byte[] _signature;
    private readonly VerifyWrapper _verifyWrapper;

    public VerifyWrapperTests()
    {
        Mock<IECDsaProvider> providerMock = new();
        var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);

        providerMock.Setup(x => x.GetECDsa()).Returns(ecdsa);

        _verifyWrapper = new VerifyWrapper();
        _verifyWrapper.Initialize(providerMock.Object, HashAlgorithmName.SHA256);
        _signature = ecdsa.SignData(Encoding.UTF8.GetBytes(_data), HashAlgorithmName.SHA256);
    }

    [Fact]
    public void VerifyData_WithCorrectDataInBytesAndSignInBytes_ReturnTrue()
    {
        var result = _verifyWrapper.VerifyData(Encoding.UTF8.GetBytes(_data), _signature);
        Assert.True(result);
    }

    [Fact]
    public void VerifyData_WithCorrectDataInStringAndSignInString_ReturnTrue()
    {
        var result = _verifyWrapper.VerifyData(_data, Convert.ToBase64String(_signature));
        Assert.True(result);
    }

    [Fact]
    public void VerifyData_WithIncorrectSignature_ReturnsFalse()
    {
        var incorrectSignature = new byte[] { 1, 2, 3 };
        var result = _verifyWrapper.VerifyData(Encoding.UTF8.GetBytes(_data), incorrectSignature);
        Assert.False(result);
    }
}