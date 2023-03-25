using Benchmarker.MVVM.Model.DTOs;
using Benchmarker.MVVM.ViewModel;
using System.Collections.Generic;
using System;

namespace Benchmarker.MVVM.Model
{
    public static class BenchmarkCompareService
    {
        public static List<ComparisonRow> CompareBenchmarks(HistoryBenchmark benchmark1, HistoryBenchmark benchmark2)
        {
            var row = new ComparisonRow()
            {
                Attribute = "Name",
                Process1 = benchmark1.Process,
                Process2 = benchmark2.Process
            };

            var row1 = CompareValues("CPU", benchmark1.CPU, benchmark2.CPU);
            var row2 = CompareValues("RAM", benchmark1.RAM, benchmark2.RAM);
            var row3 = CompareValues("Disk", benchmark1.Disk, benchmark2.Disk);
            var row4 = CompareValues("Energy", benchmark1.Energy, benchmark2.Energy);

            var rows = new List<ComparisonRow>() { row, row1, row2, row3, row4 };

            return rows;
        }

        private static ComparisonRow CompareValues(string attributeName, double value1, double value2)
        {
            List<string> comparison = new List<string>() {
                value1.ToString(),
                value2.ToString()
            };

            if (value1 == value2)
            {
                return new ComparisonRow()
                {
                    Attribute = attributeName,
                    Process1 = comparison[0],
                    Process2 = comparison[1]
                };
            }

            double minMetric = Math.Min(value1, value2);
            double maxMetric = Math.Max(value1, value2);
            double percentage = (minMetric != 0) ? Math.Abs(maxMetric - minMetric) / minMetric * 100 : 100;

            int modifyIndex = value1 > value2 ? 0 : 1;
            comparison[modifyIndex] += $" (+{Math.Round((percentage > 0 ? 1 : -1) * percentage, 2)}%)";

            var row = new ComparisonRow()
            {
                Attribute = attributeName,
                Process1 = comparison[0],
                Process2 = comparison[1]
            };

            return row;
        }
    }
}
