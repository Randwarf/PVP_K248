using Benchmarker.MVVM.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Benchmarker.DTOs;
using System;

namespace Benchmarker
{
    /// <summary>
    /// Interaction logic for ProcessSelectionWindow.xaml
    /// </summary>
    public partial class ProcessSelectionWindow : Window
    {
        public KeyValuePair<Process, List<Process>> ChosenProcess { get; private set; }
        
        private Dictionary<Process, List<Process>> topLevelProcesses;

        public ProcessSelectionWindow(Dictionary<Process, List<Process>> topLevelProcesses)
        {
            InitializeComponent();

            this.topLevelProcesses = topLevelProcesses;

            List<ProcessInfo> processes = new List<ProcessInfo>();
            //topLevelProcesses = ProcessService.GetTopLevelProcesses();

            topLevelProcesses.Keys
                .ToList()
                .ForEach(x =>
                {
                    processes.Add(new ProcessInfo()
                    {
                        Id = x.Id,
                        Name = x.ProcessName
                    });
                });

            processes = processes.OrderBy(x => x.Name).ToList();
            ProcessListGrid.ItemsSource = processes;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var row = e.Source as ListBoxItem;
                ProcessInfo info = row.Content as ProcessInfo;
                var process = topLevelProcesses.Where(x => x.Key.Id == info.Id).First();
                ChosenProcess = process;
                DialogResult = true;
                Hide();
            }
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
