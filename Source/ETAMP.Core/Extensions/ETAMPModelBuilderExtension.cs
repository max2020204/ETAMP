using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

public static class ETAMPModelBuilderExtension
{
    public static async Task<string> ToJsonAsync(this ETAMPModelBuilder builder)
    {
        using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, builder);
        ms.Position = 0;

        using var sr = new StreamReader(ms, Encoding.UTF8);
        return await sr.ReadToEndAsync();
    }
}