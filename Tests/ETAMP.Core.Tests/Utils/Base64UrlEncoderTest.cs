#region

using System.Text;
using ETAMP.Core.Utils;
using JetBrains.Annotations;

#endregion

namespace ETAMP.Core.Tests.Utils;

[TestSubject(typeof(Base64UrlEncoder))]
public class Base64UrlEncoderTest

{
    [Fact]
    public void Encode_ValidStringInput_ShouldReturnEncodedString()
    {
        // Arrange
        var input = "Hello World!";
        var expectedOutput = "SGVsbG8gV29ybGQh";

        // Act
        var result = Base64UrlEncoder.Encode(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public void Encode_EmptyString_ShouldThrowArgumentException()
    {
        // Arrange
        var input = "";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Base64UrlEncoder.Encode(input));
        Assert.Equal("Value cannot be null or whitespace. (Parameter 'arg')", exception.Message);
    }

    [Fact]
    public void Encode_NullString_ShouldThrowArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Base64UrlEncoder.Encode((string)null!));
        Assert.Equal("Value cannot be null or whitespace. (Parameter 'arg')", exception.Message);
    }

    [Fact]
    public void Encode_ValidByteArray_ShouldReturnEncodedString()
    {
        // Arrange
        var input = Encoding.UTF8.GetBytes("Hello World!");
        var expectedOutput = "SGVsbG8gV29ybGQh";

        // Act
        var result = Base64UrlEncoder.Encode(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public void Encode_NullByteArray_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Base64UrlEncoder.Encode((byte[])null!));
        Assert.Equal("Value cannot be null. (Parameter 'inArray')", exception.Message);
    }

    [Fact]
    public void Encode_EmptyByteArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        var input = Array.Empty<byte>();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Base64UrlEncoder.Encode(input));
        Assert.Equal("Value cannot be null. (Parameter 'inArray')", exception.Message);
    }

    [Fact]
    public void Decode_ValidEncodedString_ShouldReturnOriginalString()
    {
        // Arrange
        var input = "SGVsbG8gV29ybGQh";
        var expectedOutput = "Hello World!";

        // Act
        var result = Base64UrlEncoder.Decode(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public void Decode_InvalidEncodedString_ShouldThrowFormatException()
    {
        // Arrange
        var input = "Invalid_Base64!";

        // Act & Assert
        Assert.Throws<FormatException>(() => Base64UrlEncoder.Decode(input));
    }

    [Fact]
    public void Decode_NullString_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Base64UrlEncoder.Decode(null!));
        Assert.Equal("Value cannot be null. (Parameter 'str')", exception.Message);
    }

    [Fact]
    public void Decode_EmptyString_ShouldThrowArgumentNullException()
    {
        // Arrange
        var input = "";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Base64UrlEncoder.Decode(input));
        Assert.Equal("Value cannot be null. (Parameter 'str')", exception.Message);
    }

    [Fact]
    public void DecodeBytes_ValidEncodedString_ShouldReturnOriginalBytes()
    {
        // Arrange
        var input = "SGVsbG8gV29ybGQh";
        var expectedOutput = Encoding.UTF8.GetBytes("Hello World!");

        // Act
        var result = Base64UrlEncoder.DecodeBytes(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public void DecodeBytes_NullString_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Base64UrlEncoder.DecodeBytes(null!));
        Assert.Equal("Value cannot be null. (Parameter 'str')", exception.Message);
    }

    [Fact]
    public void DecodeBytes_EmptyString_ShouldThrowArgumentNullException()
    {
        // Arrange
        var input = "";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Base64UrlEncoder.DecodeBytes(input));
        Assert.Equal("Value cannot be null. (Parameter 'str')", exception.Message);
    }

    [Fact]
    public void EncodeDecode_CorrectFunctionality_ShouldRoundTripSuccessfully()
    {
        // Arrange
        var originalString = "This is a test string for round-trip!";

        // Act
        var encoded = Base64UrlEncoder.Encode(originalString);
        var decoded = Base64UrlEncoder.Decode(encoded);

        // Assert
        Assert.NotNull(encoded);
        Assert.NotNull(decoded);
        Assert.Equal(originalString, decoded);
    }
}