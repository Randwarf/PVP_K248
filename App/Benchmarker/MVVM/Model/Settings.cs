using Newtonsoft.Json;

namespace Benchmarker.MVVM.Model
{
    internal class Settings
    {
        [JsonProperty("agreedToDataSharing")]
        public bool agreedToDataSharing = false;
    }
}
