using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;
using Benchmarker.MVVM.Model;

internal class CPUService : GraphableService
{
    private List<Process> processes;

    private List<TimeSpan> previousCPUTimes;
    private DateTime previousCheckTime;

    public CPUService(List<Process> processes) : base(280)
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

    protected override double GetRawNext()
    {
        return GetPercentage();
    }

    private double GetPercentage()
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

        // Clamp percentage to 100
        percentageSum = Math.Min(percentageSum, 100);

        return Math.Round(percentageSum, 2);
    }
}
