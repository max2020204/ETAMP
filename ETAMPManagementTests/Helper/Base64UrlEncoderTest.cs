using ETAMPManagement.Helper;
using Xunit;

namespace ETAMPManagementTests.Helper;

public class Base64UrlEncoderTest
{
    //Test cases for Encode(string arg)
    [Fact]
    public void Encode_String_ShouldReturnEncodedBase64UrlString()
    {
        //Arrange
        var arg = "Hello World";
        //Act
        var encoded = Base64UrlEncoder.Encode(arg);
        //Assert
        Assert.Equal("SGVsbG8gV29ybGQ", encoded);
    }

    [Fact]
    public void Encode_NullString_ShouldThrowArgumentException()
    {
        //Arrange
        string arg = null;
        //Act and Assert
        Assert.Throws<ArgumentException>(() => Base64UrlEncoder.Encode(arg));
    }

    [Fact]
    public void Encode_ByteArray_ShouldReturnEncodedBase64UrlString()
    {
        //Arrange
        byte[] bytes = { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 };
        //Act
        var encoded = Base64UrlEncoder.Encode(bytes);
        //Assert
        Assert.Equal("SGVsbG8gV29ybGQ", encoded);
    }

    [Fact]
    public void Encode_NullByteArray_ShouldThrowArgumentNullException()
    {
        //Arrange
        byte[] bytes = null;
        //Act and Assert
        Assert.Throws<ArgumentNullException>(() => Base64UrlEncoder.Encode(bytes));
    }

    //Test cases for DecodeBytes(string? str)
    [Fact]
    public void DecodeBytes_Base64UrlEncodedString_ShouldReturnDecodedByteArray()
    {
        //Arrange
        var encoded = "SGVsbG8gV29ybGQ=";
        //Act
        var decoded = Base64UrlEncoder.DecodeBytes(encoded);
        //Assert
        Assert.Equal(new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 }, decoded);
    }

    [Fact]
    public void DecodeBytes_NullOrEmptyString_ShouldThrowArgumentNullException()
    {
        //Arrange
        var encoded = string.Empty;
        //Act and Assert
        Assert.Throws<ArgumentNullException>(() => Base64UrlEncoder.DecodeBytes(encoded));
    }

    //Test cases for Decode(string? arg)
    [Fact]
    public void Decode_Base64UrlEncodedString_ShouldReturnDecodedString()
    {
        //Arrange
        var encoded = "SGVsbG8gV29ybGQ=";
        //Act
        var decoded = Base64UrlEncoder.Decode(encoded);
        //Assert
        Assert.Equal("Hello World", decoded);
    }

    [Fact]
    public void Decode_NullOrEmptyString_ShouldThrowArgumentNullException()
    {
        //Arrange
        var encoded = string.Empty;
        //Act and Assert
        Assert.Throws<ArgumentNullException>(() => Base64UrlEncoder.Decode(encoded));
    }
}