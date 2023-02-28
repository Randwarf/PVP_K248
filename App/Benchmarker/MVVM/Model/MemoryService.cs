using System;
using System.Diagnostics;

namespace Benchmarker.MVVM.Model
{
    internal class MemoryService
    {
        private PerformanceCounter performanceCounter;
        private float memoryInDevice;

        private Process process;

        public MemoryService(Process process)
        {
            this.process = process;

            string instanceName = GetProcessInstanceNameById(process.Id);
            memoryInDevice = GetMemoryInDevice();
            InitializeMemoryPerformanceCounter(instanceName);
        }

        public double GetPercentage()
        {
            process.Refresh();
            long memoryUsage = performanceCounter.RawValue;
            double memoryPercent = 100 * memoryUsage / memoryInDevice;
            return Math.Round(memoryPercent, 2);
        }

        public double GetRawValue()
        {
            process.Refresh();
            long memoryUsage = performanceCounter.RawValue;
            double memoryUsageKb = memoryUsage / 1024.0;
            return memoryUsageKb;
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
