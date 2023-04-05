using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.Database;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Benchmarker.MVVM.View
{
    /// <summary>
    /// Interaction logic for AccountView.xaml
    /// </summary>
    public partial class AccountView : UserControl
    {
        private IUserRepository userRepository;

        public AccountView()
        {
            InitializeComponent();

            userRepository = new UserRepository();
        }

        public async void Register_OnClick(object sender, RoutedEventArgs e)
        {
            EmailError.Text = "";
            PasswordError.Text = "";

            if (string.IsNullOrWhiteSpace(EmailText.Text))
            {
                Console.WriteLine("Email can't be empty");
                EmailError.Text = "Email can't be empty";
                return;
            }

            if (!EmailText.Text.Contains("@"))
            {
                Console.WriteLine("Incorrect email");
                EmailError.Text = "Incorrect email";
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordText.Password))
            {
                Console.WriteLine("Password can't be empty");
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
                Console.WriteLine("User with this email already exists");
                EmailError.Text = "User with this email already exists";
                return;
            }

            userRepository.InsertUser(user);

            EmailText.Text = "";
            PasswordText.Password = "";
            EmailError.Text = "";
            PasswordError.Text = "";
        }

        public async void Login_OnClick(object sender, RoutedEventArgs e)
        {
            EmailError.Text = "";
            PasswordError.Text = "";

            if (string.IsNullOrWhiteSpace(EmailText.Text))
            {
                Console.WriteLine("Email can't be empty");
                EmailError.Text = "Email can't be empty";
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordText.Password))
            {
                Console.WriteLine("Password can't be empty");
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
                Console.WriteLine("Account with this email was not found");
                EmailError.Text = "Account with this email was not found";
                return;
            }

            var loggedInUser = await userRepository.Login(user);
            if (loggedInUser == null)
            {
                Console.WriteLine("Wrong password");
                PasswordError.Text = "Wrong password";
                return;
            }

            Console.WriteLine("Logged in");
        }
    }
}
