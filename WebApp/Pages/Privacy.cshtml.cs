using ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace WebApp.Pages
{
	[Authorize]
	public class PrivacyModel : PageModel
	{
		private readonly IWebApi1Service webApi1Service;
		private readonly IWebApi2Service webApi2Service;

		public PrivacyModel(IWebApi1Service webApi1Service, IWebApi2Service webApi2Service)
		{
			this.webApi1Service = webApi1Service;
			this.webApi2Service = webApi2Service;
		}

		public string ApiResponse { get; set; }

		public void OnGet()
		{
		}

		public async Task<ActionResult> OnGetWebApi1PublicAsync()
		{
			ApiResponse = await webApi1Service.CallPublicEndPoint();

			return Page();
		}

		public async Task<ActionResult> OnGetWebApi1ProtectedAsync()
		{
			var response = await webApi1Service.CallProtectedEndPoint();

			ApiResponse = response.Value;

			return Page();
		}

		public async Task<ActionResult> OnGetWebApi2PublicAsync()
		{
			ApiResponse = await webApi2Service.CallPublicEndPoint();

			return Page();
		}

		public async Task<ActionResult> OnGetWebApi2ProtectedAsync()
		{
			var response = await webApi2Service.CallProtectedEndPoint();

			ApiResponse = response.Value;

			return Page();
		}
	}
}
