using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ReferenceNumbers;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Project
{
    public interface IGetProjectReferenceNumbersService
    {
        public Task<GetProjectReferenceNumbersResponse> Execute(string projectId);
    }

    public class GetProjectReferenceNumbersService : IGetProjectReferenceNumbersService
    {
        private readonly MfspApiClient _apiClient;

        public GetProjectReferenceNumbersService(MfspApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetProjectReferenceNumbersResponse> Execute(string projectId)
        {
            var endpoint = $"/api/v1/client/projects/{projectId}/referencenumbers";

            var result = await _apiClient.Get<ApiSingleResponseV2<GetProjectReferenceNumbersResponse>>(endpoint);

            return result.Data;
        }
    }
}
