using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

public static class ETAMPModelBuilderExtensions
{
    /// <summary>
    ///     Converts the current <see cref="ETAMPModelBuilder" /> instance to a JSON string representation asynchronously.
    /// </summary>
    /// <param name="builder">The <see cref="ETAMPModelBuilder" /> instance to be converted to JSON format.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the JSON string representation of the
    ///     builder.
    /// </returns>
    public static async Task<string> ToJsonAsync(this ETAMPModelBuilder builder)
    {
        using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, builder);
        ms.Position = 0;

        using var sr = new StreamReader(ms, Encoding.UTF8);
        return await sr.ReadToEndAsync();
    }
}