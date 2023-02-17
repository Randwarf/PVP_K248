using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Benchmarker.MVVM.View
{
    /// <summary>
    /// Interaction logic for BenchmarkView.xaml
    /// </summary>
    public partial class BenchmarkView : UserControl
    {
        public BenchmarkView()
        {
            InitializeComponent();
        }

        private void StartBenchmark_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".exe";
            dlg.Filter = "EXE Files (*.exe)|*.exe";
            Nullable<bool> result = dlg.ShowDialog();

            if (result != true) return;

            string filename = dlg.FileName;
            ProcessSelection.Visibility= System.Windows.Visibility.Collapsed;
            Benchmarking.Visibility = System.Windows.Visibility.Visible;
            BenchmarkName.Text = dlg.SafeFileName;
        }
    }
}
