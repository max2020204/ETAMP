#region

using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagementTests.Encryption.ECDsaManager;

public class ECDsaKeyManagerTests
{
    private readonly ECCurve _curve = ECCurve.NamedCurves.nistP256;
    private readonly byte[] _dummyPrivateKey;
    private readonly ECDsaKeyManager _keyManager;
    private readonly Mock<IECDsaRegistrar> _mockRegistrar;

    public ECDsaKeyManagerTests()
    {
        _mockRegistrar = new Mock<IECDsaRegistrar>();
        var ecdsa = ECDsa.Create();
        _dummyPrivateKey = ecdsa.ExportPkcs8PrivateKey();
        _keyManager = new ECDsaKeyManager(_mockRegistrar.Object);
    }

    [Fact]
    public void ImportECDsa_WithByteArrayAndCurve_ShouldRegisterECDsa()
    {
        _mockRegistrar.Setup(r => r.RegisterECDsa(It.IsAny<ECDsa>()))
            .Returns(new ECDsaProvider());

        var provider = _keyManager.ImportECDsa(_dummyPrivateKey, _curve);

        _mockRegistrar.Verify(r => r.RegisterECDsa(It.IsAny<ECDsa>()), Times.Once);
        Assert.NotNull(provider);
    }

    [Fact]
    public void ImportECDsa_WithByteArray_ShouldRegisterECDsa()
    {
        _mockRegistrar.Setup(r => r.RegisterECDsa(It.IsAny<ECDsa>()))
            .Returns(new ECDsaProvider());

        var provider = _keyManager.ImportECDsa(_dummyPrivateKey);

        _mockRegistrar.Verify(r => r.RegisterECDsa(It.IsAny<ECDsa>()), Times.Once);
        Assert.NotNull(provider);
    }

    [Fact]
    public void ImportECDsa_WithBase64StringAndCurve_ShouldRegisterECDsa()
    {
        _mockRegistrar.Setup(r => r.RegisterECDsa(It.IsAny<ECDsa>()))
            .Returns(new ECDsaProvider());

        var provider = _keyManager.ImportECDsa(Convert.ToBase64String(_dummyPrivateKey), _curve);

        _mockRegistrar.Verify(r => r.RegisterECDsa(It.IsAny<ECDsa>()), Times.Once);
        Assert.NotNull(provider);
    }

    [Fact]
    public void ImportECDsa_WithBase64String_ShouldRegisterECDsa()
    {
        _mockRegistrar.Setup(r => r.RegisterECDsa(It.IsAny<ECDsa>()))
            .Returns(new ECDsaProvider());

        var provider = _keyManager.ImportECDsa(Convert.ToBase64String(_dummyPrivateKey));

        _mockRegistrar.Verify(r => r.RegisterECDsa(It.IsAny<ECDsa>()), Times.Once);
        Assert.NotNull(provider);
    }
}