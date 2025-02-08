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

        var reader = new Utf8JsonReader(pipe.Writer.GetSpan());
        JsonSerializer.Deserialize<T>(ref reader);

        await using (var writer = new Utf8JsonWriter(pipe.Writer))
        {
            JsonSerializer.Serialize(writer, model);
            await writer.FlushAsync();
        }

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