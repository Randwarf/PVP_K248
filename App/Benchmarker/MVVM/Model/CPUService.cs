using System;
using System.Diagnostics;

namespace Benchmarker.MVVM.Model
{
    internal class CPUService
    {
        private readonly Process process;

        private TimeSpan prevTotalCPUTime;
        private DateTime prevCheck;

        public CPUService(Process process) {
            this.process = process;

            prevTotalCPUTime = process.TotalProcessorTime;
            prevCheck = process.StartTime;
        }

        public double GetPercentage()
        {
            var newTotalCPUTime = process.TotalProcessorTime;
            TimeSpan elapsed = DateTime.Now - prevCheck;
            
            TimeSpan timeThisCheck = newTotalCPUTime - prevTotalCPUTime;
            double cpuUsage = (double)timeThisCheck.Ticks / elapsed.Ticks;
            double cpuPercentage = cpuUsage * 100;

            prevCheck = DateTime.Now;
            prevTotalCPUTime = newTotalCPUTime;

            return Math.Round(cpuPercentage, 2);
        }
    }
}
