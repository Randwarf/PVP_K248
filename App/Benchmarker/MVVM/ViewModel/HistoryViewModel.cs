using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.DTOs;

namespace Benchmarker.MVVM.ViewModel
{
    internal class HistoryViewModel : ObservableObject
    {
        private List<HistoryBenchmark> benchmarks;
        private BenchmarkViewModel benchmarkViewModel;

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

        public HistoryViewModel(BenchmarkViewModel benchmarkViewModel)
        {
            this.benchmarkViewModel = benchmarkViewModel;
            UpdateTable();
            HistoryService.OnBenchmarksChanged += UpdateTable;
        }

        private void UpdateTable()
        {
            Benchmarks = null;
            var historyBenchmarks = HistoryService.GetBenchmarks();
            var coloredBenchmarks = SetRowColorBasedOnEnergy(historyBenchmarks);
            coloredBenchmarks.Sort((x, y) => y.Energy.CompareTo(x.Energy));
            Benchmarks = coloredBenchmarks;
            benchmarkViewModel.StartVM.ResetScreen();
        }

		private List<HistoryBenchmark> SetRowColorBasedOnEnergy(List<HistoryBenchmark> benchmarks)
		{
            double maxEnergy = benchmarks.Select(x => x.Energy).Max();

			foreach (HistoryBenchmark benchmark in benchmarks)
			{
				double normalizedEnergy = benchmark.Energy / maxEnergy;
				byte red = (byte)(255 * (1 - normalizedEnergy));
				byte green = (byte)(255 * normalizedEnergy);
				byte blue = 0;

				Color color = Color.FromRgb(red, green, blue);
				SolidColorBrush rowColor = new SolidColorBrush(color);
				benchmark.RowColor = rowColor;
			}

            return benchmarks;
		}
	}
}
