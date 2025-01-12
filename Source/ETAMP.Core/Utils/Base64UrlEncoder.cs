#region

using System.Text;

#endregion

namespace ETAMP.Core.Utils;

/// <summary>
///     Provides methods to encode and decode strings using the Base64 URL encoding scheme.
/// </summary>
public static class Base64UrlEncoder
{
    private const char Base64PadCharacter = '=';
    private const char Base64Character62 = '+';
    private const char Base64Character63 = '/';
    private const char Base64UrlCharacter62 = '-';
    private const char Base64UrlCharacter63 = '_';

    /// <summary>
    ///     Encodes a string using Base64Url encoding.
    /// </summary>
    /// <param name="arg">The string to be encoded.</param>
    /// <returns>The encoded string.</returns>
    public static string? Encode(string arg)
    {
        if (string.IsNullOrWhiteSpace(arg))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(arg));
        return Encode(Encoding.UTF8.GetBytes(arg));
    }

    /// <summary>
    ///     Encodes a byte array using Base64Url encoding.
    /// </summary>
    /// <param name="inArray">The byte array to be encoded.</param>
    /// <returns>The encoded string.</returns>
    public static string Encode(byte[] inArray)
    {
        if (inArray == null || inArray.Length == 0) throw new ArgumentNullException(nameof(inArray));
        return Encode(inArray, 0, inArray.Length);
    }

    private static string Encode(byte[] inArray, int offset, int length)
    {
        if (inArray == null) throw new ArgumentNullException(nameof(inArray));
        if (offset < 0 || length < 0 || offset + length > inArray.Length)
            throw new ArgumentOutOfRangeException("Invalid offset or length.");

        var destination = new char[(length + 2) / 3 * 4];
        var actualLength = Encode(inArray.AsSpan().Slice(offset, length), destination.AsSpan());
        var result = new string(destination, 0, actualLength);

        // Remove any trailing '=' characters for Base64Url
        return result.TrimEnd(Base64PadCharacter);
    }

    private static int Encode(ReadOnlySpan<byte> inArray, Span<char> output)
    {
        var table = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_".AsSpan();
        int i = 0, j = 0, limit = inArray.Length - inArray.Length % 3;

        for (; i < limit; i += 3)
        {
            var d0 = inArray[i];
            var d1 = inArray[i + 1];
            var d2 = inArray[i + 2];
            output[j++] = table[d0 >> 2];
            output[j++] = table[((d0 & 0x03) << 4) | (d1 >> 4)];
            output[j++] = table[((d1 & 0x0f) << 2) | (d2 >> 6)];
            output[j++] = table[d2 & 0x3f];
        }

        if (i < inArray.Length)
        {
            var d0 = inArray[i];
            output[j++] = table[d0 >> 2];
            if (++i < inArray.Length)
            {
                var d1 = inArray[i];
                output[j++] = table[((d0 & 0x03) << 4) | (d1 >> 4)];
                output[j++] = table[(d1 & 0x0f) << 2];
            }
            else
            {
                output[j++] = table[(d0 & 0x03) << 4];
            }
        }

        return j;
    }

    /// <summary>
    ///     Decodes a Base64Url-encoded string to bytes.
    /// </summary>
    /// <param name="str">The Base64Url-encoded string to decode.</param>
    /// <returns>The decoded bytes.</returns>
    public static byte[] DecodeBytes(string? str)
    {
        if (string.IsNullOrEmpty(str)) throw new ArgumentNullException(nameof(str));
        return Decode(str.AsSpan());
    }

    /// <summary>
    ///     Decodes a Base64Url-encoded string into a byte array.
    /// </summary>
    /// <param name="strSpan">The Base64Url-encoded string to decode.</param>
    /// <returns>A byte array representing the decoded string.</returns>
    private static byte[] Decode(ReadOnlySpan<char> strSpan)
    {
        var input = new string(strSpan);

        input = input.Replace(Base64UrlCharacter62, Base64Character62)
            .Replace(Base64UrlCharacter63, Base64Character63);

        var paddingNeeded = (4 - input.Length % 4) % 4;
        input = input.PadRight(input.Length + paddingNeeded, Base64PadCharacter);

        return Convert.FromBase64String(input);
    }

    /// <summary>
    ///     Decodes a Base64Url-encoded string into its original form.
    /// </summary>
    /// <param name="arg">The Base64Url-encoded string to decode.</param>
    /// <returns>The decoded string.</returns>
    public static string Decode(string? arg)
    {
        return Encoding.UTF8.GetString(DecodeBytes(arg));
    }
}