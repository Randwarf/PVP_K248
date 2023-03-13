using Newtonsoft.Json;
using System;
using System.IO;

namespace Benchmarker.MVVM.Model
{
    internal class UserInfo
    {
        public Settings settings;
        private string path;

        public UserInfo()
        {
            //TODO: Gal visą šitą path ištraukimą ir sukurimą iškelti į atskirą static klasę? 
            //Dubliuojasi ir History.cs'e
            path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Benchmarker";
            DirectoryInfo directory = new DirectoryInfo(path);
            directory.Create();
            path += "/Settings.json";
            try //reading actual settings
            {
                using(StreamReader reader = new StreamReader(path))
                {
                    string content = reader.ReadToEnd();
                    settings = JsonConvert.DeserializeObject<Settings>(content);
                }
            }
            catch
            {
                //Something went wrong - use default settings
                settings = new Settings();
            }
        }

        public void saveSettings()
        {
            string content = JsonConvert.SerializeObject(settings);
            using(StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(content);
            }
        }
    }
}