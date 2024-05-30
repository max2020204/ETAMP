using ETAMPManagement.Codec;
using ETAMPManagement.Extensions.Builder;
using ETAMPManagement.Factory;
using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Management;
using ETAMPManagement.Models;
using ETAMPManagement.Validators;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace ETAMPManagementTests.Validators;

[TestSubject(typeof(StructureValidator))]
public class StructureValidatorTest
{
    private readonly StructureValidator _structureValidator;

    public StructureValidatorTest()
    {
        var _compressMock = new Mock<ICompressionServiceFactory>();
        _structureValidator = new StructureValidator(_compressMock.Object);
    }

    [Fact]
    public void ValidateETAMP_NullModel_ReturnsInvalid()
    {
        var result = _structureValidator.ValidateETAMP<Token>(model: null);

        Assert.False(result.IsValid);
        Assert.Equal("ETAMP model is null.", result.ErrorMessage);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ValidateETAMP_InvalidModel_ReturnsInvalid(bool validateLite)
    {
        var model = new ETAMPModel<Token>
        {
            Id = Guid.Empty,
            UpdateType = null,
            CompressionType = null,
            Token = null,
            SignatureMessage = validateLite ? null : string.Empty
        };

        var result = _structureValidator.ValidateETAMP(model, validateLite);

        Assert.False(result.IsValid);
        Assert.Equal("ETAMP model has empty/missing fields or contains invalid values.", result.ErrorMessage);
    }

    [Fact]
    public void ValidateETAMP_ValidModel_ReturnsValid()
    {
        var model = new ETAMPModel<Token>
        {
            Id = Guid.NewGuid(),
            UpdateType = "update",
            CompressionType = "compression",
            Token = new Token(),
            SignatureMessage = "signatureMessage"
        };

        var result = _structureValidator.ValidateETAMP(model);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateETAMP_InvalidJson_ReturnsInvalid()
    {
        Assert.Throws<ArgumentException>(() => _structureValidator.ValidateETAMP<Token>("Invalid"));
    }

    [Fact]
    public void ValidateETAMP_ValidJson_ReturnsValid()
    {
        var collection = new ServiceCollection();
        collection.AddScoped<DeflateCompressionService>();
        collection.AddScoped<GZipCompressionService>();
        var provider = collection.BuildServiceProvider();
        var compressionServiceFactory = new CompressionServiceFactory(provider);
        var model = new ETAMPModel<Token>
        {
            Id = Guid.NewGuid(),
            UpdateType = "update",
            CompressionType = CompressionNames.Deflate,
            Token = new Token(),
            SignatureMessage = "signatureMessage"
        }.Build(compressionServiceFactory);
        var structureValidator = new StructureValidator(compressionServiceFactory);
        var result = structureValidator.ValidateETAMP<Token>(model);

        // Assert
        Assert.True(result.IsValid);
    }
}