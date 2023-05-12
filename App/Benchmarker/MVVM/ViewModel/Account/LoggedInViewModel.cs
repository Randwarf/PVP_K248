using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.Database;
using System.Diagnostics;
using System.Windows.Controls;

namespace Benchmarker.MVVM.ViewModel.Account
{
    internal class LoggedInViewModel : ObservableObject
    {
        public RelayCommand SwitchViewCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        private readonly IUserRepository userRepository;

        private string loginMessage;

        public string LoginMessage
        {
            get { return loginMessage; }
            set { loginMessage = value; OnPropertyChanged(); }
        }

        public LoggedInViewModel(RelayCommand switchView)
        {
            SwitchViewCommand = switchView;
            userRepository = new UserRepository();

            LogoutCommand = new RelayCommand(o =>
            {
                Logout();
            });
        }
        
        public void RefreshData()
        {
            User loggedInUser = AccountManager.GetUser();
            var tempMessage = $"Logged in as {loggedInUser.Email}, ";
            if (loggedInUser.IsPremium)
            {
                tempMessage += "premium user";
            }
            else
            {
                tempMessage += "regular user";
            }
            LoginMessage = tempMessage;
        }

        public void Logout()
        {
            AccountManager.SetUser(null);
            userRepository.Logout(UserInfo.Settings.userToken);
            var settings = UserInfo.Settings;
            settings.userToken = null;
            UserInfo.Settings = settings;  
            SwitchViewCommand.Execute(this);
        }
    }
}
