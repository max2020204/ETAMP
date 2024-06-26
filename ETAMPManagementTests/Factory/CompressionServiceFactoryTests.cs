﻿using ETAMPManagement.Codec;
using ETAMPManagement.Codec.Interfaces;
using ETAMPManagement.Factory;
using ETAMPManagement.Management;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace ETAMPManagementTests.Factory;

public class CompressionServiceFactoryTests
{
    private readonly CompressionServiceFactory _factory;
    private readonly IServiceProvider _serviceProvider;

    public CompressionServiceFactoryTests()
    {
        var serviceCollection = new ServiceCollection();

        // Mock the services to be added to the service provider
        var deflateMock = new Mock<ICompressionService>();
        var gzipMock = new Mock<ICompressionService>();

        // Add these services to the service collection
        serviceCollection.AddTransient(_ => deflateMock.Object)
            .AddSingleton<DeflateCompressionService>();
        serviceCollection.AddTransient(_ => gzipMock.Object)
            .AddSingleton<GZipCompressionService>();

        _serviceProvider = serviceCollection.BuildServiceProvider();

        _factory = new CompressionServiceFactory(_serviceProvider);
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