using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Payments;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Project
{
    public interface IGetProjectPaymentsService
    {
        public Task<ProjectPayments> Execute(string projectId);
    }

    public class GetProjectPaymentsService : IGetProjectPaymentsService
    {
        private readonly MfspApiClient _apiClient;

        public GetProjectPaymentsService(MfspApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ProjectPayments> Execute(string projectId)
        {
            var endpoint = $"/api/v1/client/projects/{projectId}/payments";

            var result = await _apiClient.Get<ApiSingleResponseV2<ProjectPayments>>(endpoint);

            return result.Data;
        }
    }
}