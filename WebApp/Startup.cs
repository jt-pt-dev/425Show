using ApiServices;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace WebApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
				.AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAdB2C"))
				.EnableTokenAcquisitionToCallDownstreamApi(
					//I was trying to supply a string[] of scopes here for WebApi1 and WebApi2
					//but doing so triggered an exception AADB2C90146. Searching for this
					//exception leads to:
					//https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/issues/550
					//which suggests only one scope can be called at a time. This is
					//why I have built the ApiServices project and have different
					//services for each Api
				)
				.AddInMemoryTokenCaches();

			services.AddHttpClient<IWebApi1Service, WebApi1Service>();
			services.AddHttpClient<IWebApi2Service, WebApi2Service>();

			services.AddRazorPages()
				.AddMicrosoftIdentityUI();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
			});
		}
	}
}
