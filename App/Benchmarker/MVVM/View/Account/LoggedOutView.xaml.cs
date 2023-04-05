using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.Database;
using Benchmarker.MVVM.ViewModel.Account;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Benchmarker.MVVM.View.Account
{
    /// <summary>
    /// Interaction logic for LoggedOutView.xaml
    /// </summary>
    public partial class LoggedOutView : UserControl
    {
        private readonly IUserRepository userRepository;

        public LoggedOutView()
        {
            InitializeComponent();
            userRepository = new UserRepository();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((LoggedOutViewModel)DataContext).PasswordText = ((PasswordBox)sender).Password;
            }
        }

        public async void Register_OnClick(object sender, RoutedEventArgs e)
        {
            EmailError.Text = "";
            PasswordError.Text = "";

            if (string.IsNullOrWhiteSpace(EmailText.Text))
            {
                EmailError.Text = "Email can't be empty";
                return;
            }

            if (!EmailText.Text.Contains("@"))
            {
                EmailError.Text = "Incorrect email";
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordText.Password))
            {
                PasswordError.Text = "Password can't be empty";
                return;
            }

            var user = new User
            {
                Email = EmailText.Text,
                Password = PasswordText.Password,
                IsPremium = false
            };

            string email = EmailText.Text;
            var registeredUser = await userRepository.GetUserByEmail(email);
            if (registeredUser != null)
            {
                EmailError.Text = "User with this email already exists";
                return;
            }

            userRepository.InsertUser(user);

            EmailText.Text = "";
            PasswordText.Password = "";
            EmailError.Text = "";
            PasswordError.Text = "";

            Console.WriteLine("Registered");

            ((LoggedOutViewModel)DataContext).SwitchViewCommand.Execute(this);
        }

        public async void Login_OnClick(object sender, RoutedEventArgs e)
        {
            EmailError.Text = "";
            PasswordError.Text = "";

            if (string.IsNullOrWhiteSpace(EmailText.Text))
            {
                EmailError.Text = "Email can't be empty";
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordText.Password))
            {
                PasswordError.Text = "Password can't be empty";
                return;
            }

            var user = new User
            {
                Email = EmailText.Text,
                Password = PasswordText.Password,
                IsPremium = false
            };

            string email = EmailText.Text;
            var registeredUser = await userRepository.GetUserByEmail(email);
            if (registeredUser == null)
            {
                EmailError.Text = "Account with this email was not found";
                return;
            }

            var loggedInUser = await userRepository.Login(user);
            if (loggedInUser == null)
            {
                PasswordError.Text = "Wrong password";
                return;
            }

            Console.WriteLine("Logged in");
            ((LoggedOutViewModel)DataContext).SwitchViewCommand.Execute(this);
        }
    }
}
