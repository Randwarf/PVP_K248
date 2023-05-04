using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Benchmarker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void ChangeTheme(Uri uri)
        {
            Application.Current.Resources.MergedDictionaries[0].Source = uri;
        }
    }


}
