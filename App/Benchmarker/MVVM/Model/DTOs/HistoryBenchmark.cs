﻿using System;

namespace Benchmarker.MVVM.Model.DTOs
{
    public class HistoryBenchmark
    {
        private Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double CPU { get; set; }
        public double RAM { get; set; }
        public double Energy { get; set; }
        public string Process { get; set; }
        public double Disk { get; set; }

        public HistoryBenchmark() { }

        public HistoryBenchmark(Benchmark benchmark)
        {
            Id = Guid.NewGuid();
            Date = benchmark.Date;
            CPU = benchmark.CPU;
            RAM = benchmark.RAM;
            Energy = benchmark.Energy;
            Process = benchmark.Process;
            Disk = benchmark.Disk;
        }

        public Guid GetId()
        {
            return Id;
        }
    }
}
