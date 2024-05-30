using ETAMPManagement.Helper;
using Xunit;

namespace ETAMPManagementTests.Helper;

public class Base64UrlEncoderTest
{
    [Fact]
    public void Encode_String_ShouldReturnEncodedBase64UrlString()
    {
        const string arg = "Hello World";

        var encoded = Base64UrlEncoder.Encode(arg);

        Assert.Equal("SGVsbG8gV29ybGQ", encoded);
    }

    [Fact]
    public void Encode_NullString_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Base64UrlEncoder.Encode(arg: null));
    }

    [Fact]
    public void Encode_ByteArray_ShouldReturnEncodedBase64UrlString()
    {
        var bytes = "Hello World"u8.ToArray();

        var encoded = Base64UrlEncoder.Encode(bytes);

        Assert.Equal("SGVsbG8gV29ybGQ", encoded);
    }

    [Fact]
    public void DecodeBytes_Base64UrlEncodedString_ShouldReturnDecodedByteArray()
    {
        const string encoded = "SGVsbG8gV29ybGQ=";
        var decoded = Base64UrlEncoder.DecodeBytes(encoded);
        Assert.Equal(new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 }, decoded);
    }

    [Fact]
    public void DecodeBytes_NullOrEmptyString_ShouldThrowArgumentNullException()
    {
        var encoded = string.Empty;
        Assert.Throws<ArgumentNullException>(() => Base64UrlEncoder.DecodeBytes(encoded));
    }

    [Fact]
    public void Decode_Base64UrlEncodedString_ShouldReturnDecodedString()
    {
        var encoded = "SGVsbG8gV29ybGQ=";

        var decoded = Base64UrlEncoder.Decode(encoded);

        Assert.Equal("Hello World", decoded);
    }

    [Fact]
    public void Decode_NullOrEmptyString_ShouldThrowArgumentNullException()
    {
        var encoded = string.Empty;
        Assert.Throws<ArgumentNullException>(() => Base64UrlEncoder.Decode(encoded));
    }
}