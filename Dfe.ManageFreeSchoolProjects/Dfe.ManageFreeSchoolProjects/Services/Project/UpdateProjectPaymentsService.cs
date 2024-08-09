using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Payments;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Project
{
    public interface IUpdateProjectPaymentsService
    {
        public Task Execute(string projectId, Payment payment);
    }

    public class UpdateProjectPaymentsService : IUpdateProjectPaymentsService
    {
        private readonly MfspApiClient _apiClient;

        public UpdateProjectPaymentsService(MfspApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task Execute(string projectId, Payment payment)
        {
            var endpoint = $"/api/v1/client/projects/{projectId}/payments";

            await _apiClient.Patch<Payment, ApiSingleResponseV2<object>>(endpoint, payment);
        }
    }
}
