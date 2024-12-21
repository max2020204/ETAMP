namespace ETAMP.Core.Models;

public class ETAMPModelBuilder
{
    public Guid Id { get; set; }
    public double Version { get; set; }
    public string? Token { get; set; }
    public string? UpdateType { get; set; }
    public string? CompressionType { get; set; }
    public string? SignatureMessage { get; set; }
}