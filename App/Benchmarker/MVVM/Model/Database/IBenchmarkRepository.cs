using Benchmarker.MVVM.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benchmarker.Database
{
    internal interface IBenchmarkRepository
    {
        Task<List<Benchmark>> GetAllBenchmarks();
        Task<Benchmark> GetBenchmarkById(int id);
        void InsertBenchmark(Benchmark benchmark);
        void UpdateBenchmark(Benchmark benchmark);
        void DeleteBenchmark(int id);
    }
}
