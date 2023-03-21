using Benchmarker.Core;

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
