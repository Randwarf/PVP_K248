using Benchmarker.MVVM.Model;
using System.Collections.Generic;

namespace Benchmarker.Repositories
{
    internal interface IBenchmarkRepository
    {
        List<Benchmark> GetAllBenchmarks();
        Benchmark GetBenchmarkByID(int id);
        void InsertBenchmark(Benchmark benchmark);
        void UpdateBenchmark(Benchmark benchmark);
        void DeleteBenchmark(int id);
    }
}
