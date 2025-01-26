#region

using ETAMP.Compression.Codec;
using ETAMP.Compression.Factory;
using ETAMP.Compression.Interfaces;
using ETAMP.Core.Management;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

#endregion

namespace ETAMP.Compression.Tests.Factory;

[TestSubject(typeof(CompressionServiceFactory))]
public class CompressionServiceFactoryTests
{
    private readonly CompressionServiceFactory _factory;
    private readonly IServiceProvider _serviceProvider;

    public CompressionServiceFactoryTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        // Mock the services to be added to the service provider
        var deflateMock = new Mock<ICompressionService>();
        var gzipMock = new Mock<ICompressionService>();
        var loggerMock = new Mock<ILogger<CompressionServiceFactory>>();

        // Add these services to the service collection
        serviceCollection.AddTransient(_ => deflateMock.Object)
            .AddSingleton<DeflateCompressionService>();
        serviceCollection.AddTransient(_ => gzipMock.Object)
            .AddSingleton<GZipCompressionService>();

        _serviceProvider = serviceCollection.BuildServiceProvider();

        _factory = new CompressionServiceFactory(_serviceProvider, loggerMock.Object);
    }

    [Fact]
    public void Create_ValidCompressionType_ReturnsService()
    {
        var service = _factory.Create(CompressionNames.Deflate);
        Assert.NotNull(service);
    }

    [Fact]
    public void Create_InvalidCompressionType_ThrowsKeyNotFoundException()
    {
        Assert.Throws<KeyNotFoundException>(() => _factory.Create("InvalidType"));
    }

    [Fact]
    public void RegisterCompressionService_ValidService_AddsService()
    {
        var newServiceMock = new Mock<ICompressionService>();
        _factory.RegisterCompressionService("NewType", newServiceMock.Object);
        var service = _factory.Create("NewType");

        Assert.Equal(newServiceMock.Object, service);
    }

    [Fact]
    public void UnregisterCompressionService_ExistingType_ReturnsTrue()
    {
        var result = _factory.UnregisterCompressionService(CompressionNames.Deflate);
        Assert.True(result);
        Assert.Throws<KeyNotFoundException>(() => _factory.Create(CompressionNames.Deflate));
    }

    [Fact]
    public void UnregisterCompressionService_NonExistingType_ReturnsFalse()
    {
        var result = _factory.UnregisterCompressionService("NonExistingType");
        Assert.False(result);
    }
}