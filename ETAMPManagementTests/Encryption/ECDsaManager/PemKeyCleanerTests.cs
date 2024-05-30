using ETAMPManagement.Encryption.ECDsaManager;
using Xunit;

namespace ETAMPManagementTests.Encryption.ECDsaManager;

public class PemKeyCleanerTests
{
    [Fact]
    public void ClearPEMPrivateKey_ShouldRemovePEMFormatting()
    {
        var cleaner = new PemKeyCleaner();
        var privateKeyWithPEM = "-----BEGIN PRIVATE KEY-----\nMIIEvAIBADANBgkqh...\r-----END PRIVATE KEY-----";

        cleaner.ClearPemPrivateKey(privateKeyWithPEM);

        Assert.DoesNotContain("-----BEGIN PRIVATE KEY-----", cleaner.KeyModelProvider.PrivateKey);
        Assert.DoesNotContain("-----END PRIVATE KEY-----", cleaner.KeyModelProvider.PrivateKey);
        Assert.DoesNotContain("\n", cleaner.KeyModelProvider.PrivateKey);
        Assert.DoesNotContain("\r", cleaner.KeyModelProvider.PrivateKey);
    }

    [Fact]
    public void ClearPEMPublicKey_ShouldRemovePEMFormatting()
    {
        var cleaner = new PemKeyCleaner();
        var publicKeyWithPEM =
            "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8A...\r-----END PUBLIC KEY-----";

        cleaner.ClearPemPublicKey(publicKeyWithPEM);

        Assert.DoesNotContain("-----BEGIN PUBLIC KEY-----", cleaner.KeyModelProvider.PublicKey);
        Assert.DoesNotContain("-----END PUBLIC KEY-----", cleaner.KeyModelProvider.PublicKey);
        Assert.DoesNotContain("\n", cleaner.KeyModelProvider.PublicKey);
        Assert.DoesNotContain("\r", cleaner.KeyModelProvider.PublicKey);
    }

    [Fact]
    public void ClearPEMPrivateKey_ShouldReturnSelfForChaining()
    {
        var cleaner = new PemKeyCleaner();
        var privateKeyWithPEM = "some key";

        var result = cleaner.ClearPemPrivateKey(privateKeyWithPEM);

        Assert.Same(cleaner, result);
    }

    [Fact]
    public void ClearPEMPublicKey_ShouldReturnSelfForChaining()
    {
        var cleaner = new PemKeyCleaner();
        var publicKeyWithPEM = "some key";

        var result = cleaner.ClearPemPublicKey(publicKeyWithPEM);

        Assert.Same(cleaner, result);
    }
}