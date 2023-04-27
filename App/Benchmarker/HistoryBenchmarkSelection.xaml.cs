using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.DTOs;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Benchmarker
{
    /// <summary>
    /// Interaction logic for HistoryBenchmarkSelection.xaml
    /// </summary>
    public partial class HistoryBenchmarkSelection : Window
    {
        public List<HistoryBenchmark> ChosenBenchmarks = new List<HistoryBenchmark>();

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
                var benchmark = row.Item as HistoryBenchmark;

                if (ChosenBenchmarks.Count >= 2 && !ChosenBenchmarks.Contains(benchmark))
                {
                    //MessageBox.Show("Please select only two items.");
                    return;
                }

                if (ChosenBenchmarks.Contains(benchmark))
                {
                    ChosenBenchmarks.Remove(benchmark);
                    row.BorderBrush = null;
                    row.BorderThickness = new Thickness(left: 0, top: 0, right: 0, bottom: 0);
                }
                else
                {
                    ChosenBenchmarks.Add(benchmark);
                    var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffc106"));
                    row.BorderBrush = brush;
                    row.BorderThickness = new Thickness(left: 5, top: 0, right: 0, bottom: 0);
                }

                OkButton.IsEnabled = ChosenBenchmarks.Count == 2;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChosenBenchmarks.Count != 2)
            {
                //MessageBox.Show("Please select two items.");
                return;
            }

            DialogResult = true;
            Hide();
        }
    }

}
