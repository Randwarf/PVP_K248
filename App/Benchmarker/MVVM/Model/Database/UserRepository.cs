using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.Model.Database
{
    internal class UserRepository : IUserRepository
    {
        private const string BASE_URL = "http://localhost:5000/";
        private const string CREATE_ENDPOINT = "create-user";
        private const string GET_ALL_ENDPOINT = "get-users";
        private const string GET_ENDPOINT = "get-user";

        private HttpClient client;

        public UserRepository()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(GET_ALL_ENDPOINT);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(responseJson);
                    return users;
                }

                throw new HttpRequestException($"Error code: {response.StatusCode}. Message: {response.ReasonPhrase}");
            }
            catch
            {
                Console.WriteLine("[API] API is down");
            }

            return new List<User>();
        }

        public async Task<User> GetUserById(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{GET_ENDPOINT}?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                User user = JsonConvert.DeserializeObject<User>(responseJson);
                return user;
            }

            throw new HttpRequestException($"Error code: {response.StatusCode}. Message: {response.ReasonPhrase}");
        }

        public async void InsertUser(User user)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, CREATE_ENDPOINT);
            request.Headers.Add("Accept", "application/json");
            string benchmarkJson = JsonConvert.SerializeObject(user);
            request.Content = new StringContent(benchmarkJson, Encoding.UTF8, "application/json");

            try
            {
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

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
