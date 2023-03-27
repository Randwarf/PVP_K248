using Benchmarker.Core;
using Benchmarker.Database;
using Benchmarker.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Benchmarker.MVVM.ViewModel
{
    internal class BenchmarkRunViewModel : ObservableObject
    {
        private const double graphHeight = 130;

        public RelayCommand SwitchView { get; set; }
        public string appName { get; set; }
        public string instanceName { get; set; }
        private string _currentCPU { get; set; }
        private string _currentMemory { get; set; }
        private string _currentDisk { get; set; }

        private KeyValuePair<Process, List<Process>> _process;

        private DispatcherTimer _timer;
        private DateTime prevCheck;
        // TODO: REWORK HOW AVG VALUES ARE CALCULATED!
        private int ticksChecked = 0;

        private List<double> _historyCPU;
        private List<double> _historyMemory;
        private List<double> _historyDisk;

        private CPUService cpuService;
        private MemoryService memoryService;
        private DiskService diskService;

        private readonly IBenchmarkRepository benchmarkRepository;

        // Process with it's child processes
        public KeyValuePair<Process, List<Process>> Process
        {
            get { return _process; }
            set
            {
                _process = value;
                appName = _process.Key.ProcessName;
                prevCheck = DateTime.Now;

                cpuService = new CPUService(_process.Value.Concat(new List<Process>() { _process.Key }).ToList());
                memoryService = new MemoryService(_process.Value.Concat(new List<Process>() { _process.Key }).ToList());
                diskService = new DiskService(_process.Value.Concat(new List<Process>() { _process.Key }).ToList());

                _timer = new DispatcherTimer();
                _timer.Tick += new EventHandler(dispatcherTimer_Tick);
                _timer.Interval = new TimeSpan(0, 0, 1);
                _timer.Start();

                _historyCPU = new List<double>();
                _historyMemory = new List<double>();
                _historyDisk = new List<double>();
                for (int i = 0; i < 280; i++)
                {
                    _historyCPU.Add(0);
                    _historyMemory.Add(0);
                    _historyDisk.Add(0);
                }

                OnPropertyChanged();
            }
        }

        public string currentCPU
        {
            get { return _currentCPU; }
            set
            {
                _currentCPU = value;
                OnPropertyChanged();
            }
        }

        public string currentMemory
        {
            get { return _currentMemory; }
            set
            {
                _currentMemory = value;
                OnPropertyChanged();
            }
        }

        public string currentDisk
        {
            get { return _currentDisk; }
            set
            {
                _currentDisk = value;
                OnPropertyChanged();
            }
        }

        public string historyCPU
        {
            get
            {
                return GetGraphString(_historyCPU, graphHeight);
            }
            set
            {
                _historyCPU.RemoveAt(0);
                _historyCPU.Add(float.Parse(value));
                OnPropertyChanged();
            }
        }

        public string historyMemory
        {
            get
            {
                return GetGraphString(_historyMemory, graphHeight);
            }
            set
            {
                _historyMemory.RemoveAt(0);
                _historyMemory.Add(float.Parse(value));
                OnPropertyChanged();
            }
        }

        public string historyDisk
        {
            get
            {
                return GetGraphString(_historyDisk, graphHeight);
            }
            set
            {
                _historyDisk.RemoveAt(0);
                _historyDisk.Add(float.Parse(value));
                OnPropertyChanged();
            }
        }

        public BenchmarkRunViewModel(RelayCommand switchView)
        {
            appName = "INSTANTIATING";
            SwitchView = new RelayCommand(o =>
            {
                if (_process.Key != null && !_process.Key.HasExited)
                {
                    StopBenchmark();
                }

                switchView.Execute(this);
            });

            benchmarkRepository = new BenchmarkRepository();
        }

        private double CalculateAvg(IEnumerable<double> numbers)
        {
            return numbers.Skip(280 - ticksChecked)
                    .Sum() / ticksChecked;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (_process.Key.HasExited || _process.Key == null)
            {
                StopBenchmark();
            }

            TimeSpan elapsed = DateTime.Now - prevCheck;

            if (elapsed.TotalSeconds > 0)
            {
                var cpuPercentage = cpuService.GetPercentage();
                currentCPU = string.Format("CPU: {0}%. Max: {1:0.00}%", cpuPercentage, _historyCPU.Max());
                historyCPU = cpuPercentage.ToString();

                double memoryPercentage = memoryService.GetPercentage();
                double memoryRawValue = memoryService.GetRawValue();
                currentMemory = string.Format("RAM: {0}% - {1:0.00}Mb. Max: {2:0.00}%", memoryPercentage, memoryRawValue / 1024, _historyMemory.Max());
                historyMemory = memoryPercentage.ToString();

                double diskRawValue = diskService.GetRawValue();
                currentDisk = string.Format("DISK: {0}Mb/s. Max:{1:0}Mb/s", diskRawValue, _historyDisk.Max());
                historyDisk = diskRawValue.ToString();
            }

            prevCheck = DateTime.Now;
            ticksChecked++;
        }

        public void StopBenchmark()
        {
            if (_timer == null || !_timer.IsEnabled)
                return;
            _timer.Stop();
            double avgCPUPercent = CalculateAvg(_historyCPU);
            double avgMemoryPercent = CalculateAvg(_historyMemory);

            var benchmark = new Benchmark()
            {
                Date = DateTime.Now,
                Process = appName,
                CPU = Math.Round(avgCPUPercent, 2),
                RAM = Math.Round(avgMemoryPercent, 2),
                Energy = -1,
                Disk = -1
            };

            if (UserInfo.Settings.agreedToDataSharing)
            {
                benchmarkRepository.InsertBenchmark(benchmark);
            }

            HistoryService.AddBenchmark(benchmark);
        }

        private string GetGraphString(IEnumerable<double> y, double graphHeight)
        {
            double maxValue = y.Max();
            if (maxValue <= 0) maxValue = 0.0001;
            double heightRatio = graphHeight / maxValue;
            var builder = new StringBuilder();
            for (int i = 0; i < y.Count(); i++)
            {
                double yPos = (maxValue - y.ElementAt(i)) * heightRatio;
                string yPosWithDot = yPos.ToString().Replace(",", ".");
                builder.Append(i + "," + yPosWithDot + " ");
            }
            return builder.ToString();
        }
    }
}
