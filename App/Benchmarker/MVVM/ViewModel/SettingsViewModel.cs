using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

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

        private int checkedIndex;
        public bool[] IsChecked
        {
            get
            {
                var bools = Enumerable.Repeat(false, 2).ToArray();
                if (checkedIndex >= 0)
                    bools[checkedIndex] = true;
                return bools;
            }
            set
            {
                OnPropertyChanged();
            }
        }

        public bool[] IsEnabled
        {
            get
            {
                bool premium = false;
                if (AccountManager.IsUserLoggedIn())
                    premium = AccountManager.GetUser().IsPremium;
                
                return new bool[] { true, premium };
            }
        }

        public string[] ThemeNames
        {
            get
            {
                return new string[] { "Light theme", "Dark theme" };
            }
        }

        public string[] PathNames
        {
            get
            {
                return new string[] { "Theme/LightTheme.xaml", "Theme/DarkTheme.xaml" };
            }
        }

        public RelayCommand[] ThemeCommands { get; set; }

        public SettingsViewModel()
        {
            GenerateCommands();
            FindSelectedIndex();
        }

        private void FindSelectedIndex()
        {
            var selectedTheme = UserInfo.Settings.currentTheme;
            checkedIndex = Array.IndexOf(PathNames, selectedTheme);

        }

        private void GenerateCommands()
        {
            ThemeCommands = new RelayCommand[PathNames.Length];
            for (int i = 0; i < PathNames.Length; i++)
            {
                var theme = PathNames[i];
                var index = i;
                ThemeCommands[i] = new RelayCommand(o =>
                {
                    ApplyTheme(theme);
                    checkedIndex = index;
                    OnPropertyChanged();
                });
            }
        }

        public void ApplyTheme(string theme)
        {
            var Application = App.Current as App;
            Application.ChangeTheme(new Uri(theme, UriKind.Relative));
            var settings = UserInfo.Settings;
            settings.currentTheme = theme;
            UserInfo.Settings = settings;
        }
    }
}
