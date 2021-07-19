using Notebook.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Net.Http;
using Serilog;

namespace Notebook.WebApi
{
    public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCore();
			services.AddInfrastructure(
				_configuration.GetConnectionString("Notebook"), 
				_configuration.GetSection("SmtpSettings").Get<SmtpSettings>()
			);

			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notebook.WebApi", Version = "v1" });
				c.EnableAnnotations();
				c.OrderActionsBy(apiDesc => $"{apiDesc.RelativePath}_{apiDesc.HttpMethod}");
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseSerilogRequestLogging();
			app.UseMiddleware<RequestResponseLoggingMiddleware>();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.Use(async (context, next) =>
			{
				if (context.Request.Method == HttpMethod.Get.Method && (context.Request.Path.Value == "" || context.Request.Path.Value == "/"))
				{
					context.Response.Redirect($"{context.Request.PathBase}/swagger/index.html");
					return;
				}

				await next();
			});

			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Notebook.WebApi v1"));

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
