using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.CompiledModels;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.StartupConfiguration;

public static class DatabaseConfigurationExtensions
{
	public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		services.AddHttpContextAccessor();

		services.AddSingleton<AuditInterceptor>();

		services.AddDbContextPool<MfspContext>((serviceProvider, options) =>
		{
			options.UseMfspSqlServer(connectionString);
			options.UseModel(MfspContextModel.Instance);
			options.AddInterceptors(serviceProvider.GetRequiredService<AuditInterceptor>());
		});

		services.AddHostedService<DatabaseWarmupHostedService>();

		AddDbHealthCheck(services);

		return services;
	}

	public static void AddDbHealthCheck(IServiceCollection services) {
		services.AddHealthChecks()
			.AddDbContextCheck<MfspContext>("Manage School Projects Database");
	}
}
