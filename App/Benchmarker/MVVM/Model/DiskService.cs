using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Benchmarker.MVVM.Model
{
    internal class DiskService
    {
        private List<PerformanceCounter> performanceCounters;
        private readonly Type monitorType;

        private readonly Process process;
        private List<Process> processes;

        private enum Type
        {
            Single,
            List
        }
        public DiskService(Process process)
        {
            performanceCounters= new List<PerformanceCounter>();
            var performanceCounter = new PerformanceCounter("Process", "IO Data Bytes/sec", process.ProcessName);
            performanceCounters.Add(performanceCounter);
            this.process = process;
            monitorType = Type.Single;
        }

        public DiskService(List<Process> processes)
        {
            performanceCounters= new List<PerformanceCounter>();
            foreach (var process in processes)
            {
                var performanceCounter = new PerformanceCounter("Process", "IO Data Bytes/sec", process.ProcessName);
                performanceCounters.Add(performanceCounter);
            }
            this.processes = processes;
            monitorType = Type.List;
        }

        public double GetRawValue()
        {
            if (monitorType == Type.Single)
            {
                return GetRawValueSingle();
            }
            else
            {
                return GetRawValueList();
            }
        }

        private double GetRawValueSingle()
        {
            process.Refresh();
            performanceCounters[0].NextValue();
            double diskUsage = performanceCounters[0].NextValue() / 1024.0 / 1024.0;
            return Math.Round(diskUsage, 2);
        }

        private double GetRawValueList()
        {
            CheckForFinishedProcesses();

            double sum = 0;

            for (int i = 0; i < processes.Count; i++)
            {
                processes[i].Refresh();
                double diskUsage = performanceCounters[i].NextValue() / 1024.0 / 1024.0;
                sum += Math.Round(diskUsage, 2);
            }

            return sum;
        }

        private void CheckForFinishedProcesses()
        {
            processes = processes.Where(x => x.HasExited == false).ToList();
        }
    }
}
