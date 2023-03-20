using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.DTOs;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Benchmarker.MVVM.View
{
    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView : UserControl
    {
        public HistoryView()
        {
            InitializeComponent();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryBenchmark benchmark = ((FrameworkElement)sender).DataContext as HistoryBenchmark;
            HistoryService.DeleteBenchmark(benchmark);
        }
    }
}
