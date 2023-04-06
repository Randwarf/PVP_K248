using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualBasic.Devices;

namespace Benchmarker.MVVM.Model
{
    internal class MemoryService : GraphableService
    {
        private readonly Process process;
        private List<Process> processes;
        private readonly Type monitorType;

        private double memoryInDevice;

        public MemoryService(Process process) : base(280)
        {
            this.process = process;
            monitorType = Type.Single;
            Initialize();
        }

        public MemoryService(List<Process> processes) : base(280)
        {
            this.processes = processes;
            monitorType = Type.List;
            Initialize();
        }

        private void Initialize()
        {
            memoryInDevice = new ComputerInfo().TotalPhysicalMemory;// / Math.Pow(1024, 3);
        }

        protected override double GetRawNext()
        {
            return GetPercentage();
        }

        public double GetPercentage()
        {
            if (monitorType == Type.Single)
            {
                return GetPercentageSingle();
            } else
            {
                return GetPercentageList();
            }
        }

        private double GetPercentageSingle()
        {
            process.Refresh();
            long memoryUsage = process.WorkingSet64;
            double memoryPercent = 100 * memoryUsage / memoryInDevice;
            return Math.Round(memoryPercent, 2);
        }

        private double GetPercentageList()
        {
            CheckProcesses();

            double sum = 0;

            foreach (Process process in processes)
            {
                process.Refresh();
                long memoryUsage = process.WorkingSet64;
                double memoryPercent = 100 * memoryUsage / memoryInDevice;
                sum += Math.Round(memoryPercent, 2);
            }

            return sum;
        }

        public double GetRawValue()
        {
            if (monitorType == Type.Single)
            {
                return GetRawValueSingle();
            } else
            {
                return GetRawValueList();
            }
        }

        private double GetRawValueSingle()
        {
            process.Refresh();
            long memoryUsage = process.WorkingSet64;
            double memoryUsageKb = memoryUsage / 1024.0;
            return memoryUsageKb;
        }

        private double GetRawValueList()
        {
            CheckProcesses();

            double sum = 0;

            foreach (Process process in processes)
            {
                process.Refresh();
                long memoryUsage = process.WorkingSet64;
                double memoryUsageKb = memoryUsage / 1024.0;
                sum += memoryUsageKb;
            }

            return sum;
        }

        private void CheckProcesses()
        {
            processes = processes.Where(x => x.HasExited == false).ToList();
        }

        private enum Type
        {
            Single,
            List
        }
    }
}
