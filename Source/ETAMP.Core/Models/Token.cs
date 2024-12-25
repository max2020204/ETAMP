using System.Diagnostics;
using System.Text.Json;

namespace ETAMP.Core.Models;

/// <summary>
///     Represents a token object.
/// </summary>
public class Token
{
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
    public void SetData<T>(T dataObject) where T : class
    {
        Data = JsonSerializer.Serialize(dataObject);
    }

    /// <summary>
    ///     Get data by converting the JSON string back to an object of type T.
    /// </summary>
    public T? GetData<T>() where T : class
    {
        if (string.IsNullOrEmpty(Data))
            return null;
        try
        {
            return JsonSerializer.Deserialize<T>(Data);
        }
        catch (JsonException ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}