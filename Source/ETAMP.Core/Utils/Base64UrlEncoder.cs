using System.Buffers.Text;
using System.Text;

namespace ETAMP.Core.Utils;

/// <summary>
///     Provides utility methods for Base64 URL encoding and decoding.
/// </summary>
/// <remarks>
///     This class allows encoding and decoding of data using the Base64 URL format, which is a URL-safe version of Base64
///     encoding.
///     It is useful for encoding binary data or strings into a URL-safe format and decoding it back without introducing
///     invalid URL characters.
/// </remarks>
public static class Base64UrlEncoder
{
    /// Encodes the specified string into a Base64Url encoded string.
    /// <param name="arg">The input string to be encoded. It must not be null.</param>
    /// <returns>A Base64Url encoded representation of the input string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input string <paramref name="arg" /> is null.</exception>
    public static string Encode(string arg)
    {
        ArgumentNullException.ThrowIfNull(arg);
        var data = Encode(Encoding.UTF8.GetBytes(arg).AsSpan());
        return Encoding.UTF8.GetString(data);
    }

    /// Encodes a byte array to a Base64Url-encoded string representation.
    /// <param name="arg">
    ///     The byte array to be encoded. Cannot be null.
    /// </param>
    /// <returns>
    ///     A string containing the Base64Url-encoded representation of the input byte array.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the input byte array is null.
    /// </exception>
    public static string Encode(byte[] arg)
    {
        ArgumentNullException.ThrowIfNull(arg);
        var data = Encode(arg.AsSpan());
        return Encoding.UTF8.GetString(data);
    }

    /// Encodes a given input using Base64 URL encoding.
    /// <param name="arg">The string input to encode. It must not be null.</param>
    /// <returns>A Base64 URL-encoded string representation of the input.</returns>
    public static Span<byte> Encode(ReadOnlySpan<byte> arg)
    {
        var destination = new Span<byte>(new byte[Base64Url.GetEncodedLength(arg.Length)]);
        Base64Url.EncodeToUtf8(arg, destination);
        return destination;
    }


    /// Decodes a Base64 URL-encoded string into a byte array.
    /// <param name="str">
    ///     The Base64 URL-encoded input string to decode.
    /// </param>
    /// <returns>
    ///     A byte array containing the decoded data from the input string.
    /// </returns>
    /// <exception cref="System.ArgumentException">
    ///     Thrown when the input string is null, empty, or contains only whitespace.
    /// </exception>
    public static byte[] DecodeBytes(string str)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(str);
        var dataSpan = DecodeAsSpan(Encoding.UTF8.GetBytes(str).AsSpan());
        return dataSpan.ToArray();
    }


    /// Decodes a Base64Url-encoded byte array into a UTF-8 encoded string.
    /// <param name="encodedBytes">The byte array that contains the Base64Url-encoded data. Cannot be null.</param>
    /// <returns>A UTF-8 encoded string representation of the decoded data.</returns>
    public static string Decode(byte[] encodedBytes)
    {
        ArgumentNullException.ThrowIfNull(encodedBytes);
        var dataSpan = DecodeAsSpan(encodedBytes.AsSpan());
        return Encoding.UTF8.GetString(dataSpan);
    }

    /// <summary>
    ///     Decodes a Base64 URL-encoded string into its original representation.
    /// </summary>
    /// <param name="arg">The Base64 URL-encoded string to decode.</param>
    /// <returns>The original string decoded from the Base64 URL-encoded representation.</returns>
    public static string Decode(string arg)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(arg);
        var dataSpan = DecodeAsSpan(Encoding.UTF8.GetBytes(arg).AsSpan());
        return Encoding.UTF8.GetString(dataSpan);
    }

    /// Decodes a Base64Url-encoded byte span into its original form and returns the result as a byte span.
    /// <param name="arg">The encoded input data as a read-only byte span.</param>
    /// <returns>The decoded data as a span of bytes.</returns>
    public static Span<byte> DecodeAsSpan(ReadOnlySpan<byte> arg)
    {
        var destination = new Span<byte>(new byte[Base64Url.GetMaxDecodedLength(arg.Length)]);
        Base64Url.DecodeFromUtf8(arg, destination);
        return destination;
    }
}