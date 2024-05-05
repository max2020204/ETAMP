#region

using System.Security.Cryptography;
using ETAMPManagement.Encryption;
using Xunit;

#endregion

namespace ETAMPManagementTests.Encryption;

public class KeyPairProviderTests
{
    private readonly ECDiffieHellman _ecDiffieHellman;
    private readonly KeyPairProvider _keyPairProvider;

    public KeyPairProviderTests()
    {
        _ecDiffieHellman = ECDiffieHellman.Create();
        _keyPairProvider = new KeyPairProvider();
        _keyPairProvider.Initialize(_ecDiffieHellman);
    }

    [Fact]
    public void HellmanPublicKey_ShouldMatchPublicKeyFromECDiffieHellman()
    {
        using var eCDiffieHellman = ECDiffieHellman.Create();
        var provider = new KeyPairProvider();
        provider.Initialize(eCDiffieHellman);
        Assert.Equal(eCDiffieHellman.PublicKey.ExportSubjectPublicKeyInfo(),
            provider.HellmanPublicKey.ExportSubjectPublicKeyInfo());
    }

    [Fact]
    public void Constructor_WithECDiffieHellman_ShouldSetPropertiesCorrectly()
    {
        using var eCDiffieHellman = ECDiffieHellman.Create();
        var provider = new KeyPairProvider();
        provider.Initialize(eCDiffieHellman);

        Assert.NotNull(provider.KeyModelProvider.PrivateKey);
        Assert.NotNull(provider.KeyModelProvider.PublicKey);
    }

    [Fact]
    public void GetECDiffieHellman_ReturnsNotNull()
    {
        var result = _keyPairProvider.GetECDiffieHellman();
        Assert.NotNull(result);
        Assert.Equal(_ecDiffieHellman, result);
    }

    [Fact]
    public void ImportPrivateKey_ValidPrivateKey_ImportsSuccessfully()
    {
        var privateKey = _ecDiffieHellman.ExportPkcs8PrivateKey();
        _keyPairProvider.ImportPrivateKey(privateKey);

        Assert.NotNull(_keyPairProvider.KeyModelProvider.PrivateKey);
        Assert.NotNull(_keyPairProvider.KeyModelProvider.PublicKey);
    }

    [Fact]
    public void ImportPublicKey_ValidPublicKey_ImportsSuccessfully()
    {
        var publicKey = _ecDiffieHellman.ExportSubjectPublicKeyInfo();
        _keyPairProvider.ImportPublicKey(publicKey);

        Assert.NotNull(_keyPairProvider.KeyModelProvider.PublicKey);
    }
}