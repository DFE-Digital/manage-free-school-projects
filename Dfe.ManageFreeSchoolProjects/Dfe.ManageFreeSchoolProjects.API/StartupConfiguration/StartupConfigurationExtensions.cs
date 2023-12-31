namespace Dfe.ManageFreeSchoolProjects.API.StartupConfiguration;

public static class StartupConfigurationExtensions
{
	public static IServiceCollection AddConcernsApiProject(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddApiDependencies();
		services.AddDatabase(configuration);
		services.AddApi(configuration);
		services.AddUseCases();
		//services.AddUseCaseAsyncs(); //TODO: need to update decisions iuseasync to use the new DecisionUseCaseRequestWrapper
		
		return services;
	}
}