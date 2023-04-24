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

        private int toggledIndex;
        public bool[] isToggled
        {
            get
            {
                var bools = Enumerable.Repeat(false, 2).ToArray();
                bools[toggledIndex] = true;
                return bools;
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
        }

        private void GenerateCommands()
        {
            ThemeCommands = new RelayCommand[PathNames.Length];
            for (int i = 0; i < PathNames.Length; i++)
            {
                var theme = PathNames[i];
                ThemeCommands[i] = new RelayCommand(o =>
                {
                    ApplyTheme(theme);
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
