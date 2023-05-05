using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.Database;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Benchmarker.MVVM.ViewModel.Account
{
    internal class LoggedOutViewModel : ObservableObject
    {
        public RelayCommand SwitchViewCommand { get; set; }
        public RelayCommand RegisterCommand { get; set; }
        public RelayCommand LoginCommand { get; set; }

        private readonly IUserRepository userRepository;

        private bool isInteractable = true;

        private string emailError;
        private string passwordError;
        private string emailText;
        private string passwordText;
		
        private string informationText;

		public bool IsInteractable
        {
            get { return isInteractable; }
            set { isInteractable = value; OnPropertyChanged(); }
        }

        public string EmailError
        {
            get { return emailError; }
            set { emailError = value; OnPropertyChanged(); }
        }

        public string PasswordError
        {
            get { return passwordError; }
            set { passwordError = value; OnPropertyChanged(); }
        }

        public string EmailText
        {
            get { return emailText; }
            set { emailText = value; OnPropertyChanged(); }
        }

        public string PasswordText
        {
            get { return passwordText; }
            set { passwordText = value; OnPropertyChanged(); }
        }

		public string InformationText
		{
			get { return informationText; }
			set { informationText = value; OnPropertyChanged(); }
		}

		public LoggedOutViewModel(RelayCommand switchView) {
            userRepository = new UserRepository();
            SwitchViewCommand = switchView;

            RegisterCommand = new RelayCommand(o =>
            {
                Register();
            });

            LoginCommand = new RelayCommand(o =>
            {
                Login();
            });
        }

        public async void Register()
        {
            IsInteractable = false;
            ResetErrors();

            if (!await IsAPIWorking())
            {
                IsInteractable = true;
                return;
            }

            if (!IsInputValid())
            {
                IsInteractable = true;
                return;
            }

            var user = new User
            {
                Email = EmailText,
                Password = PasswordText,
                premiumEndDate = "1900-01-01"
            };

            string email = EmailText;
            var registeredUser = await userRepository.GetUserByEmail(email);
            if (registeredUser != null)
            {
                EmailError = "User with this email already exists";
                IsInteractable = true;
                return;
            }

            userRepository.InsertUser(user);

            ResetInputs();
            IsInteractable = true;

            AccountManager.SetUser(user);
            SwitchViewCommand.Execute(this);
        }

        public async void Login()
        {
            IsInteractable = false;
            ResetErrors();

			if (!await IsAPIWorking())
			{
				IsInteractable = true;
				return;
			}

			if (!IsInputValid())
            {
                IsInteractable = true;
                return;
            }

            var user = new User
            {
                Email = EmailText,
                Password = PasswordText,
                premiumEndDate = "1900-01-01"
            };

            string email = EmailText;
            var registeredUser = await userRepository.GetUserByEmail(email);
            if (registeredUser == null)
            {
                EmailError = "Account with this email was not found";
                IsInteractable = true;
                return;
            }

            var loggedInUser = await userRepository.Login(user);
            if (loggedInUser == null)
            {
                PasswordError = "Wrong password";
                IsInteractable = true;
                return;
            }

            ResetInputs();
            IsInteractable = true;

            AccountManager.SetUser(loggedInUser);
            SwitchViewCommand.Execute(this);
        }

        private bool IsInputValid()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(EmailText))
            {
                EmailError = "Email can't be empty";
                isValid = false;
            } else if (!EmailText.Contains("@"))
            {
                EmailError = "Incorrect email";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(PasswordText))
            {
                PasswordError = "Password can't be empty";
                isValid = false;
            }

            return isValid;
        }

        private async Task<bool> IsAPIWorking()
        {
			bool apiWorking = await APIStatus.IsAPIWorking();
			
            if (!apiWorking)
			{
				isInteractable = true;
				InformationText = "Our servers are currently down. Try again later.";
			}
            
            return apiWorking;
		}

        private void ResetInputs()
        {
            EmailText = "";
            PasswordText = "";
        }

        private void ResetErrors()
        {
            EmailError = "";
            PasswordError = "";
            InformationText = "";
        }
    }
}
