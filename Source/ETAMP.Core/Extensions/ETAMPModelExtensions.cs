using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

public static class ETAMPModelExtensions
{
    public static async Task<string> ToJsonAsync<T>(this ETAMPModel<T> model) where T : Token
    {
        using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, model);
        ms.Position = 0;

        using var sr = new StreamReader(ms, Encoding.UTF8);
        return await sr.ReadToEndAsync();
    }

    public static ETAMPModel<T> FromJsonAsync<T>(this string json) where T : Token
    {
        return JsonSerializer.Deserialize<ETAMPModel<T>>(json);
    }
}