using ETAMPManagement.Codec;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Encryption.Interfaces;
using ETAMPManagement.ETAMP.Interfaces;
using ETAMPManagement.Extensions;
using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Helper;
using ETAMPManagement.Validators.Interfaces;
using ETAMPManagement.Wrapper.Base;
using ETAMPManagement.Wrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ETAMPManagementTests.Extensions;

public class ETAMPServiceCollectionExtensionsTest
{
    [Fact]
    public void AddETAMPServices_ShouldRegisterAllServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddETAMPServices();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        AssertService<IKeyExchanger>(serviceProvider);
        AssertService<IKeyPairProvider>(serviceProvider);
        AssertService<IEncryptionService>(serviceProvider);
        AssertService<IECIESEncryptionService>(serviceProvider);
        AssertService<IECDsaRegistrar>(serviceProvider);
        AssertService<IECDsaProvider>(serviceProvider);
        AssertService<IECDsaCreator>(serviceProvider);
        AssertService<IECDsaKeyManager>(serviceProvider);
        AssertService<IPemKeyCleaner>(serviceProvider);
        AssertService<IETAMPBase>(serviceProvider);
        AssertService<DeflateCompressionService>(serviceProvider);
        AssertService<GZipCompressionService>(serviceProvider);
        AssertService<ICompressionServiceFactory>(serviceProvider);
        AssertService<IKeyPairProviderFactory>(serviceProvider);
        AssertService<IETAMPValidator>(serviceProvider);
        AssertService<ISignatureValidator>(serviceProvider);
        AssertService<IStructureValidator>(serviceProvider);
        AssertService<ITokenValidator>(serviceProvider);
        AssertService<ISignWrapper>(serviceProvider);
        AssertService<SignWrapperBase>(serviceProvider);
        AssertService<IVerifyWrapper>(serviceProvider);
        AssertService<VersionInfo>(serviceProvider);
    }

    private void AssertService<T>(IServiceProvider serviceProvider)
    {
        var service = serviceProvider.GetService<T>();
        Assert.NotNull(service);
    }
}