using Benchmarker.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.ViewModel
{
    internal class BenchmarkRunViewModel : ObservableObject
    {
        public RelayCommand SwitchView { get; set; }
        public string appName { get; set; }
        private OpenFileDialog _file { get; set; }


        public OpenFileDialog File
        {
            get { return _file; }
            set
            {
                _file = value;
                appName = _file.SafeFileName;
                OnPropertyChanged();
            }
        }

        public BenchmarkRunViewModel(RelayCommand switchView)
        {
            appName = "INSTANTIATING";
            SwitchView = switchView;
        }
    }
}
