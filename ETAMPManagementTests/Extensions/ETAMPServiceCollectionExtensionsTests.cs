#region

using ETAMPManagement.Encryption;
using ETAMPManagement.Encryption.ECDsaManager;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Encryption.Interfaces;
using ETAMPManagement.ETAMP.Base;
using ETAMPManagement.ETAMP.Base.Interfaces;
using ETAMPManagement.ETAMP.Encrypted;
using ETAMPManagement.ETAMP.Encrypted.Interfaces;
using ETAMPManagement.Extensions;
using ETAMPManagement.Factory;
using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Services;
using ETAMPManagement.Services.Interfaces;
using ETAMPManagement.Validators;
using ETAMPManagement.Validators.Interfaces;
using ETAMPManagement.Wrapper;
using ETAMPManagement.Wrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

#endregion

namespace ETAMPManagementTests.Extensions;

public class ETAMPServiceCollectionExtensionsTests
{
    [Fact]
    public void AddETAMPServices_RegistersExpectedServices()
    {
        var services = new ServiceCollection();

        services.AddETAMPServices();

        // Assert for Transient services
        Assert.Contains(services,
            s => s.ServiceType == typeof(IKeyExchanger) && s.ImplementationType == typeof(KeyExchanger) &&
                 s.Lifetime == ServiceLifetime.Transient);
        Assert.Contains(services,
            s => s.ServiceType == typeof(IKeyPairProvider) && s.ImplementationType == typeof(KeyPairProvider) &&
                 s.Lifetime == ServiceLifetime.Transient);
        Assert.Contains(services,
            s => s.ServiceType == typeof(ISigningCredentialsProvider) &&
                 s.ImplementationType == typeof(ECDsaSigningCredentialsProvider) &&
                 s.Lifetime == ServiceLifetime.Transient);

        // Assert for Scoped services
        Assert.Contains(services,
            s => s.ServiceType == typeof(IEciesEncryptionService) &&
                 s.ImplementationType == typeof(EciesEncryptionService) 
                 && s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IECDsaRegistrar) 
                 && s.ImplementationType == typeof(ECDsaProvider) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IECDsaProvider) 
                 && s.ImplementationType == typeof(ECDsaProvider) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IECDsaCreator) 
                 && s.ImplementationType == typeof(ECDsaCreator) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IECDsaKeyManager) 
                 && s.ImplementationType == typeof(ECDsaKeyManager) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IPemKeyCleaner) 
                 && s.ImplementationType == typeof(PemKeyCleaner) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IETAMPBase) 
                 && s.ImplementationType == typeof(ETAMPBase) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IETAMPData) 
                 && s.ImplementationType == typeof(ETAMPData) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IEncryptToken) 
                 && s.ImplementationType == typeof(EncryptToken) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ImplementationType == typeof(DeflateCompressionService) 
                 && s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ImplementationType == typeof(GZipCompressionService) 
                 && s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IKeyPairProviderFactory) &&
                 s.ImplementationType == typeof(KeyPairProviderFactory) 
                 && s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IETAMPValidator) 
                 && s.ImplementationType == typeof(ETAMPValidator) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IJwtValidator) 
                 && s.ImplementationType == typeof(JwtValidator) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(ISignatureValidator) 
                 && s.ImplementationType == typeof(SignatureValidator) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IStructureValidator) 
                 && s.ImplementationType == typeof(StructureValidator) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(ISignWrapper) 
                 && s.ImplementationType == typeof(SignWrapper) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(IVerifyWrapper) 
                 && s.ImplementationType == typeof(VerifyWrapper) &&
                 s.Lifetime == ServiceLifetime.Scoped);
        
        Assert.Contains(services,
            s => s.ServiceType == typeof(ICompressionServiceFactory) &&
                 s.ImplementationType == typeof(CompressionServiceFactory) 
                 && s.Lifetime == ServiceLifetime.Scoped);
    }
}