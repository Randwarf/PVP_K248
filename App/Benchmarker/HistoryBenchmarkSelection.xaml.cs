using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.DTOs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Benchmarker
{
    /// <summary>
    /// Interaction logic for HistoryBenchmarkSelection.xaml
    /// </summary>
    public partial class HistoryBenchmarkSelection : Window
    {
        public HistoryBenchmark ChosenBenchmark { get; private set; }

        public HistoryBenchmarkSelection()
        {
            InitializeComponent();

            BenchmarkList.ItemsSource = HistoryService.GetBenchmarks();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var row = e.Source as DataGridRow;
                HistoryBenchmark benchmark = row.Item as HistoryBenchmark;
                ChosenBenchmark = benchmark;
                DialogResult = true;
                Hide();
            }
        }
    }
}
