using Newtonsoft.Json;
using System;

namespace Benchmarker.MVVM.Model
{
    public class Benchmark
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

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
