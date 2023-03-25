using Benchmarker.MVVM.Model.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Benchmarker.MVVM.Model
{
    public static class HistoryService
    {
        private const string RELATIVE_PATH = "Benchmarker/";
        private const string FILE_NAME = "Saves.json";

        public static Action OnBenchmarksChanged;

        private static List<HistoryBenchmark> Benchmarks = new List<HistoryBenchmark>();

        private static bool isRead = false;

        private static void ReadBenchmarks()
        {
            string absolutePath = Path.Combine(GetAppDataPath(), RELATIVE_PATH, FILE_NAME);
            
            if (File.Exists(absolutePath))
            {
                string savesJson = File.ReadAllText(absolutePath);
                Benchmarks = JsonConvert.DeserializeObject<List<HistoryBenchmark>>(savesJson);
            }

            isRead = true;
        }

        private static void SaveBenchmarks()
        {
            string absolutePath = Path.Combine(GetAppDataPath(), RELATIVE_PATH);
            string filePath = Path.Combine(absolutePath, FILE_NAME);

            if (!File.Exists(filePath))
            {
                new DirectoryInfo(absolutePath).Create();
            }

            string savesJson = JsonConvert.SerializeObject(Benchmarks);
            File.WriteAllText(filePath, savesJson);
        }

        public static List<HistoryBenchmark> GetBenchmarks()
        {
            if (!isRead)
            {
                ReadBenchmarks();
            }

            return Benchmarks;
        }

        public static void AddBenchmark(Benchmark benchmark)
        {
            Benchmarks.Add(new HistoryBenchmark(benchmark));
            SaveBenchmarks();
            OnBenchmarksChanged?.Invoke();
        }

        public static void DeleteBenchmark(HistoryBenchmark benchmark)
        {
            Benchmarks.Remove(benchmark);
            SaveBenchmarks();
            OnBenchmarksChanged?.Invoke();
        }

        private static string GetAppDataPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }
    }
}
