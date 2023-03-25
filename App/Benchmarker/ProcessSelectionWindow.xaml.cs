using Benchmarker.MVVM.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Benchmarker.DTOs;

namespace Benchmarker
{
    /// <summary>
    /// Interaction logic for ProcessSelectionWindow.xaml
    /// </summary>
    public partial class ProcessSelectionWindow : Window
    {
        public KeyValuePair<Process, List<Process>> ChosenProcess { get; private set; }
        
        private Dictionary<Process, List<Process>> topLevelProcesses;

        public ProcessSelectionWindow()
        {
            InitializeComponent();

            List<ProcessInfo> processes = new List<ProcessInfo>();
            topLevelProcesses = ProcessService.GetTopLevelProcesses();

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
                var row = e.Source as DataGridRow;
                ProcessInfo info = row.Item as ProcessInfo;
                var process = topLevelProcesses.Where(x => x.Key.Id == info.Id).First();
                ChosenProcess = process;
                DialogResult = true;
                Hide();
            }
        }
    }
}
