using Benchmarker.Core;
using Microsoft.Win32;
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
        private OpenFileDialog _file { get; set; }

        private Process _process;
        private DispatcherTimer _timer;
        private Queue<double> _historyCPU;
        private DateTime prevCheck;
        private TimeSpan prevTotalCPUTime;

        private Queue<double> _historyMemory;
        private float memoryInDevice;

        private PerformanceCounter performanceCounter;

        public OpenFileDialog File
        {
            get { return _file; }
            set
            {
                _file = value;
                appName = _file.SafeFileName;

                _process = new Process();
                _process.StartInfo.FileName = _file.FileName;
                _process.Start();
                prevCheck = _process.StartTime;
                prevTotalCPUTime = new TimeSpan(0);

                instanceName = GetProcessInstanceNameById(_process.Id);
                memoryInDevice = GetMemoryInDevice();
                InitializeMemoryPerformanceCounter(instanceName);

                _timer = new DispatcherTimer();
                _timer.Tick += new EventHandler(dispatcherTimer_Tick);
                _timer.Interval = new TimeSpan(0, 0, 1);
                _timer.Start();

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
            appName = "INSTANTIATING";
            SwitchView = new RelayCommand(o =>
            {
                if (_process != null && !_process.HasExited)
                {
                    performanceCounter.Close();
                    performanceCounter.Dispose();
                    _process.Kill();
                    _timer.Stop();
                }
                switchView.Execute(this);
            });

            _historyCPU = new Queue<double>();
            _historyMemory= new Queue<double>();
            for (int i = 0; i < 280; i++)
            {
                _historyCPU.Enqueue(0);
                _historyMemory.Enqueue(0);
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var newTotalCPUTime = _process.TotalProcessorTime;
            TimeSpan elapsed = DateTime.Now - prevCheck;
            if (elapsed.TotalSeconds > 0)
            {
                TimeSpan timeThisCheck = (newTotalCPUTime - prevTotalCPUTime);
                double CPUusage = (double)timeThisCheck.Ticks / elapsed.Ticks;
                currentCPU = string.Format("CPU: {0:0.00}%", CPUusage * 100);
                historyCPU = (CPUusage * 100).ToString();

                GetMemoryUsage();
            }
            prevCheck = DateTime.Now;
            prevTotalCPUTime = newTotalCPUTime;
        }

        private void GetMemoryUsage()
        {
            _process.Refresh();
            long memoryUsage = performanceCounter.RawValue;
            double memoryUsageKb = memoryUsage / 1024.0;
            double memoryPercent = 100 * memoryUsage / memoryInDevice;
            currentMemory = string.Format("RAM: {0:0.00}% - {1}Kb", memoryPercent, memoryUsageKb);
            historyMemory = memoryPercent.ToString();
        }

        private string GetProcessInstanceNameById(int id)
        {
            PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");

            string[] instances = cat.GetInstanceNames();
            foreach (string instance in instances)
            {
                using (PerformanceCounter cnt = new PerformanceCounter("Process", "ID Process", instance, true))
                {
                    int val = (int)cnt.RawValue;
                    if (val == id)
                    {
                        return instance;
                    }
                }
            }

            return null;
        }

        private void InitializeMemoryPerformanceCounter(string instanceName)
        {
            performanceCounter = new PerformanceCounter();
            performanceCounter.CategoryName = "Process";
            performanceCounter.CounterName = "Working Set - Private";
            performanceCounter.InstanceName = instanceName;
        }

        private float GetMemoryInDevice()
        {
            PerformanceCounter memoryAvailable;
            memoryAvailable = new PerformanceCounter("Memory", "Available Bytes"); // "Available MBytes" for MB
            return memoryAvailable.NextValue();
        }
    }
}
