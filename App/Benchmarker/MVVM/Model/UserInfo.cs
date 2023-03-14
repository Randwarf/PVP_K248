using Newtonsoft.Json;
using System;
using System.IO;

namespace Benchmarker.MVVM.Model
{
    internal class UserInfo
    {
        public Settings Settings;

        private string path;

        public UserInfo()
        {
            // TODO: Gal visą šitą path ištraukimą ir sukurimą iškelti į atskirą static klasę? 
            // Dubliuojasi ir History.cs'e
            path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Benchmarker";

            if (Directory.Exists(path) == false)
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                directory.Create();
            }

            path += "/Settings.json";
            
            try // Reading actual settings
            {
                if (File.Exists(path) == false)
                {
                    Settings = new Settings();
                    return;
                }

                using (StreamReader reader = new StreamReader(path))
                {
                    string content = reader.ReadToEnd();
                    Settings = JsonConvert.DeserializeObject<Settings>(content);
                }
            }
            catch
            {
                // Something went wrong - use default settings
                Settings = new Settings();
            }
        }

        public void SaveSettings()
        {
            string content = JsonConvert.SerializeObject(Settings);
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(content);
            }
        }
    }
}