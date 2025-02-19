using System.Globalization;
using System.Text;

namespace ETAMP.Core.Utils;

/// <summary>
/// Provides functionality for encoding and decoding data using the Base64 URL encoding scheme.
/// </summary>
public static class Base64UrlEncoder
{
    /// <summary>
    ///     Represents the padding character ('=') used in Base64 encoding to ensure
    ///     that the encoded output length is a multiple of 4 bytes.
    /// </summary>
    private static readonly char base64PadCharacter = '=';

    /// <summary>
    ///     Represents a string value created by doubling the Base64 padding character ('=').
    ///     This is used internally in the Base64UrlEncoder class to format or handle scenarios
    ///     requiring two consecutive padding characters in Base64 encoding operations.
    /// </summary>
    private static string doubleBase64PadCharacter =
        string.Format(CultureInfo.InvariantCulture, "{0}{0}", base64PadCharacter);

    /// <summary>
    ///     Represents the 62nd character used in the standard Base64 encoding scheme.
    ///     In Base64 encoding, this character is '+'. It is replaced with a
    ///     different character in Base64 URL encoding schemes for URL safety.
    /// </summary>
    private static readonly char base64Character62 = '+';

    /// <summary>
    ///     Represents the character used in the Base64 encoding scheme at the 63rd position.
    ///     In the standard Base64 encoding table, this character is typically '/'.
    /// </summary>
    private static readonly char base64Character63 = '/';

    /// <summary>
    ///     Represents the character used as the 62nd value in the Base64 URL encoding scheme.
    ///     This character replaces the '+' symbol used in standard Base64 encoding
    ///     to ensure URLs and filenames remain safe and do not require escaping.
    /// </summary>
    private static readonly char base64UrlCharacter62 = '-';

    /// <summary>
    ///     Represents the character used to replace the 63rd character ('/') in Base64 encoding
    ///     when converting to Base64 URL encoding. This character improves URL safety by avoiding
    ///     invalid or reserved characters in URLs and file names.
    /// </summary>
    private static readonly char base64UrlCharacter63 = '_';

    /// <summary>
    ///     Encodes the specified string into a Base64 URL-encoded string.
    /// </summary>
    /// <param name="arg">The string to be encoded. It cannot be null.</param>
    /// <returns>A Base64 URL-encoded representation of the input string.</returns>
    public static string Encode(string arg)
    {
        ArgumentNullException.ThrowIfNull(arg);
        return Encode(Encoding.UTF8.GetBytes(arg));
    }

    /// <summary>
    ///     Encodes the input string into a Base64 URL-safe format.
    /// </summary>
    /// <param name="arg">The string to be encoded. Cannot be null.</param>
    /// <returns>A Base64 URL-encoded representation of the input string.</returns>
    public static string Encode(byte[] arg)
    {
        ArgumentNullException.ThrowIfNull(arg);
        return Encode(arg.AsSpan());
    }

    /// <summary>
    ///     Encodes the input data using Base64 URL encoding format.
    ///     This method eliminates padding characters and replaces characters
    ///     that are not URL safe with URL-safe alternatives.
    /// </summary>
    /// <param name="arg">The input data to encode as a read-only span of bytes.</param>
    /// <returns>A string representing the Base64 URL encoded format of the input data.</returns>
    public static string Encode(ReadOnlySpan<byte> arg)
    {
        if (arg.IsEmpty)
            return string.Empty;

        var base64 = Convert.ToBase64String(arg);
        var padIndex = base64.IndexOf(base64PadCharacter);

        Span<char> result = stackalloc char[padIndex < 0 ? base64.Length : padIndex];
        base64.AsSpan(0, result.Length).CopyTo(result);

        for (var i = 0; i < result.Length; i++)
        {
            if (result[i] == base64Character62)
                result[i] = base64UrlCharacter62;
            else if (result[i] == base64Character63)
                result[i] = base64UrlCharacter63;
        }

        return new string(result);
    }


    /// Decodes a Base64Url-encoded string into a byte array.
    /// This method processes a string encoded using Base64Url encoding and converts it into its original byte representation.
    /// It handles the replacement of characters specific to Base64Url encoding and correctly pads the string
    /// to ensure proper decoding.
    /// <param name="str">The Base64Url-encoded string to be decoded. Must not be null.</param>
    /// <returns>A byte array obtained by decoding the input string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input string is null.</exception>
    /// <exception cref="FormatException">Thrown if the input string is not a valid Base64Url-encoded string.</exception>
    public static byte[] DecodeBytes(string str)
    {
        ArgumentNullException.ThrowIfNull(str);

        var fixedStr = string.Create(str.Length, str, (span, s) =>
        {
            s.AsSpan().CopyTo(span);
            for (var i = 0; i < span.Length; i++)
                if (span[i] == base64UrlCharacter62)
                    span[i] = base64Character62;
                else if (span[i] == base64UrlCharacter63)
                    span[i] = base64Character63;
        });

        var mod = fixedStr.Length % 4;
        if (mod > 0) fixedStr = fixedStr.PadRight(fixedStr.Length + (4 - mod), base64PadCharacter);

        return Convert.FromBase64String(fixedStr);
    }


    /// <summary>
    ///     Decodes a Base64 URL-encoded string into its original UTF-8 string representation.
    /// </summary>
    /// <param name="arg">The Base64 URL-encoded string to decode. Must not be null.</param>
    /// <returns>The decoded UTF-8 string from the input Base64 URL-encoded string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the supplied <paramref name="arg" /> is null.</exception>
    public static string Decode(string arg)
    {
        ArgumentNullException.ThrowIfNull(arg);
        return Encoding.UTF8.GetString(DecodeBytes(arg));
    }
}