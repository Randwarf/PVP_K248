using Newtonsoft.Json;

namespace Benchmarker.MVVM.Model
{
    internal class Benchmark
    {
        [JsonProperty("cpu")]
        public string CPU { get; set; }
        [JsonProperty("ram")]
        public string RAM { get; set; }
        [JsonProperty("energy")]
        public float Energy { get; set; }
    }
}
