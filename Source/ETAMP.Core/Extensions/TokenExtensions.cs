using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

public static class TokenExtensions
{
    /// <summary>
    /// Сериализует объект в JSON через Pipe и возвращает строку JSON
    /// </summary>
    private static async Task<string> SerializeToJsonAsync<T>(T data)
    {
        var pipe = new Pipe();

        await using (var writer = new Utf8JsonWriter(pipe.Writer))
        {
            JsonSerializer.Serialize(writer, data);
            await writer.FlushAsync();
        }

        var result = await pipe.Reader.ReadAsync();
        var json = Encoding.UTF8.GetString(result.Buffer.ToArray());

        pipe.Reader.AdvanceTo(result.Buffer.End);
        await pipe.Reader.CompleteAsync();

        return json;
    }

    /// <summary>
    /// Десериализует JSON-строку в объект через Pipe
    /// </summary>
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

    /// <summary>
    /// Записывает объект в Token.Data в формате JSON
    /// </summary>
    public static async Task SetData<T>(this Token token, T dataObject) where T : class
    {
        token.Data = await SerializeToJsonAsync(dataObject);
    }

    /// <summary>
    /// Получает объект из Token.Data, десериализуя его
    /// </summary>
    public static async Task<T> GetData<T>(this Token token) where T : class
    {
        ArgumentNullException.ThrowIfNull(token.Data);
        return await DeserializeFromJsonAsync<T>(token.Data);
    }

    /// <summary>
    /// Сериализует Token в JSON-строку
    /// </summary>
    public static async Task<string> ToJsonAsync(this Token token)
    {
        return await SerializeToJsonAsync(token);
    }
}