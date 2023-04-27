using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.DTOs;
using Benchmarker.MVVM.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Benchmarker.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand BenchmarkViewCommand { get; set; }
        public RelayCommand HistoryViewCommand { get; set; }
        public RelayCommand CompareViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand AccountViewCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }

        public BenchmarkViewModel BenchmarkVM { get; set; }
        public HistoryViewModel HistoryVM { get; set; }
        public CompareViewModel CompareVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public AccountViewModel AccountVM { get; set; }

        public Action Close { get; set; }


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

        public MainViewModel()
        {
            UserInfo.UpdateAsyncPublicIPAddress();
            CreateModels();
            CurrentView = BenchmarkVM;
            CreateCommands();
            ValidateDataSharing();
        }

        private void ValidateDataSharing()
        {
            if (!UserInfo.Settings.agreedToDataSharing)
            {
                var popUp = new DataSharingPopUp();
                popUp.ShowDialog();
            }
        }

        private void CreateModels()
        {
            BenchmarkVM = new BenchmarkViewModel();
            HistoryVM = new HistoryViewModel();
            CompareVM = new CompareViewModel();
            SettingsVM = new SettingsViewModel();
            AccountVM = new AccountViewModel();
        }

        private void CreateCommands()
        {
            BenchmarkViewCommand = new RelayCommand(o =>
            {
                CurrentView = BenchmarkVM;
            });

            HistoryViewCommand = new RelayCommand(o =>
            {
                CurrentView = HistoryVM;
            });

            CompareViewCommand = new RelayCommand(o =>
            {
                HistoryBenchmark benchmark1 = null;
                HistoryBenchmark benchmark2 = null;

                var historyControl = new HistoryBenchmarkSelection();
                bool? success = historyControl.ShowDialog();

                if (success == true && historyControl.ChosenBenchmarks != null && historyControl.ChosenBenchmarks.Count == 2)
                {
                    benchmark1 = historyControl.ChosenBenchmarks[0];
                    benchmark2 = historyControl.ChosenBenchmarks[1];
                    var comparisonRows = BenchmarkCompareService.CompareBenchmarks(benchmark1, benchmark2);
                    CompareVM.BenchmarkComparisonRows = comparisonRows;
                    CurrentView = CompareVM;
                }
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });

            AccountViewCommand = new RelayCommand(o =>
            {
                CurrentView = AccountVM;
            });

            ExitCommand = new RelayCommand(o =>
            {
                BenchmarkVM.RunVM.StopBenchmark();
                Close?.Invoke();
            });
        }
    }
}
