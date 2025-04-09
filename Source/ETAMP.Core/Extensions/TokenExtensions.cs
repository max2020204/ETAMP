using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

public static class TokenExtensions
{
    /// <summary>
    ///     Asynchronously serializes the given object to a JSON-encoded string.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="data">The object to be serialized.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the JSON string representation of
    ///     the serialized object.
    /// </returns>
    private static async Task<string> SerializeToJsonAsync<T>(T data, CancellationToken cancellationToken = default)
    {
        using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, data, cancellationToken: cancellationToken);
        ms.Position = 0;

        using var sr = new StreamReader(ms, Encoding.UTF8);
        return await sr.ReadToEndAsync(cancellationToken);
    }

    /// <summary>
    ///     Deserializes a JSON string into an object of type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize. Must be a reference type.</typeparam>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The deserialized object of type <typeparamref name="T" />, or null if deserialization fails.</returns>
    private static async Task<T?> DeserializeFromJsonAsync<T>(string json,
        CancellationToken cancellationToken = default)
        where T : class
    {
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        using var stream = new MemoryStream(jsonBytes);

        return await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
    }

    /// <summary>
    ///     Sets the data for the token by serializing the provided object to a JSON string.
    /// </summary>
    /// <typeparam name="T">The type of the data object to be serialized and set in the token.</typeparam>
    /// <param name="token">The token where the serialized data will be stored.</param>
    /// <param name="dataObject">The object to be serialized and set in the token.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task SetData<T>(this Token token, T dataObject, CancellationToken cancellationToken = default)
        where T : class
    {
        token.Data = await SerializeToJsonAsync(dataObject, cancellationToken);
    }

    /// <summary>
    ///     Asynchronously retrieves and deserializes the data stored in the <see cref="Token.Data" /> property into an object
    ///     of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the stored data into.</typeparam>
    /// <param name="token">The token containing the serialized data.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>The deserialized object of type <typeparamref name="T" /> or <c>null</c> if deserialization fails.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <see cref="Token.Data" /> is <c>null</c>.</exception>
    public static async Task<T?> GetData<T>(this Token token, CancellationToken cancellationToken = default)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(token.Data);
        return await DeserializeFromJsonAsync<T>(token.Data, cancellationToken);
    }

    /// <summary>
    ///     Converts the current <see cref="Token" /> instance to its JSON representation asynchronously.
    /// </summary>
    /// <param name="token">The <see cref="Token" /> instance to serialize to JSON format.</param>
    /// <param name="cancellationToken">A CancellationToken used to observe cancellation requests.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the JSON string representation of
    ///     the <see cref="Token" />.
    /// </returns>
    public static async Task<string> ToJsonAsync(this Token token, CancellationToken cancellationToken = default)
    {
        return await SerializeToJsonAsync(token, cancellationToken);
    }
}