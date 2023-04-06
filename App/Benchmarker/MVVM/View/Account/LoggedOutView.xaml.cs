using System.Windows;
using System.Windows.Controls;
using Benchmarker.MVVM.Model.Database;
using Benchmarker.MVVM.ViewModel.Account;

namespace Benchmarker.MVVM.View.Account
{
    /// <summary>
    /// Interaction logic for LoggedOutView.xaml
    /// </summary>
    public partial class LoggedOutView : UserControl
    {
        public LoggedOutView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((LoggedOutViewModel)DataContext).PasswordText = ((PasswordBox)sender).Password;
            }
        }
    }
}
