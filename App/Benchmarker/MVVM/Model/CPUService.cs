using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;

internal class CPUService
{
    private List<Process> processes;

    private List<TimeSpan> previousCPUTimes;
    private DateTime previousCheckTime;

    public CPUService(List<Process> processes)
    {
        this.processes = processes;
        
        Initialize();
    }

    private void Initialize()
    {
        previousCheckTime = DateTime.Now;

        previousCPUTimes= new List<TimeSpan>();
        foreach (Process process in processes)
        {
            previousCPUTimes.Add(process.TotalProcessorTime);
        }
    }

    public double GetPercentage()
    {
        processes = processes.Where(x => x.HasExited == false).ToList();

        double percentageSum = 0;
        TimeSpan deltaTime = DateTime.Now - previousCheckTime;

        for (int i = 0; i < processes.Count; i++)
        {
            var newCPUTime = processes[i].TotalProcessorTime;
            TimeSpan deltaCPUTime = newCPUTime - previousCPUTimes[i];

            double cpuUsage = (double)deltaCPUTime.Ticks / deltaTime.Ticks;
            double cpuPercentage = cpuUsage * 100;

            percentageSum += cpuPercentage;

            previousCPUTimes[i] = newCPUTime;
        }

        percentageSum = Math.Min(percentageSum, 100);
        return Math.Round(percentageSum, 2);
    }
}
