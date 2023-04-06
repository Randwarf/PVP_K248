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
        private const int minBenchmarkTime = 30;

        private const int graphHeight = 130;
        private const int graphWidth = 280;

        public RelayCommand SwitchView { get; set; }
        public string appName { get; set; }
        public string instanceName { get; set; }

        private KeyValuePair<Process, List<Process>> _process;

        private DispatcherTimer _timer;
        private DateTime prevCheck;
        private int ticksChecked = 0;

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

                OnPropertyChanged();
            }
        }

        private string _cpuLabel;
        public string cpuLabel
        {
            get { return _cpuLabel; }
            private set
            {
                _cpuLabel = value;
                OnPropertyChanged();
            }
        }

        private string _memoryLabel;
        public string memoryLabel
        {
            get { return _memoryLabel; }
            set
            {
                _memoryLabel = value;
                OnPropertyChanged();
            }
        }

        private string _diskLabel;
        public string diskLabel
        {
            get { return _diskLabel; }
            set
            {
                _diskLabel = value;
                OnPropertyChanged();
            }
        }

        public string cpuGraph
        {
            get
            {
                return cpuService.GetGraphString(graphHeight, graphWidth);
            }
            set 
            {
                OnPropertyChanged();
            }
        }

        public string memoryGraph
        {
            get
            {
                return memoryService.GetGraphString(graphHeight, graphWidth);
            }
            set
            {
                OnPropertyChanged();
            }
        }

        public string diskGraph
        {
            get
            {
                return diskService.GetGraphString(graphHeight, graphWidth);
            }
            set
            {
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

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (_process.Key.HasExited || _process.Key == null)
            {
                StopBenchmark();
            }

            TimeSpan elapsed = DateTime.Now - prevCheck;

            if (elapsed.TotalSeconds > 0)
            {
                cpuService.CalculateNext();
                memoryService.CalculateNext();
                diskService.CalculateNext();
                cpuGraph = "update"; //I hate this part but i am not sure how to do the whole property changed thing otherwise
                memoryGraph = "update";
                diskGraph = "update";
                
                cpuLabel = string.Format("CPU: {0}%. Max: {1:0.00}%", 
                    cpuService.GetCurrentValue(), 
                    cpuService.GetMaxValue());
                memoryLabel = string.Format("RAM: {0}% - {1:0.00}Mb. Max: {2:0.00}%", 
                    memoryService.GetCurrentValue(), 
                    memoryService.GetRawValue() / 1024, 
                    memoryService.GetMaxValue());
                diskLabel = string.Format("Disk: {0}Mb/s. Max:{1:0.00}Mb/s", 
                    diskService.GetCurrentValue(), 
                    diskService.GetMaxValue());
            }

            prevCheck = DateTime.Now;
            ticksChecked++;
        }

        public void StopBenchmark()
        {
            if (_timer == null || !_timer.IsEnabled || ticksChecked <= minBenchmarkTime)
                return;

            _timer.Stop();

            int energy = (int)(((100 - cpuService.average) * 1.5 +
                            (100 - memoryService.average) * 0.75 +
                            (100 - diskService.average) * 0.25) *
                            12 * new Random().NextDouble() * 0.5 + 0.75);

            var benchmark = new Benchmark()
            {
                Date = DateTime.Now,
                Process = appName,
                CPU = Math.Round(cpuService.average, 2),
                RAM = Math.Round(memoryService.average, 2),
                Energy = energy,
                Disk = Math.Round(diskService.average, 2)
            };

            if (UserInfo.Settings.agreedToDataSharing)
            {
                benchmarkRepository.InsertBenchmark(benchmark);
            }

            HistoryService.AddBenchmark(benchmark);
        }
    }
}
