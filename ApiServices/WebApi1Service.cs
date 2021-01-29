using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiServices
{
	public interface IWebApi1Service
	{
		Task<string> CallPublicEndPoint();
		Task<ActionResult<string>> CallProtectedEndPoint();
	}

	public class WebApi1Service : IWebApi1Service
	{
		private readonly IConfiguration configuration;
		private readonly HttpClient httpClient;
		private readonly ITokenAcquisition tokenAcquisition;

		public WebApi1Service(IConfiguration configuration, HttpClient httpClient, ITokenAcquisition tokenAcquisition)
		{
			this.configuration = configuration;
			this.httpClient = httpClient;
			this.tokenAcquisition = tokenAcquisition;

			httpClient.BaseAddress = new Uri(configuration["AzureAdB2C:WebApi1BaseUrl"]);
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<ActionResult<string>> CallProtectedEndPoint()
		{
			var authResult = await tokenAcquisition.GetAuthenticationResultForUserAsync(new string[] {
				configuration["AzureAdB2C:WebApi1Scope"]
			});

			var idToken = authResult.IdToken; //just a check to see we have an id token
			var accessToken = authResult.AccessToken; //access token is null

			//also returns a null access token
			var accessToken2 = await tokenAcquisition.GetAccessTokenForUserAsync(new string[] {
				configuration["AzureAdB2C:WebApi1Scope"]
			});

			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			var response = await httpClient.GetAsync("Test/GetProtected");

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadAsStringAsync();
			}
			else
			{
				return response.StatusCode.ToString();
			}
		}

		public async Task<string> CallPublicEndPoint()
		{
			return await httpClient.GetStringAsync("Test/GetPublic");
		}
	}
}
