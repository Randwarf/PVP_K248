using Benchmarker.Core;

namespace Benchmarker.MVVM.ViewModel.Account
{
    internal class LoggedInViewModel
    {
        public RelayCommand SwitchView { get; set; }

        public LoggedInViewModel(RelayCommand switchView)
        {
            SwitchView = new RelayCommand(o =>
            {
                switchView.Execute(this);
            });
        }
    }
}
