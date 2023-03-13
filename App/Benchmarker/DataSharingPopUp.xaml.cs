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
            UserInfo userInfo = new UserInfo();
            userInfo.settings.agreedToDataSharing = false;
            userInfo.saveSettings();
            this.Close();
        }

        private void AcceptClick(Object sender, RoutedEventArgs e)
        {
            UserInfo userInfo = new UserInfo();
            userInfo.settings.agreedToDataSharing = true;
            userInfo.saveSettings();
            this.Close();
        }
    }
}
