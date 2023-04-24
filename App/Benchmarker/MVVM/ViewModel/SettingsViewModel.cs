using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.ViewModel
{
    internal class SettingsViewModel : ObservableObject
    {
        public bool isSharing 
        { 
            get 
            {
                return UserInfo.Settings.agreedToDataSharing;
            } 
            set
            {
                var tempSettings = UserInfo.Settings;
                tempSettings.agreedToDataSharing = value;
                UserInfo.Settings = tempSettings;
                OnPropertyChanged();
            }
        }

        public RelayCommand LightThemeCommand { get; set; }

        public SettingsViewModel() 
        {
            LightThemeCommand = new RelayCommand(o =>
            {
                var Application = App.Current as App;
                Application.ChangeTheme(new Uri("Theme/LightTheme.xaml", UriKind.Relative));
            });
        }
    }
}
