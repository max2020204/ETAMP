using System.Buffers;
using System.Globalization;
using System.Text;

namespace ETAMP.Core.Utils;

/// <summary>
///     Provides methods to encode and decode strings using the Base64 URL encoding scheme.
/// </summary>
public static class Base64UrlEncoder
{
    private static readonly char base64PadCharacter = '=';

    private static string doubleBase64PadCharacter =
        string.Format(CultureInfo.InvariantCulture, "{0}{0}", base64PadCharacter);

    private static readonly char base64Character62 = '+';
    private static readonly char base64Character63 = '/';
    private static readonly char base64UrlCharacter62 = '-';
    private static readonly char base64UrlCharacter63 = '_';

    public static string Encode(string arg)
    {
        ArgumentNullException.ThrowIfNull(arg);

        return Encode(Encoding.UTF8.GetBytes(arg));
    }

    public static string Encode(ReadOnlySequence<byte> sequence)
    {
        if (sequence.IsEmpty)
            return string.Empty;

        var buffer = ArrayPool<byte>.Shared.Rent((int)sequence.Length);

        try
        {
            sequence.CopyTo(buffer);
            return Encode(buffer);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }


    public static string Encode(byte[] arg)
    {
        ArgumentNullException.ThrowIfNull(arg);

        var base64 = Convert.ToBase64String(arg);
        var padIndex = base64.IndexOf(base64PadCharacter);

        Span<char> result = stackalloc char[padIndex < 0 ? base64.Length : padIndex];
        base64.AsSpan(0, result.Length).CopyTo(result);

        for (var i = 0; i < result.Length; i++)
            if (result[i] == base64Character62)
                result[i] = base64UrlCharacter62;
            else if (result[i] == base64Character63)
                result[i] = base64UrlCharacter63;

        return new string(result);
    }


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


    public static string Decode(string arg)
    {
        ArgumentNullException.ThrowIfNull(arg);
        return Encoding.UTF8.GetString(DecodeBytes(arg));
    }
}