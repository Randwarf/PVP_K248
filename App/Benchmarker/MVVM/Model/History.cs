using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.Model
{
    internal class History
    {
        const string relativePath = "/Benchmarker/Saves/";
        public static FileSystemWatcher FileSystemWatcher()
        {
            var watcher = new FileSystemWatcher(getAppDataPath()+relativePath);
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.json";
            watcher.EnableRaisingEvents = true;
            return watcher;
        }

        public static List<Benchmark> ReadHistory()
        {
            var history = new List<Benchmark>();
            string dirPath = getAppDataPath() + relativePath;
            var dir = new DirectoryInfo(dirPath);

            foreach (var fileInfo in dir.EnumerateFiles())
            {
                using (var reader = new StreamReader(fileInfo.Open(FileMode.Open)))
                {
                    var text = reader.ReadToEnd();
                    Benchmark entry = JsonConvert.DeserializeObject<Benchmark>(text);
                    history.Add(entry);
                }
            }

            return history;
        }

        public static void SaveBenchmark(Benchmark benchmark)
        {
            string fileRoot = getAppDataPath();
            string fileName = GenerateTimestampName();
            string fileBody = JsonConvert.SerializeObject(benchmark);
            CreatePath(fileRoot);
            using (StreamWriter writer = new StreamWriter(fileRoot + relativePath + fileName))
            {
                writer.WriteLine(fileBody);
            }
        }

        private static string getAppDataPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        private static string GenerateTimestampName()
        {
            var now = DateTime.Now;
            return string.Format("{0}-{1}-{2}-{3}-{4}-{5}.json",
                                                        now.Year,
                                                        now.Month,
                                                        now.Day,
                                                        now.Hour,
                                                        now.Minute,
                                                        now.Second);
        }

        private static void CreatePath(string fileRoot)
        {
            DirectoryInfo dir = new DirectoryInfo(fileRoot + "/Benchmarker");
            dir.Create();
            dir = new DirectoryInfo(fileRoot + "/Benchmarker/Saves");
            dir.Create();
        }
    }
}
