using ETAMPManagement.Management;
using ETAMPManagement.Models;
using ETAMPManagement.Validators;
using JetBrains.Annotations;
using Xunit;

namespace ETAMPManagementTests.Validators;

[TestSubject(typeof(TokenValidator))]
public class TokenValidatorTest
{
    private readonly TokenValidator _validator = new();

    [Fact]
    public void ValidateToken_GivenNullCompressionType_ReturnsFalseWithCompressionTypeError()
    {
        // Arrange
        var model = new ETAMPModel<Token> { CompressionType = null };

        // Act
        var result = _validator.ValidateToken(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("ETAMPBuilder type is null", result.ErrorMessage);
    }

    [Fact]
    public void ValidateToken_GivenNullToken_ReturnsFalseWithTokenNullError()
    {
        // Arrange
        var model = new ETAMPModel<Token>
        {
            CompressionType = CompressionNames.Deflate
        };

        // Act
        var result = _validator.ValidateToken(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("Token is null.", result.ErrorMessage);
    }

    [Fact]
    public void ValidateToken_GivenUnmatchingMessageId_ReturnsFalseWithMessageIdMatchingError()
    {
        // Arrange
        var model = new ETAMPModel<Token>
        {
            Id = Guid.NewGuid(),
            CompressionType = CompressionNames.Deflate,
            Token = new Token { MessageId = Guid.NewGuid() }
        };

        // Act
        var result = _validator.ValidateToken(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("MessageId does not match the model Id.", result.ErrorMessage);
    }

    [Fact]
    public void ValidateToken_GivenEmptyTokenId_ReturnsFalseWithTokenIdEmptyError()
    {
        // Arrange
        var model = new ETAMPModel<Token>
        {
            Id = Guid.Empty,
            CompressionType = CompressionNames.Deflate,
            Token = new Token { Id = Guid.Empty, MessageId = Guid.Empty }
        };

        // Act
        var result = _validator.ValidateToken(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("Token Id cannot be empty.", result.ErrorMessage);
    }

    [Fact]
    public void ValidateToken_GivenValidModel_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = new ETAMPModel<Token>
        {
            Id = id,
            CompressionType = "valid",
            Token = new Token { Id = Guid.NewGuid(), MessageId = id }
        };

        // Act
        var result = _validator.ValidateToken(model);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal("", result.ErrorMessage);
    }
}