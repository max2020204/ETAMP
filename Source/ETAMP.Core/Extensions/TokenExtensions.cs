using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

public static class TokenExtensions
{
    private static async Task<string> SerializeToJsonAsync<T>(T data, CancellationToken cancellationToken = default)
    {
        using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, data, cancellationToken: cancellationToken);
        ms.Position = 0;

        using var sr = new StreamReader(ms, Encoding.UTF8);
        return await sr.ReadToEndAsync(cancellationToken);
    }

    private static async Task<T?> DeserializeFromJsonAsync<T>(string json,
        CancellationToken cancellationToken = default)
        where T : class
    {
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        using var stream = new MemoryStream(jsonBytes);

        return await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
    }

    public static async Task SetData<T>(this Token token, T dataObject, CancellationToken cancellationToken = default)
        where T : class
    {
        token.Data = await SerializeToJsonAsync(dataObject, cancellationToken);
    }

    public static async Task<T?> GetData<T>(this Token token, CancellationToken cancellationToken = default)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(token.Data);
        return await DeserializeFromJsonAsync<T>(token.Data, cancellationToken);
    }

    public static async Task<string> ToJsonAsync(this Token token, CancellationToken cancellationToken = default)
    {
        return await SerializeToJsonAsync(token, cancellationToken);
    }
}