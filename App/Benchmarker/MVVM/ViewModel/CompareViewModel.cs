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
            //HistoryBenchmark benchmark1 = null;
            //HistoryBenchmark benchmark2 = null;

            //HistoryBenchmarkSelection historyWindow1 = new HistoryBenchmarkSelection();
            //bool? success1 = historyWindow1.ShowDialog();
            //if (success1 == true)
            //{
            //    benchmark1 = historyWindow1.ChosenBenchmark;
            //    historyWindow1.Close();
            //}

            //HistoryBenchmarkSelection historyWindow2 = new HistoryBenchmarkSelection();
            //bool? success2 = historyWindow2.ShowDialog();
            //if (success2 == true)
            //{
            //    benchmark2 = historyWindow2.ChosenBenchmark;
            //    historyWindow2.Close();
            //}

            //if (success1 == true && success2 == true)
            //{
            //    benchmarkComparisonRows = BenchmarkCompareService.CompareBenchmarks(benchmark1, benchmark2);
            //}
        }
    }

    public class ComparisonRow
    {
        public string Attribute { get; set; }
        public string Process1 { get; set; }
        public string Process2 { get; set; }
    }
}
