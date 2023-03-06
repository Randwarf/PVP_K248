using Newtonsoft.Json;

namespace Benchmarker.MVVM.Model
{
    internal class Benchmark
    {
        [JsonProperty("cpu")]
        public double CPU { get; set; }
        [JsonProperty("ram")]
        public double RAM { get; set; }
        [JsonProperty("energy")]
        public double Energy { get; set; }
        [JsonProperty("process")]
        public string Process { get; set; }
        [JsonProperty("disk")]
        public double Disk { get; set; }
    }
}
