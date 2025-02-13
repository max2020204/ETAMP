using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

public static class TokenExtensions
{
    private static async Task<string> SerializeToJsonAsync<T>(T data)
    {
        Pipe pipe = new();
        await pipe.Writer.WriteAsync(JsonSerializer.SerializeToUtf8Bytes(data));
        await pipe.Writer.CompleteAsync();
        var result = await pipe.Reader.ReadAsync();
        var json = Encoding.UTF8.GetString(result.Buffer);
        pipe.Reader.AdvanceTo(result.Buffer.End);
        return json;
    }

    private static async Task<T> DeserializeFromJsonAsync<T>(string json) where T : class
    {
        var pipe = new Pipe();
        await pipe.Writer.WriteAsync(Encoding.UTF8.GetBytes(json));
        await pipe.Writer.CompleteAsync();

        var result = await pipe.Reader.ReadAsync();
        var obj = JsonSerializer.Deserialize<T>(result.Buffer.ToArray());

        pipe.Reader.AdvanceTo(result.Buffer.End);
        await pipe.Reader.CompleteAsync();

        return obj!;
    }

    public static async Task SetData<T>(this Token token, T dataObject) where T : class
    {
        token.Data = await SerializeToJsonAsync(dataObject);
    }

    public static async Task<T> GetData<T>(this Token token) where T : class
    {
        ArgumentNullException.ThrowIfNull(token.Data);
        return await DeserializeFromJsonAsync<T>(token.Data);
    }

    public static async Task<string> ToJsonAsync(this Token token)
    {
        return await SerializeToJsonAsync(token);
    }
}