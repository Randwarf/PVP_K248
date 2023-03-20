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
        private List<HistoryBenchmark> _Benchmarks { get; set; }
        public List<HistoryBenchmark> Benchmarks
        {
            get
            {
                return _Benchmarks ?? (_Benchmarks = new List<HistoryBenchmark>());
            }
            set
            {
                _Benchmarks = value;
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
            Debug.WriteLine("Called");
            Benchmarks = null;
            Benchmarks = HistoryService.GetBenchmarks();
            OnPropertyChanged(nameof(Benchmarks));
        }
    }
}
