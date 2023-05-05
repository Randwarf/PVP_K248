using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.Model.Database
{
	public static class APIStatus
	{
		public static async Task<bool> IsAPIWorking()
		{
			string apiUrl = "http://127.0.0.1:5000/api_status";

			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = await client.GetAsync(apiUrl);
					response.EnsureSuccessStatusCode();

					string result = await response.Content.ReadAsStringAsync();
					Console.WriteLine(result);
					return true;
				}
				catch (HttpRequestException)
				{
					Console.WriteLine("Failed to connect to the API.");
					return false;
				}
			}

		}
	}
}
