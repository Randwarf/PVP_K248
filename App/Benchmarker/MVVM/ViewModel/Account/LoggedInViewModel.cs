using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using System.Diagnostics;
using System.Windows.Controls;

namespace Benchmarker.MVVM.ViewModel.Account
{
    internal class LoggedInViewModel : ObservableObject
    {
        public RelayCommand SwitchViewCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        private string loginMessage;

        public string LoginMessage
        {
            get { return loginMessage; }
            set { loginMessage = value; OnPropertyChanged(); }
        }

        public LoggedInViewModel(RelayCommand switchView)
        {
            SwitchViewCommand = switchView;

            LogoutCommand = new RelayCommand(o =>
            {
                Logout();
            });
        }
        
        public void RefreshData()
        {
            User loggedInUser = AccountManager.GetUser();
            LoginMessage = $"Logged in as {loggedInUser.Email}";
        }

        public void Logout()
        {
            AccountManager.SetUser(null);
            SwitchViewCommand.Execute(this);
        }
    }
}
