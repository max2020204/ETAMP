using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

public static class ETAMPModelBuilderExtension
{
    public static async Task<string> ToJsonAsync(this ETAMPModelBuilder builder)
    {
        var pipe = new Pipe();
        await pipe.Writer.WriteAsync(JsonSerializer.SerializeToUtf8Bytes(builder));
        await pipe.Writer.CompleteAsync();
        var result = await pipe.Reader.ReadAsync();
        var json = Encoding.UTF8.GetString(result.Buffer);
        pipe.Reader.AdvanceTo(result.Buffer.End);
        return json;
    }
}