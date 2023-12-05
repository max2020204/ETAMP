using Newtonsoft.Json;

namespace ETAMP.Models
{
    public class BasePaylaod
    {
        [JsonProperty("jti")]
        public Guid JTI { get; set; }
        [JsonProperty("iat")]
        public DateTime IssuedAt { get; set; }
        [JsonProperty("iss")]
        public string? Issuer { get; set; }
        [JsonProperty("aud")]
        public string? Audience { get; set; }
        [JsonProperty("exp")]
        public DateTime Expires { get; set; }
    }
}