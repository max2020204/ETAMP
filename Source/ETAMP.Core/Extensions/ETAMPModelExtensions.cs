using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

public static class ETAMPModelExtensions
{
    /// <summary>
    ///     Converts the specified ETAMP model to its JSON string representation asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of token associated with the ETAMP model, constrained to inherit from Token.</typeparam>
    /// <param name="model">The ETAMP model instance to be serialized to JSON.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the JSON string representation of the ETAMP model.
    /// </returns>
    public static async Task<string> ToJsonAsync<T>(this ETAMPModel<T> model) where T : Token
    {
        using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, model);
        ms.Position = 0;

        using var sr = new StreamReader(ms, Encoding.UTF8);
        return await sr.ReadToEndAsync();
    }
}