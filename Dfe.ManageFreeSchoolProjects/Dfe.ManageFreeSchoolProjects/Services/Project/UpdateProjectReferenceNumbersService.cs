using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ReferenceNumbers;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Project
{
    public interface IUpdateProjectReferenceNumbersService
    {
        public Task Execute(string projectId, UpdateProjectReferenceNumbersRequest request);
    }

    public class UpdateProjectReferenceNumbersService : IUpdateProjectReferenceNumbersService
    {
        private readonly MfspApiClient _apiClient;

        public UpdateProjectReferenceNumbersService(MfspApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task Execute(string projectId, UpdateProjectReferenceNumbersRequest request)
        {
            var endpoint = $"/api/v1/client/projects/{projectId}/referencenumbers";

            await _apiClient.Patch<UpdateProjectReferenceNumbersRequest, ApiSingleResponseV2<object>>(endpoint, request);
        }
    }
}
