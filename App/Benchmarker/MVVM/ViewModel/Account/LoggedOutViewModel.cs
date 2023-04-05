using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.Database;
using System;

namespace Benchmarker.MVVM.ViewModel.Account
{
    internal class LoggedOutViewModel : ObservableObject
    {
        public RelayCommand SwitchView { get; set; }

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

            SwitchView = new RelayCommand(o =>
            {
                switchView.Execute(this);
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

            SwitchView.Execute(this);
        }
    }
}
