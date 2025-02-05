using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Project
{
    public interface IGetProjectSitesPresumptionService
    {
        public Task<GetProjectSitesPresumptionResponse> Execute(string projectId);
    }

    public class GetProjectSitesPresumptionService : IGetProjectSitesPresumptionService
    {
        private readonly MfspApiClient _apiClient;

        public GetProjectSitesPresumptionService(MfspApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetProjectSitesPresumptionResponse> Execute(string projectId)
        {
            var endpoint = $"/api/v1/client/projects/{projectId}/sites/presumption";

            var result = await _apiClient.Get<ApiSingleResponseV2<GetProjectSitesPresumptionResponse>>(endpoint);

            return result.Data;
        }
    }
}
