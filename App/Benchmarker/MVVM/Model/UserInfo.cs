using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Shapes;

namespace Benchmarker.MVVM.Model
{
    internal static class UserInfo
    {
        private static Settings _Settings;
        public static Settings Settings { 
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

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "0.0.0.0";
        }
    }
}