#region

using System.Security.Cryptography;
using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagment.Encryption.ECDsaManager.Tests;

public class ECDsaCreatorTests
{
    [Fact]
    public void CreateECDsa_ShouldRegisterAndReturnProvider()
    {
        var registrar = new ECDsaProvider();
        var creator = new ECDsaCreator(registrar);

        var provider = creator.CreateECDsa();

        Assert.NotNull(provider.GetECDsa());
    }

    [Fact]
    public void CreateECDsa_WithCurve_ShouldRegisterAndReturnProvider()
    {
        var registrar = new ECDsaProvider();
        var creator = new ECDsaCreator(registrar);
        var curve = ECCurve.NamedCurves.nistP256;

        var provider = creator.CreateECDsa(curve);

        Assert.NotNull(provider.GetECDsa());
    }

    [Fact]
    public void CreateECDsa_WithPublicKeyString_ShouldRegisterECDsa()
    {
        // Arrange
        var mockRegistrar = new Mock<IECDsaRegistrar>();
        var creator = new ECDsaCreator(mockRegistrar.Object);
        var ecdsa = ECDsa.Create();
        var publicKey = ecdsa.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
            .Replace("-----END PUBLIC KEY-----", "")
            .Replace("\n", "")
            .Replace("\r", "");
        var curve = ECCurve.NamedCurves.nistP256;

        mockRegistrar.Setup(r => r.RegisterECDsa(It.IsAny<ECDsa>()))
            .Returns(new ECDsaProvider());

        var result = creator.CreateECDsa(publicKey, curve);

        mockRegistrar.Verify(r => r.RegisterECDsa(It.IsAny<ECDsa>()), Times.Once);
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateECDsa_WithPublicKeyByteArray_ShouldRegisterECDsa()
    {
        var mockRegistrar = new Mock<IECDsaRegistrar>();
        var creator = new ECDsaCreator(mockRegistrar.Object);
        var ecdsa = ECDsa.Create();
        var publicKey = ecdsa.ExportSubjectPublicKeyInfo();
        var curve = ECCurve.NamedCurves.nistP256;

        mockRegistrar.Setup(r => r.RegisterECDsa(It.IsAny<ECDsa>()))
            .Returns(new ECDsaProvider());

        var result = creator.CreateECDsa(publicKey, curve);

        mockRegistrar.Verify(r => r.RegisterECDsa(It.IsAny<ECDsa>()), Times.Once);
        Assert.NotNull(result);
    }
}