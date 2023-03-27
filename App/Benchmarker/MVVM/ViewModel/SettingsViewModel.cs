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

        public SettingsViewModel() { }
    }
}
