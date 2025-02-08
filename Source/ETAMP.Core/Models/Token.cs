using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ETAMP.Core.Models;

/// <summary>
///     Represents a token object.
/// </summary>
public class Token
{
    private readonly ILogger<Token> _logger;

    public Token(ILogger<Token>? logger = null)
    {
        _logger = logger ?? NullLogger<Token>.Instance;
    }

    /// <summary>
    ///     Represents the unique identifier of a token.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    ///     Represents the unique identifier of a message.
    /// </summary>
    public Guid MessageId { get; protected internal set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the data for this token is encrypted.
    /// </summary>
    /// <value><c>true</c> if the data is encrypted; otherwise, <c>false</c>.</value>
    public bool IsEncrypted { get; set; }

    /// <summary>
    ///     Represents the data property of a Token object as a JSON string.
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    ///     Represents a timestamp associated with a token.
    /// </summary>
    public DateTimeOffset TimeStamp { get; } = DateTimeOffset.Now.ToUniversalTime();

    /// <summary>
    ///     Set data after converting an object to a JSON string.
    /// </summary>
    public async Task SetData<T>(T dataObject) where T : class
    {
        _logger.LogDebug("Serializing token data");
        await using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, dataObject);
        _logger.LogDebug("Serializing token data done");
        Data = Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    /// <summary>
    ///     Get data by converting the JSON string back to an object of type T.
    /// </summary>
    public async Task<T?> GetData<T>() where T : class
    {
        if (string.IsNullOrEmpty(Data))
            return null;
        try
        {
            _logger.LogDebug("Deserializing token data");
            var byteArray = Encoding.UTF8.GetBytes(Data);
            await using var memoryStream = new MemoryStream(byteArray);
            _logger.LogDebug("Deserializing token data done");
            return await JsonSerializer.DeserializeAsync<T>(memoryStream);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing token data");
            return null;
        }
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public async Task<Stream> ToJsonStreamAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Serializing token");
        var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, this, cancellationToken: cancellationToken);
        memoryStream.Position = 0;
        _logger.LogDebug("Serializing token done");
        return memoryStream;
    }

    public async Task<string> ToJsonAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Converting token to JSON string");
        using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, this, cancellationToken: cancellationToken);
        memoryStream.Position = 0;
        using var reader = new StreamReader(memoryStream, Encoding.UTF8);
        _logger.LogDebug("Token successfully serialized to JSON string");
        return await reader.ReadToEndAsync(cancellationToken);
    }
}