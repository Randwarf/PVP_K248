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
        public List<HistoryBenchmark> Benchmarks { get; private set; }

        public HistoryViewModel()
        {
            UpdateTable();
            HistoryService.OnBenchmarksChanged += UpdateTable;
        }

        private void UpdateTable()
        {
            Debug.WriteLine("Called");
            Benchmarks = HistoryService.GetBenchmarks();
            OnPropertyChanged(nameof(Benchmarks));
        }
    }
}
