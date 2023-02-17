using Benchmarker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.ViewModel
{
    class BenchmarkStartViewModel
    {
        public RelayCommand SwitchView { get; set; }
        public BenchmarkStartViewModel(RelayCommand switchView) 
        {
            SwitchView = switchView;
        }
    }
}
