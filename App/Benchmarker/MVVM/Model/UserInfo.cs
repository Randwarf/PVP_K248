using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Net.Http;

namespace Benchmarker.MVVM.Model
{
    internal static class UserInfo
    {
        private static Settings _Settings;
        public static Settings Settings 
        { 
            get 
            {
                if (_Settings == null)
                    _Settings = ReadSettings();
                return _Settings;
            }
            set
            {
                _Settings = value;
                SaveSettings();                
            }
        }

        public static string IPAdress = "0.0.0.0"; 

        private static string Path()
        {
            var tempPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Benchmarker";

            if (Directory.Exists(tempPath) == false)
            {
                DirectoryInfo directory = new DirectoryInfo(tempPath);
                directory.Create();
            }

            return tempPath + "/Settings.json";
        }

        private static Settings ReadSettings()
        {
            Settings settings = new Settings();
            if (File.Exists(Path()) == true)
            {
                using (StreamReader reader = new StreamReader(Path()))
                {
                    string content = reader.ReadToEnd();
                    settings = JsonConvert.DeserializeObject<Settings>(content);
                }
            }
            return settings;
        }

        private static void SaveSettings()
        {
            string content = JsonConvert.SerializeObject(Settings);
            using (StreamWriter writer = new StreamWriter(Path(), false))
            {
                writer.Write(content);
            }
        }

        public static async void UpdateAsyncPublicIPAddress()
        {
            string url = "https://www.manoip.lt/";
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(url);
            string a1 = response.Split(new string[] { "<b>" }, StringSplitOptions.None)[1];
            string a2 = a1.Split(new string[] { "</b>" }, StringSplitOptions.None)[0];
            IPAdress = a2;            
        }
    }
}