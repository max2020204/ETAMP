using System.Reflection;
using System.Text;

namespace ETAMP.Core.Models;

/// <summary>
///     Represents a token object.
/// </summary>
public record Token
{
    /// <summary>
    ///     Represents the unique identifier of a token.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    ///     Represents the unique identifier of a message.
    /// </summary>
    public Guid MessageId { get; set; }

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

    public override string ToString()
    {
        var type = typeof(Token);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        StringBuilder sb = new();
        foreach (var property in properties)
            sb.Append(property.Name).Append(": ").Append(property.GetValue(this)).Append(", ");

        return sb.ToString();
    }
}