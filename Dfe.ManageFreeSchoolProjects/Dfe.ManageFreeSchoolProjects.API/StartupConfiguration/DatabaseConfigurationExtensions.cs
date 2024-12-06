using Dfe.ManageFreeSchoolProjects.Data;

namespace Dfe.ManageFreeSchoolProjects.API.StartupConfiguration;

public static class DatabaseConfigurationExtensions
{
	public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		services.AddDbContext<MfspContext>(options =>
			options.UseMfspSqlServer(connectionString)
		);

		services.AddScoped<AuditInterceptor, AuditInterceptor>();

		AddDbHealthCheck(services);

		return services;
	}

	public static void AddDbHealthCheck(IServiceCollection services) {
		services.AddHealthChecks()
			.AddDbContextCheck<MfspContext>("Manage Free School Projects Database");
	}
}
