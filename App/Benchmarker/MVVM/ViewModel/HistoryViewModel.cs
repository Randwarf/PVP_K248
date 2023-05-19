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
            coloredBenchmarks.Sort((x, y) => x.Energy.CompareTo(y.Energy));
            Benchmarks = coloredBenchmarks;
            benchmarkViewModel.StartVM.ResetScreen();
        }

		public List<HistoryBenchmark> SetRowColorBasedOnEnergy(List<HistoryBenchmark> benchmarks)
		{
            var energyValues = benchmarks.Select(x => x.Energy);
            double minEnergy = energyValues.Min();
            double maxEnergy = energyValues.Max();

			foreach (HistoryBenchmark benchmark in benchmarks)
			{
                double normalizedEnergy = (benchmark.Energy - minEnergy) / (maxEnergy - minEnergy);
                Color green = Color.FromRgb(144, 238, 144);
                Color red = Color.FromRgb(252, 108, 133);
                Color color = ColorExtensions.LerpColor(red, green, normalizedEnergy); // Inverted color range
                SolidColorBrush rowColor = new SolidColorBrush(color);
                benchmark.RowColor = rowColor;
            }

            return benchmarks;
		}
	}
}
