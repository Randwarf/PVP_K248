using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using System.Diagnostics;

namespace Benchmarker.MVVM.ViewModel.Account
{
    internal class LoggedInViewModel
    {
        public RelayCommand SwitchViewCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        public LoggedInViewModel(RelayCommand switchView)
        {
            SwitchViewCommand = switchView;

            LogoutCommand = new RelayCommand(o =>
            {
                Logout();
            });
        }

        public void Logout()
        {
            AccountManager.SetUser(null);
            SwitchViewCommand.Execute(this);
        }
    }
}
