using System.Net.Mime;

namespace Dfe.ManageFreeSchoolProjects.API.StartupConfiguration;

public static class ApiConfigurationExtensions
{
	public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddHttpClient("DefaultClient");

		services.AddControllers();
		services.AddApiVersioning(config =>
			{
				config.ReportApiVersions = true;
				// Every route is "api/v{version:apiVersion}/...", so only read the version from the URL segment
				config.ApiVersionReader = new UrlSegmentApiVersionReader();
			})
			.AddMvc()
			.AddApiExplorer(setup =>
			{
				// ReSharper disable once StringLiteralTypo
				setup.GroupNameFormat = "'v'VVV";
				setup.SubstituteApiVersionInUrl = true;
			});

		services.AddSwaggerGen();
		services.ConfigureOptions<SwaggerOptions>();

		return services;
	}
	
	public static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
	{
		app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		return app;
	}
}