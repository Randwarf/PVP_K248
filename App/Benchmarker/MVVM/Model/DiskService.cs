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
        private PerformanceCounter performanceCounter;
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
            this.process = process;
            monitorType = Type.Single;
        }

        public DiskService(List<Process> processes)
        {
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
            performanceCounter = new PerformanceCounter("Process", "IO Data Bytes/sec", process.ProcessName);
            process.Refresh();
            performanceCounter.NextValue();
            Thread.Sleep(1000);
            double diskUsage = performanceCounter.NextValue() / 1024.0 / 1024.0;
            return Math.Round(diskUsage, 2);
        }

        private double GetRawValueList()
        {
            CheckProcesses();

            double sum = 0;

            foreach (Process process in processes)
            {
                performanceCounter = new PerformanceCounter("Process", "IO Data Bytes/sec", process.ProcessName );
                process.Refresh();
                performanceCounter.NextValue();
                Thread.Sleep(1000);
                double diskUsage = performanceCounter.NextValue() / 1024.0 / 1024.0;
                sum += Math.Round(diskUsage, 2);
            }

            return sum;
        }

        private void CheckProcesses()
        {
            processes = processes.Where(x => x.HasExited == false).ToList();
        }
    }
}
