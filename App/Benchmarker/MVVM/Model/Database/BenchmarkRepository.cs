using Benchmarker.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Benchmarker.Database
{
    internal class BenchmarkRepository : IBenchmarkRepository
    {
        private const string BASE_URL = "http://localhost:5000/";
        private const string SAVE_ENDPOINT = "save-benchmark";
        private const string GET_ENDPOINT = "get-benchmark";

        private HttpClient client;

        public BenchmarkRepository()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Benchmark>> GetAllBenchmarks()
        {
            HttpResponseMessage response = await client.GetAsync(GET_ENDPOINT);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                List<Benchmark> benchmarks = JsonConvert.DeserializeObject<List<Benchmark>>(responseJson);
                return benchmarks;
            }

            throw new HttpRequestException($"Error code: {response.StatusCode}. Message: {response.ReasonPhrase}");
        }

        public async Task<Benchmark> GetBenchmarkByID(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{GET_ENDPOINT}?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                Benchmark benchmark = JsonConvert.DeserializeObject<Benchmark>(responseJson);
                return benchmark;
            }

            throw new HttpRequestException($"Error code: {response.StatusCode}. Message: {response.ReasonPhrase}");
        }

        public async void InsertBenchmark(Benchmark benchmark)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, SAVE_ENDPOINT);
            request.Headers.Add("Accept", "application/json");
            string benchmarkJson = JsonConvert.SerializeObject(benchmark);
            request.Content = new StringContent(benchmarkJson, Encoding.UTF8, "application/json");

            try {
                HttpResponseMessage response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error code: {response.StatusCode}. Message: {response.ReasonPhrase}.");
                }
            }
            catch
            {
                Debug.WriteLine("[API] Error with API");
            }
        }

        public void UpdateBenchmark(Benchmark benchmark)
        {
            throw new NotImplementedException();
        }

        public void DeleteBenchmark(int id)
        {
            throw new NotImplementedException();
        }
    }
}
