using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Project
{
    public interface IGetProjectSitesCentralService
    {
        public Task<GetProjectSitesCentralResponse> Execute(string projectId);
    }

    public class GetProjectSitesService : IGetProjectSitesCentralService
    {
        private readonly MfspApiClient _apiClient;

        public GetProjectSitesService(MfspApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetProjectSitesCentralResponse> Execute(string projectId)
        {
            var endpoint = $"/api/v1/client/projects/{projectId}/sites";

            var result = await _apiClient.Get<ApiSingleResponseV2<GetProjectSitesCentralResponse>>(endpoint);

            return result.Data;
        }
    }
}
