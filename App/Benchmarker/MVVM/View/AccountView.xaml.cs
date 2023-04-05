using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.Model.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            Console.WriteLine("Register: " + EmailText.Text + " " + PasswordText.Password);

            if (string.IsNullOrWhiteSpace(EmailText.Text))
            {
                Console.WriteLine("Email can't be empty");
                return;
            }

            if (!EmailText.Text.Contains("@"))
            {
                Console.WriteLine("Incorrect email");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordText.Password))
            {
                Console.WriteLine("Password can't be empty");
                return;
            }

            var user = new User
            {
                Email = EmailText.Text,
                Password = PasswordText.Password,
                IsPremium = false
            };

            var users = await userRepository.GetAllUsers();
            var registeredUser = users.Where(x => x.Email == user.Email).FirstOrDefault();

            if (registeredUser != null)
            {
                Console.WriteLine("User with this email already exists");
                return;
            }

            userRepository.InsertUser(user);

            EmailText.Text = "";
            PasswordText.Password = "";
        }

        public void Login_OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Login");
        }
    }
}
