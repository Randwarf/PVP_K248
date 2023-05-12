using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.ViewModel.Account;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.ViewModel
{
    internal class AccountViewModel : ObservableObject
    {
        public RelayCommand SwitchLoggedInViewCommand { get; set; }
        public RelayCommand SwitchLoggedOutViewCommand { get; set; }

        public LoggedInViewModel LoggedInVM { get; set; }
        public LoggedOutViewModel LoggedOutVM { get; set; }
        public LoadingUserViewModel LoadingVM { get; set; }

        private object currentView;

        public object CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        public AccountViewModel(Task<bool> loggingInTask)
        {
            SwitchLoggedInViewCommand = new RelayCommand(o =>
            {
                LoggedInVM.RefreshData();
                CurrentView = LoggedInVM;
            });
            
            SwitchLoggedOutViewCommand = new RelayCommand(o =>
            {
                CurrentView = LoggedOutVM;
            });

            LoggedInVM = new LoggedInViewModel(SwitchLoggedOutViewCommand);
            LoggedOutVM = new LoggedOutViewModel(SwitchLoggedInViewCommand);
            LoadingVM = new LoadingUserViewModel();
            CurrentView = LoadingVM;
            waitForLogin(loggingInTask);
        }

        private async void waitForLogin(Task<bool> loggingIn)
        {
            var loggedIn = await loggingIn;
            if (loggedIn)
            {
                SwitchLoggedInViewCommand.Execute(this);
            }
            else
            {
                SwitchLoggedOutViewCommand.Execute(this);
            }
        }
    }
}
