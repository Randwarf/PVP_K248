using Newtonsoft.Json;

namespace Benchmarker.MVVM.Model
{
    internal class Settings
    {
        [JsonProperty("agreedToDataSharing")]
        public bool agreedToDataSharing = false;

        [JsonProperty("currentTheme")]
        public string currentTheme = "Theme/LightTheme.xaml";
    }
}
