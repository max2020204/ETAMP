#region

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

#endregion

namespace ETAMP.Core.Models;

/// <summary>
///     Represents the model for the ETAMP (Encrypted Token And Message Protocol) structure.
///     This model is designed to facilitate secure and efficient exchanges of messages and transactions within a network,
///     by leveraging the ETAMP framework which encapsulates data in a secure and structured manner.
/// </summary>
public struct ETAMPModel<T> where T : Token
{
    private readonly ILogger<ETAMPModel<T>> _logger;

    public ETAMPModel(ILogger<ETAMPModel<T>>? logger = null)
    {
        _logger = logger ?? NullLogger<ETAMPModel<T>>.Instance;
    }

    /// <summary>
    ///     Gets or sets the unique identifier for the ETAMP model instance.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Gets or sets the version of the ETAMP protocol. This information is used to ensure compatibility across different
    ///     versions of the protocol.
    /// </summary>
    public double Version { get; init; }

    /// <summary>
    ///     Represents a token used in the ETAMP model.
    /// </summary>
    /// <typeparam name="T">The type of the token.</typeparam>
    public T? Token { get; init; }

    /// <summary>
    ///     Gets or sets the type of update this model represents, providing context for the transaction or message exchange.
    /// </summary>
    public string? UpdateType { get; init; }

    /// <summary>
    ///     Gets or sets the compression type used for the ETAMP (Encrypted Token And Message Protocol) structure.
    ///     The compression type determines the algorithm used to compress the data within the ETAMP structure,
    ///     allowing for efficient and compact representation of the information being exchanged.
    /// </summary>
    public string? CompressionType { get; set; }


    /// <summary>
    ///     Gets or sets the signature for the message, adding a layer of security by safeguarding the integrity of
    ///     the message within the ETAMP structure.
    /// </summary>
    public string? SignatureMessage { get; set; }


    /// <summary>
    ///     Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    ///     <c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        _logger.LogDebug("Comparing ETAMP model");
        if (obj is ETAMPModel<T> other)
            return Id == other.Id
                   && Version.Equals(other.Version)
                   && UpdateType == other.UpdateType
                   && Token == other.Token
                   && CompressionType == other.CompressionType
                   && SignatureMessage == other.SignatureMessage;

        return false;
    }


    public string ToJson()
    {
        using var stream = new MemoryStream();
        _logger.LogDebug("Serializing ETAMP model");
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
        {
            Indented = false
        });

        WriteJson(writer);
        _logger.LogDebug("Serializing ETAMP model done");
        writer.Flush();


        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public async Task<string> ToJsonAsync()
    {
        await using var stream = new MemoryStream();
        _logger.LogDebug("Serializing ETAMP model");
        await using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
        {
            Indented = false
        });

        WriteJson(writer);
        _logger.LogDebug("Serializing ETAMP model done");
        await writer.FlushAsync();

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private void WriteJson(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        _logger.LogDebug("Serializing ETAMP model properties");
        writer.WriteString(nameof(Id), Id.ToString());
        writer.WriteNumber(nameof(Version), Version);


        if (Token != null)
        {
            _logger.LogDebug("Serializing ETAMP model token");
            writer.WritePropertyName(nameof(Token));
            writer.WriteRawValue(Token.ToJson(), true);
        }

        if (!string.IsNullOrEmpty(UpdateType))
        {
            _logger.LogDebug("Serializing ETAMP model update type");
            writer.WriteString(nameof(UpdateType), UpdateType);
        }

        if (!string.IsNullOrEmpty(CompressionType))
        {
            _logger.LogDebug("Serializing ETAMP model compression type");
            writer.WriteString(nameof(CompressionType), CompressionType);
        }

        if (!string.IsNullOrEmpty(SignatureMessage))
        {
            _logger.LogDebug("Serializing ETAMP model signature message");
            writer.WriteString(nameof(SignatureMessage), SignatureMessage);
        }

        _logger.LogDebug("Serializing ETAMP model properties done");
        writer.WriteEndObject();
    }

    /// <summary>
    ///     Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        _logger.LogDebug("Calculating hash code");
        return HashCode.Combine(Id, Version, Token, UpdateType, CompressionType, SignatureMessage);
    }

    /// <summary>
    ///     Returns a string that represents the current ETAMPModel object.
    /// </summary>
    /// <returns>
    ///     A string that represents the current object.
    /// </returns>
    public override string ToString()
    {
        _logger.LogDebug("Converting ETAMP model to string");
        return ToJson();
    }

    public Task<string> ToStringAsync()
    {
        _logger.LogDebug("Converting ETAMP model to string");
        return ToJsonAsync();
    }
}