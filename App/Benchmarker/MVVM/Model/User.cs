using Newtonsoft.Json;

namespace Benchmarker.MVVM.Model
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
        
        public bool IsPremium {
            get
            {
                return premiumEndDate != "1900-01-01";
            }
        }

        [JsonProperty("premiumEndDate")]
        public string premiumEndDate { get; set; }
    }
}
