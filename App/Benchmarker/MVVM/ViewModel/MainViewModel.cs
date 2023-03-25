using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.DTOs;
using Benchmarker.MVVM.View;
using System;
using System.Collections.Generic;
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
        public RelayCommand ExitCommand { get; set; }

        public BenchmarkViewModel BenchmarkVM { get; set; }
        public HistoryViewModel HistoryVM { get; set; }
        public CompareViewModel CompareVM { get; set; }

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

                HistoryBenchmarkSelection historyWindow1 = new HistoryBenchmarkSelection();
                bool? success1 = historyWindow1.ShowDialog();
                historyWindow1.Close();
                if (success1 == false)
                {
                    return;
                }
                benchmark1 = historyWindow1.ChosenBenchmark;

                HistoryBenchmarkSelection historyWindow2 = new HistoryBenchmarkSelection();
                bool? success2 = historyWindow2.ShowDialog();
                historyWindow2.Close();
                if (success2 == false)
                {
                    return;
                }
                benchmark2 = historyWindow2.ChosenBenchmark;

                if (success1 == true && success2 == true)
                {
                    var comparisonRows = BenchmarkCompareService.CompareBenchmarks(benchmark1, benchmark2);
                    CompareVM.BenchmarkComparisonRows = comparisonRows;
                    CurrentView = CompareVM;
                }
            });

            ExitCommand = new RelayCommand(o =>
            {
                BenchmarkVM.RunVM.StopBenchmark();
                Close?.Invoke();
            });
        }
    }
}
