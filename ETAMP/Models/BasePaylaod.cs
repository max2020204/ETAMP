namespace ETAMP.Models
{
    public class BasePaylaod
    {
        public Guid JTI { get; set; }
        public DateTime IssuedAt { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public DateTime Expires { get; set; }
    }
}