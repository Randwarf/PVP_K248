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
        public RelayCommand SwitchView { get; set; }
        public string appName { get; set; }
        public string instanceName { get; set; }
        private string _currentCPU { get; set; }
        private string _currentMemory { get; set; }

        private KeyValuePair<Process, List<Process>> _process;

        private DispatcherTimer _timer;
        private DateTime prevCheck;
        // TODO: REWORK HOW AVG VALUES ARE CALCULATED!
        private int ticksChecked = 0;

        private Queue<double> _historyCPU;
        private Queue<double> _historyMemory;

        private CPUService cpuService;
        private MemoryService memoryService;

        private readonly IBenchmarkRepository benchmarkRepository;

        private RelayCommand switchView;

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

                _timer = new DispatcherTimer();
                _timer.Tick += new EventHandler(dispatcherTimer_Tick);
                _timer.Interval = new TimeSpan(0, 0, 1);
                _timer.Start();

                _historyCPU = new Queue<double>();
                _historyMemory = new Queue<double>();
                for (int i = 0; i < 280; i++)
                {
                    _historyCPU.Enqueue(0);
                    _historyMemory.Enqueue(0);
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

        public string historyCPU
        {
            get
            {
                var builder = new StringBuilder();
                for (int i = 0; i < _historyCPU.Count; i++)
                {
                    double yPos = (100 - _historyCPU.ElementAt(i)) * 1.3;
                    string yPosWithDot = yPos.ToString().Replace(",", ".");
                    builder.Append(i + "," + yPosWithDot + " ");
                }
                return builder.ToString();
            }
            set
            {
                _historyCPU.Dequeue();
                _historyCPU.Enqueue(float.Parse(value));
                OnPropertyChanged();
            }
        }

        public string historyMemory
        {
            get
            {
                var builder = new StringBuilder();
                for (int i = 0; i < _historyMemory.Count; i++)
                {
                    double yPos = (100 - _historyMemory.ElementAt(i)) * 1.3;
                    string yPosWithDot = yPos.ToString().Replace(",", ".");
                    builder.Append(i + "," + yPosWithDot + " ");
                }
                return builder.ToString();
            }
            set
            {
                _historyMemory.Dequeue();
                _historyMemory.Enqueue(float.Parse(value));
                OnPropertyChanged();
            }
        }

        public BenchmarkRunViewModel(RelayCommand switchView)
        {
            this.switchView = switchView;
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

            _historyCPU = new Queue<double>();
            _historyMemory = new Queue<double>();
            for (int i = 0; i < 280; i++)
            {
                _historyCPU.Enqueue(0);
                _historyMemory.Enqueue(0);
            }
        }

        private double CalculateAvg(Queue<double> q)
        {
            return q.Skip(280 - ticksChecked)
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
                currentCPU = string.Format("CPU: {0}%", cpuPercentage);
                historyCPU = cpuPercentage.ToString();

                double memoryPercentage = memoryService.GetPercentage();
                double memoryRawValue = memoryService.GetRawValue();
                currentMemory = string.Format("RAM: {0}% - {1:0.00}Mb", memoryPercentage, memoryRawValue / 1024);
                historyMemory = memoryPercentage.ToString();
            }

            prevCheck = DateTime.Now;
            ticksChecked++;
        }

        private void StopBenchmark()
        {
            _timer.Stop();
            double avgCPUPercent = CalculateAvg(_historyCPU);
            double avgMemoryPercent = CalculateAvg(_historyMemory);

            var benchmark = new Benchmark()
            {
                CPU = Math.Round(avgCPUPercent, 2),
                RAM = Math.Round(avgMemoryPercent, 2),
                Energy = -1,
                Disk = -1,
                Process = appName
            };

            if (new UserInfo().Settings.agreedToDataSharing) 
            {
                benchmarkRepository.InsertBenchmark(benchmark);
            }
            
            History.SaveBenchmark(benchmark);
            switchView.Execute(this);
        }
    }
}
