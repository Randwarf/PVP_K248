using System;
using System.Web.UI.WebControls;
using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.Database;

namespace Benchmarker.MVVM.ViewModel.Account
{
    internal class LoggedOutViewModel : ObservableObject
    {
        public RelayCommand SwitchViewCommand { get; set; }
        public RelayCommand RegisterCommand { get; set; }
        public RelayCommand LoginCommand { get; set; }

        private readonly IUserRepository userRepository;

        private string emailError;
        private string passwordError;
        private string emailText;
        private string passwordText;

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

        public LoggedOutViewModel(RelayCommand switchView) {
            userRepository = new UserRepository();

            SwitchViewCommand = new RelayCommand(o =>
            {
                switchView.Execute(this);
            });

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
            EmailError = "";
            PasswordError = "";

            if (string.IsNullOrWhiteSpace(EmailText))
            {
                EmailError = "Email can't be empty";
                return;
            }

            if (!EmailText.Contains("@"))
            {
                EmailError = "Incorrect email";
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordText))
            {
                PasswordError = "Password can't be empty";
                return;
            }

            var user = new User
            {
                Email = EmailText,
                Password = PasswordText,
                IsPremium = false
            };

            string email = EmailText;
            var registeredUser = await userRepository.GetUserByEmail(email);
            if (registeredUser != null)
            {
                EmailError = "User with this email already exists";
                return;
            }

            userRepository.InsertUser(user);

            EmailText = "";
            PasswordText = "";
            EmailError = "";
            PasswordError = "";

            Console.WriteLine("Registered");

            SwitchViewCommand.Execute(this);
        }

        public async void Login()
        {
            EmailError = "";
            PasswordError = "";

            if (string.IsNullOrWhiteSpace(EmailText))
            {
                EmailError = "Email can't be empty";
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordText))
            {
                PasswordError = "Password can't be empty";
                return;
            }

            var user = new User
            {
                Email = EmailText,
                Password = PasswordText,
                IsPremium = false
            };

            string email = EmailText;
            var registeredUser = await userRepository.GetUserByEmail(email);
            if (registeredUser == null)
            {
                EmailError = "Account with this email was not found";
                return;
            }

            var loggedInUser = await userRepository.Login(user);
            if (loggedInUser == null)
            {
                PasswordError = "Wrong password";
                return;
            }

            Console.WriteLine("Logged in");
            SwitchViewCommand.Execute(this);
        }
    }
}
