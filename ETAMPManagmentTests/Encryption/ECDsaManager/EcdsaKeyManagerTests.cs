using System.Security.Cryptography;
using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using Moq;
using Xunit;

namespace ETAMPManagment.Encryption.ECDsaManager.Tests;

public class EcdsaKeyManagerTests
{
    private readonly ECCurve _curve = ECCurve.NamedCurves.nistP256;
    private readonly byte[] _dummyPrivateKey;
    private readonly EcdsaKeyManager _keyManager;
    private readonly Mock<IECDsaRegistrar> _mockRegistrar;

    public EcdsaKeyManagerTests()
    {
        _mockRegistrar = new Mock<IECDsaRegistrar>();
        var ecdsa = ECDsa.Create();
        _dummyPrivateKey = ecdsa.ExportPkcs8PrivateKey();
        _keyManager = new EcdsaKeyManager(_mockRegistrar.Object);
    }

    [Fact]
    public void ImportECDsa_WithByteArrayAndCurve_ShouldRegisterECDsa()
    {
        _mockRegistrar.Setup(r => r.RegisterEcdsa(It.IsAny<ECDsa>()))
            .Returns(new ECDsaProvider());

        var provider = _keyManager.ImportECDsa(_dummyPrivateKey, _curve);

        _mockRegistrar.Verify(r => r.RegisterEcdsa(It.IsAny<ECDsa>()), Times.Once);
        Assert.NotNull(provider);
    }

    [Fact]
    public void ImportECDsa_WithByteArray_ShouldRegisterECDsa()
    {
        _mockRegistrar.Setup(r => r.RegisterEcdsa(It.IsAny<ECDsa>()))
            .Returns(new ECDsaProvider());

        var provider = _keyManager.ImportECDsa(_dummyPrivateKey);

        _mockRegistrar.Verify(r => r.RegisterEcdsa(It.IsAny<ECDsa>()), Times.Once);
        Assert.NotNull(provider);
    }

    [Fact]
    public void ImportECDsa_WithBase64StringAndCurve_ShouldRegisterECDsa()
    {
        _mockRegistrar.Setup(r => r.RegisterEcdsa(It.IsAny<ECDsa>()))
            .Returns(new ECDsaProvider());

        var provider = _keyManager.ImportECDsa(Convert.ToBase64String(_dummyPrivateKey), _curve);

        _mockRegistrar.Verify(r => r.RegisterEcdsa(It.IsAny<ECDsa>()), Times.Once);
        Assert.NotNull(provider);
    }

    [Fact]
    public void ImportECDsa_WithBase64String_ShouldRegisterECDsa()
    {
        _mockRegistrar.Setup(r => r.RegisterEcdsa(It.IsAny<ECDsa>()))
            .Returns(new ECDsaProvider());

        var provider = _keyManager.ImportECDsa(Convert.ToBase64String(_dummyPrivateKey));

        _mockRegistrar.Verify(r => r.RegisterEcdsa(It.IsAny<ECDsa>()), Times.Once);
        Assert.NotNull(provider);
    }
}