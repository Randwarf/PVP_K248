using Benchmarker.Core;
using Benchmarker.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand BenchmarkViewCommand { get; set; }
        public RelayCommand HistoryViewCommand { get; set; }
        public RelayCommand CompareViewCommand { get; set; }

        public BenchmarkViewModel BenchmarkVM { get; set; }
        public HistoryViewModel HistoryVM { get; set; }
        public CompareViewModel CompareVM { get; set; }


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
                CurrentView = CompareVM;
            });
        }
    }
}
