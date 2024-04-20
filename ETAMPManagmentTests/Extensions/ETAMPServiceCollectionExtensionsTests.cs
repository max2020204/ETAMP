using ETAMPManagment.Encryption;
using ETAMPManagment.Encryption.ECDsaManager;
using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Base;
using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.ETAMP.Encrypted;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Factory;
using ETAMPManagment.Factory.Interfaces;
using ETAMPManagment.Services;
using ETAMPManagment.Services.Interfaces;
using ETAMPManagment.Validators;
using ETAMPManagment.Validators.Interfaces;
using ETAMPManagment.Wrapper;
using ETAMPManagment.Wrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ETAMPManagment.Extensions.Tests
{
    public class ETAMPServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddETAMPServices_RegistersExpectedServices()
        {
            var services = new ServiceCollection();

            services.AddETAMPServices();

            // Assert for Transient services
            Assert.Contains(services, s => s.ServiceType == typeof(IKeyExchanger) && s.ImplementationType == typeof(KeyExchanger) && s.Lifetime == ServiceLifetime.Transient);
            Assert.Contains(services, s => s.ServiceType == typeof(IKeyPairProvider) && s.ImplementationType == typeof(KeyPairProvider) && s.Lifetime == ServiceLifetime.Transient);
            Assert.Contains(services, s => s.ServiceType == typeof(ISigningCredentialsProvider) && s.ImplementationType == typeof(ECDsaSigningCredentialsProvider) && s.Lifetime == ServiceLifetime.Transient);

            // Assert for Scoped services
            Assert.Contains(services, s => s.ServiceType == typeof(IEciesEncryptionService) && s.ImplementationType == typeof(EciesEncryptionService) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IECDsaRegistrar) && s.ImplementationType == typeof(ECDsaProvider) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IECDsaProvider) && s.ImplementationType == typeof(ECDsaProvider) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IEcdsaCreator) && s.ImplementationType == typeof(EcdsaCreator) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IEcdsaKeyManager) && s.ImplementationType == typeof(EcdsaKeyManager) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IPemKeyCleaner) && s.ImplementationType == typeof(PemKeyCleaner) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IETAMPBase) && s.ImplementationType == typeof(ETAMPBase) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IETAMPData) && s.ImplementationType == typeof(ETAMPData) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IEncryptToken) && s.ImplementationType == typeof(EncryptToken) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(ICompressionService) && s.ImplementationType == typeof(DeflateCompressionService) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(ICompressionService) && s.ImplementationType == typeof(GZipCompressionService) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IKeyPairProviderFactory) && s.ImplementationType == typeof(KeyPairProviderFactory) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IETAMPValidator) && s.ImplementationType == typeof(ETAMPValidator) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IJwtValidator) && s.ImplementationType == typeof(JwtValidator) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(ISignatureValidator) && s.ImplementationType == typeof(SignatureValidator) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IStructureValidator) && s.ImplementationType == typeof(StructureValidator) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(ISignWrapper) && s.ImplementationType == typeof(SignWrapper) && s.Lifetime == ServiceLifetime.Scoped);
            Assert.Contains(services, s => s.ServiceType == typeof(IVerifyWrapper) && s.ImplementationType == typeof(VerifyWrapper) && s.Lifetime == ServiceLifetime.Scoped);
            // Assert for Singleton services
            Assert.Contains(services, s => s.ServiceType == typeof(ICompressionServiceFactory) && s.ImplementationType == typeof(CompressionServiceFactory) && s.Lifetime == ServiceLifetime.Singleton);
        }
    }
}