using System.Collections.Generic;
using System.Diagnostics;
using System;

internal class ProcessUsage
{
    private readonly List<Process> processes;

    private Dictionary<int, TimeSpan> prevTotalCPUTimes;
    private Dictionary<int, DateTime> prevChecks;

    public ProcessUsage(List<Process> processes)
    {
        this.processes = processes;

        prevTotalCPUTimes = new Dictionary<int, TimeSpan>();
        prevChecks = new Dictionary<int, DateTime>();

        foreach (Process process in processes)
        {
            if (process.HasExited || process == null)
            {
                continue;
            }

            prevTotalCPUTimes[process.Id] = new TimeSpan(0);
            prevChecks[process.Id] = process.StartTime;
        }
    }

    public Dictionary<int, double> GetPercentages()
    {
        var percentages = new Dictionary<int, double>();

        foreach (Process process in processes)
        {
            if (process.HasExited || process == null)
            {
                continue;
            }

            var newTotalCPUTime = process.TotalProcessorTime;
            TimeSpan elapsed = DateTime.Now - prevChecks[process.Id];

            TimeSpan timeThisCheck = newTotalCPUTime - prevTotalCPUTimes[process.Id];
            double cpuUsage = (double)timeThisCheck.Ticks / elapsed.Ticks;
            double cpuPercentage = cpuUsage * 100;

            prevChecks[process.Id] = DateTime.Now;
            prevTotalCPUTimes[process.Id] = newTotalCPUTime;

            percentages[process.Id] = Math.Round(cpuPercentage, 2);
        }

        return percentages;
    }
}
