using Benchmarker.MVVM.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Benchmarker
{
    /// <summary>
    /// Interaction logic for DataSharingPopUp.xaml
    /// </summary>
    public partial class DataSharingPopUp : Window
    {
        public DataSharingPopUp()
        {
            InitializeComponent();
        }

        private void DeclineClick(Object sender, RoutedEventArgs e)
        {
            var updatedSettings = UserInfo.Settings;
            updatedSettings.agreedToDataSharing = false;
            UserInfo.Settings = updatedSettings;
            this.Close();
        }

        private void AcceptClick(Object sender, RoutedEventArgs e)
        {
            var updatedSettings = UserInfo.Settings;
            updatedSettings.agreedToDataSharing = true;
            UserInfo.Settings = updatedSettings;
            this.Close();
        }

        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        // State change FOR LATER IMPLEMENTATION
        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                RestoreButton.Visibility = Visibility.Visible;
                //MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                RestoreButton.Visibility = Visibility.Collapsed;
                //MaximizeButton.Visibility = Visibility.Visible;
            }
        }
    }
}
