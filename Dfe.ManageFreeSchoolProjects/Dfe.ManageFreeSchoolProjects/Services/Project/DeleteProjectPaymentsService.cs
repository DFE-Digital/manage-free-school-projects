using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Payments;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Project
{
    public interface IDeleteProjectPaymentsService
    {
        public Task Execute(string projectId, int paymentIndex);
    }

    public class DeleteProjectPaymentsService : IDeleteProjectPaymentsService
    {
        private readonly MfspApiClient _apiClient;

        public DeleteProjectPaymentsService(MfspApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task Execute(string projectId, int paymentIndex)
        {
            var endpoint = $"/api/v1/client/projects/{projectId}/payments/delete/{paymentIndex}";

            await _apiClient.Patch<int, ApiSingleResponseV2<object>>(endpoint, paymentIndex);
        }
    }
}
