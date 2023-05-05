using System;
using System.Net.Http;
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
					if (response.IsSuccessStatusCode)
					{
						return true;
					}

					return false;
				}
				catch (HttpRequestException)
				{
					Console.WriteLine("[API] Failed to connect to the API.");
					return false;
				}
			}

		}
	}
}
