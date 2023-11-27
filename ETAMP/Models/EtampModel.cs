namespace ETAMP.Models
{
    public class EtampModel
    {
        public Guid Id { get; set; }
        public double Version { get; set; }
        public string? Token { get; set; }
        public string? UpdateType { get; set; }
        public string? SignatureToken { get; set; }
        public string? SignatureMessage { get; set; }
    }
}