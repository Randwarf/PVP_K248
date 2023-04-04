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
        public AccountView()
        {
            InitializeComponent();
        }

        public void Register_OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Register: " + EmailText.Text + " " + PasswordText.Password);
        }

        public void Login_OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Login");
        }
    }
}
