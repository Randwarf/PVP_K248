using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Benchmarker.Core;
using Benchmarker.MVVM.Model;

namespace Benchmarker.MVVM.ViewModel
{
    internal class HistoryViewModel : ObservableObject
    {
        private List<Benchmark> _benchmarks;
        private FileSystemWatcher _watcher;


        public List<Benchmark> benchmarks {
            get
            {
                return _benchmarks;
            }
            set
            {
                _benchmarks = value;
                OnPropertyChanged();
            }
        }

        public HistoryViewModel()
        {
            UpdateTable();
            _watcher = History.FileSystemWatcher();
            _watcher.Created += OnCreate;
        }
        private void OnCreate(object sender, FileSystemEventArgs e)
        {
            UpdateTable();
        }

        private void UpdateTable()
        {
            _benchmarks = History.ReadHistory();
            benchmarks = _benchmarks; //: ) fuck this
        }
    }
}
