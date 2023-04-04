using Benchmarker.Core;

namespace Benchmarker.MVVM.ViewModel
{
    internal class AccountViewModel : ObservableObject
    {
        private string email;

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }
    }
}
