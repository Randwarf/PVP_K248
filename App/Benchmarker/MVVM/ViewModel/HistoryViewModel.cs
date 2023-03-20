using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.DTOs;

namespace Benchmarker.MVVM.ViewModel
{
    internal class HistoryViewModel : ObservableObject
    {
        private List<HistoryBenchmark> benchmarks;

        public List<HistoryBenchmark> Benchmarks
        {
            get
            {
                return benchmarks ?? (benchmarks = new List<HistoryBenchmark>());
            }
            set
            {
                benchmarks = value;
                OnPropertyChanged();
            }
        }

        public HistoryViewModel()
        {
            UpdateTable();
            HistoryService.OnBenchmarksChanged += UpdateTable;
        }

        private void UpdateTable()
        {
            Benchmarks = null;
            Benchmarks = HistoryService.GetBenchmarks();
        }
    }
}
