using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.DTOs;
using System;
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

        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        // State change FOR LATER IMPLEMENTATION
        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
                //MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                //MaximizeButton.Visibility = Visibility.Visible;
            }
        }
    }

}
