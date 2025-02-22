using System.Buffers.Text;
using System.Text;

namespace ETAMP.Core.Utils;

public static class Base64UrlEncoder
{
    public static string Encode(string arg)
    {
        ArgumentNullException.ThrowIfNull(arg);
        var data = Encode(Encoding.UTF8.GetBytes(arg).AsSpan());
        return Encoding.UTF8.GetString(data);
    }

    public static string Encode(byte[] arg)
    {
        ArgumentNullException.ThrowIfNull(arg);
        var data = Encode(arg.AsSpan());
        return Encoding.UTF8.GetString(data);
    }

    public static Span<byte> Encode(ReadOnlySpan<byte> arg)
    {
        var destination = new Span<byte>(new byte[Base64Url.GetEncodedLength(arg.Length)]);
        Base64Url.EncodeToUtf8(arg, destination);
        return destination;
    }


    public static byte[] DecodeBytes(string str)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(str);
        var dataSpan = DecodeAsSpan(Encoding.UTF8.GetBytes(str).AsSpan());
        return dataSpan.ToArray();
    }


    public static string Decode(byte[] encodedBytes)
    {
        ArgumentNullException.ThrowIfNull(encodedBytes);
        var dataSpan = DecodeAsSpan(encodedBytes.AsSpan());
        return Encoding.UTF8.GetString(dataSpan);
    }

    public static string Decode(string arg)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(arg);
        var dataSpan = DecodeAsSpan(Encoding.UTF8.GetBytes(arg).AsSpan());
        return Encoding.UTF8.GetString(dataSpan);
    }

    public static Span<byte> DecodeAsSpan(ReadOnlySpan<byte> arg)
    {
        var destination = new Span<byte>(new byte[Base64Url.GetMaxDecodedLength(arg.Length)]);
        Base64Url.DecodeFromUtf8(arg, destination);
        return destination;
    }
}