using Dfe.ManageFreeSchoolProjects.UserContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dfe.ManageFreeSchoolProjects.Data.Tests;

public class DatabaseTestFixture
{
	private readonly string? _connectionString;

	private static readonly object _lock = new();
	private static bool _databaseInitialized;

	protected DatabaseTestFixture()
	{

		var configPath = Path.Combine(
		Directory.GetCurrentDirectory(), "appsettings.tests.json");

		var config = new ConfigurationBuilder()
			.AddJsonFile(configPath)
			.AddEnvironmentVariables();

		_connectionString = BuildDatabaseConnectionString(config);

		lock (_lock)
		{
			if (!_databaseInitialized)
			{
				using (var context = CreateContext())
				{
					context.Database.EnsureDeleted();
					context.Database.Migrate();
				}

				_databaseInitialized = true;
			}
		}
	}

	private static string BuildDatabaseConnectionString(IConfigurationBuilder configBuilder)
	{
		var currentConfig = configBuilder.Build();

		var connection = currentConfig.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string not found");

		var sqlBuilder = new SqlConnectionStringBuilder(connection);
		sqlBuilder.InitialCatalog = "DataTests";

		var result = sqlBuilder.ToString();

		return result;
	}

	protected ProjectsDbContext CreateContext()
	{
		var userInfoService = new ServerUserInfoService()
		{
			UserInfo = new UserInfo() { Name = "Data.Tests@test.gov.uk", Roles = new[] { Claims.CaseWorkerRoleClaim } }
		};

		return new ProjectsDbContext(new DbContextOptionsBuilder<ProjectsDbContext>().UseSqlServer(_connectionString).Options, userInfoService);
	}
}