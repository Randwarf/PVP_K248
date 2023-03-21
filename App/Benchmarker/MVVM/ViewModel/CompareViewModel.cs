using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.DTOs;
using System.Collections.Generic;

namespace Benchmarker.MVVM.ViewModel
{
    internal class CompareViewModel : ObservableObject
    {
        private List<ComparisonRow> benchmarkComparisonRows;

        public List<ComparisonRow> BenchmarkComparisonRows
        {
            get { return benchmarkComparisonRows; }
            set { benchmarkComparisonRows = value; OnPropertyChanged(); }
        }

        public CompareViewModel()
        {
            HistoryBenchmark benchmark1 = HistoryService.GetBenchmarks()[5];
            HistoryBenchmark benchmark2 = HistoryService.GetBenchmarks()[6];
            benchmarkComparisonRows = BenchmarkCompareService.CompareBenchmarks(benchmark1, benchmark2);
            
        }
    }

    public class ComparisonRow
    {
        public string Attribute { get; set; }
        public string Process1 { get; set; }
        public string Process2 { get; set; }
    }
}
