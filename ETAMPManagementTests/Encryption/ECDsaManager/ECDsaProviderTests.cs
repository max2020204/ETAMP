#region

using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager;
using Xunit;

#endregion

namespace ETAMPManagementTests.Encryption.ECDsaManager;

public class EcdsaProviderTests
{
    [Fact]
    public void RegisterEcdsa_ShouldSetEcdsaInstance()
    {
        var provider = new ECDsaProvider();
        var ecdsa = ECDsa.Create();

        var returnedProvider = provider.RegisterECDsa(ecdsa);

        Assert.Same(ecdsa, provider.GetECDsa());
        Assert.Same(provider, returnedProvider);
    }
}