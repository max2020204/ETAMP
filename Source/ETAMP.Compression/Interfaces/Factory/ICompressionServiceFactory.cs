namespace ETAMP.Compression.Interfaces.Factory;

/// <summary>
///     Defines a factory interface for creating instances of compression services based on a specified type.
/// </summary>
public interface ICompressionServiceFactory
{
    ICompressionService? Get(string compressionType);
}