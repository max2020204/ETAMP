using AutoFixture.Xunit2;
using ETAMP.Compression.Codec;
using ETAMP.Compression.Interfaces;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Factories;
using ETAMP.Core.Interfaces;
using ETAMP.Core.Management;
using ETAMP.Core.Models;
using ETAMP.Core.Utils;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;

namespace ETAMP.Compression.Tests;

[TestSubject(typeof(CompressionManager))]
public class CompressionManagerTest
{
    private CompressionManager _compressionManager;
    private Mock<ILogger<ICompressionManager>> _logger;
    private Mock<ILogger<DeflateCompressionService>> _loggerDeflate;
    private Mock<ICompressionServiceFactory> _compressionServiceFactory;

    public CompressionManagerTest()
    {
        _logger = new Mock<ILogger<ICompressionManager>>();
        _loggerDeflate = new Mock<ILogger<DeflateCompressionService>>();
        _compressionServiceFactory = new Mock<ICompressionServiceFactory>();
        _compressionManager = new CompressionManager(_compressionServiceFactory.Object, _logger.Object);
    }

    [Theory, AutoData]
    public async Task CompressAsync_CompressesCorrectly(string input)
    {
        _compressionServiceFactory.Setup(x => x.Get(CompressionNames.Deflate))
            .Returns(new DeflateCompressionService(_loggerDeflate.Object));


        var token = new Token
        {
            Data = input
        };
        IETAMPBase etampBase = new ETAMPModelFactory(new VersionInfo());
        var model = etampBase.CreateETAMPModel("update", token, CompressionNames.Deflate);

        var builder = await _compressionManager.CompressAsync(model);


        Assert.NotEqual(Guid.Empty, builder.Id);
        Assert.NotEqual(0, builder.Version);

        Assert.NotNull(builder.UpdateType);
        Assert.False(string.IsNullOrWhiteSpace(builder.UpdateType));
        Assert.Equal("update", builder.UpdateType);

        Assert.NotNull(builder.Token);
        Assert.False(string.IsNullOrWhiteSpace(builder.Token));

        Assert.NotNull(builder.CompressionType);
        Assert.False(string.IsNullOrWhiteSpace(builder.CompressionType));
        Assert.Equal(CompressionNames.Deflate, builder.CompressionType);
    }
}