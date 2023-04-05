using Newtonsoft.Json;

namespace Benchmarker.MVVM.Model
{
    internal class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
        
        [JsonProperty("isPremium")]
        public bool IsPremium { get; set; }
    }
}
