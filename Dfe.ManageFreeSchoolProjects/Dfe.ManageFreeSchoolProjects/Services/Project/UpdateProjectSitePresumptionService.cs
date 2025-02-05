using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Project
{
    public interface IUpdateProjectSitePresumptionService
    {
        public Task Execute(string projectId, UpdateProjectSitePresumptionRequest request, ProjectSiteType projectSiteType);
    }

    public class UpdateProjectSitePresumptionService : IUpdateProjectSitePresumptionService
    {
        private readonly MfspApiClient _apiClient;

        public UpdateProjectSitePresumptionService(MfspApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task Execute(string projectId, UpdateProjectSitePresumptionRequest request, ProjectSiteType projectSiteType)
        {
            var endpoint = $"/api/v1/client/projects/{projectId}/sites/presumption/{projectSiteType.ToString().ToLower()}";

            await _apiClient.Patch<UpdateProjectSitePresumptionRequest, ApiSingleResponseV2<object>>(endpoint, request);
        }
    }
}