using System.Collections.Generic;

namespace Benchmarker.MVVM.Model.DTOs
{
    public class BenchmarkCompareInfo
    {
        public List<string> OverallRating { get; set; }
        public List<string> CPUComparison { get; set; }
        public List<string> RAMComparison { get; set; }
        public List<string> DiskComparison { get; set; }
        public List<string> EnergyComparison { get; set; }
    }
}
