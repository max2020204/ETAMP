using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

public static class ETAMPModelExtensions
{
    public static async Task<string> ToJsonAsync<T>(this ETAMPModel<T> model) where T : Token
    {
        var pipe = new Pipe();
        await pipe.Writer.WriteAsync(JsonSerializer.SerializeToUtf8Bytes(model));
        await pipe.Writer.CompleteAsync();
        var result = await pipe.Reader.ReadAsync();
        var json = Encoding.UTF8.GetString(result.Buffer);
        pipe.Reader.AdvanceTo(result.Buffer.End);
        return json;
    }

    public static ETAMPModel<T> FromJsonAsync<T>(this string json) where T : Token
    {
        return JsonSerializer.Deserialize<ETAMPModel<T>>(json);
    }
}