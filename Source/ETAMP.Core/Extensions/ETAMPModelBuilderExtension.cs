using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

public static class ETAMPModelBuilderExtension
{
    public static async Task<string> ToJsonAsync(this ETAMPModelBuilder builder)
    {
        await using var stream = new MemoryStream();
        await using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
        {
            Indented = false
        });

        WriteJson(builder, writer);
        await writer.FlushAsync();

        return Encoding.UTF8.GetString(stream.GetBuffer());
    }

    private static void WriteJson(this ETAMPModelBuilder builder, Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString(nameof(builder.Id), builder.Id.ToString());
        writer.WriteNumber(nameof(builder.Version), builder.Version);


        if (!string.IsNullOrEmpty(builder.Token)) writer.WriteString(nameof(builder.Token), builder.Token);

        if (!string.IsNullOrEmpty(builder.UpdateType))
            writer.WriteString(nameof(builder.UpdateType), builder.UpdateType);

        if (!string.IsNullOrEmpty(builder.CompressionType))
            writer.WriteString(nameof(builder.CompressionType), builder.CompressionType);

        if (!string.IsNullOrEmpty(builder.SignatureMessage))
            writer.WriteString(nameof(builder.SignatureMessage), builder.SignatureMessage);

        writer.WriteEndObject();
    }
}