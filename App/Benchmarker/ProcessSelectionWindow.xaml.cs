using Benchmarker.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Benchmarker
{
    /// <summary>
    /// Interaction logic for ProcessSelectionWindow.xaml
    /// </summary>
    public partial class ProcessSelectionWindow : Window
    {
        public Process Process { get; private set; }

        public ProcessSelectionWindow()
        {
            InitializeComponent();

            List<ProcessInfo> processes = new List<ProcessInfo>();
            Process.GetProcesses().ToList().ForEach(x =>
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

        private class ProcessInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var row = e.Source as DataGridRow;
                ProcessInfo info = row.Item as ProcessInfo;
                Process = Process.GetProcessById(info.Id);
                DialogResult = true;
                Hide();
            }
        }
    }
}
