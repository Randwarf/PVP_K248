using Benchmarker.Core;
using Benchmarker.MVVM.ViewModel.Account;

namespace Benchmarker.MVVM.ViewModel
{
    internal class AccountViewModel : ObservableObject
    {
        public RelayCommand SwitchLoggedInViewCommand { get; set; }
        public RelayCommand SwitchLoggedOutViewCommand { get; set; }

        public LoggedInViewModel LoggedInVM { get; set; }
        public LoggedOutViewModel LoggedOutVM { get; set; }

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

        public AccountViewModel()
        {
            SwitchLoggedInViewCommand = new RelayCommand(o =>
            {
                CurrentView = LoggedInVM;
            });

            SwitchLoggedOutViewCommand = new RelayCommand(o =>
            {
                CurrentView = LoggedOutVM;
            });

            LoggedInVM = new LoggedInViewModel(SwitchLoggedOutViewCommand);
            LoggedOutVM = new LoggedOutViewModel(SwitchLoggedInViewCommand);

            CurrentView = LoggedOutVM;
        }
    }
}
