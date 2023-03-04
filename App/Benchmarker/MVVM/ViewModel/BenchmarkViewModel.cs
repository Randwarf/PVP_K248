using Benchmarker.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Benchmarker.MVVM.ViewModel
{
    internal class BenchmarkViewModel : ObservableObject
    {
        public RelayCommand SwitchRunViewCommand { get; set; }
        public RelayCommand SwitchStartViewCommand { get; set; }


        public BenchmarkRunViewModel RunVM { get; set; }
        public BenchmarkStartViewModel StartVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public BenchmarkViewModel()
        {
            SwitchRunViewCommand = new RelayCommand(o =>
            {
                //Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                //dlg.DefaultExt = ".exe";
                //dlg.Filter = "EXE Files (*.exe)|*.exe";
                //Nullable<bool> result = dlg.ShowDialog();

                //if (result == true)
                //{
                //    CurrentView = RunVM;
                //    RunVM.File = dlg;
                //}

                ProcessSelectionWindow processWindow = new ProcessSelectionWindow();
                bool? success = processWindow.ShowDialog();
                if (success == true)
                {
                    Process process = processWindow.Process;
                    processWindow.Close();
                    CurrentView = RunVM;
                    RunVM.Process = process;
                }
            });

            SwitchStartViewCommand = new RelayCommand(o =>
            {
                CurrentView = StartVM;
            });

            RunVM = new BenchmarkRunViewModel(SwitchStartViewCommand);
            StartVM = new BenchmarkStartViewModel(SwitchRunViewCommand);

            CurrentView = StartVM;
        }
    }
}
