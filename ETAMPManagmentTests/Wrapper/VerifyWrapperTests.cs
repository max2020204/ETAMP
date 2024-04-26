using System.Security.Cryptography;
using System.Text;
using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using Moq;
using Xunit;

namespace ETAMPManagment.Wrapper.Tests;

public class VerifyWrapperTests
{
    private readonly string _data = "testdata";
    private readonly Mock<IECDsaProvider> _providerMock;
    private readonly ECDsa _ecdsa;
    private readonly byte[] _signature;
    private readonly VerifyWrapper _verifyWrapper;

    public VerifyWrapperTests()
    {
        _providerMock = new Mock<IECDsaProvider>();
        _ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);

        _providerMock.Setup(x => x.GetECDsa()).Returns(_ecdsa);

        _verifyWrapper = new VerifyWrapper();
        _verifyWrapper.Initialize(_providerMock.Object, HashAlgorithmName.SHA256);
        _signature = _ecdsa.SignData(Encoding.UTF8.GetBytes(_data), HashAlgorithmName.SHA256);
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