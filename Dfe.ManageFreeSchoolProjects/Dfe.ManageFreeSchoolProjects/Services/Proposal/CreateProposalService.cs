using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Proposal
{
    public interface ICreateProposalService
    {
        public Task<ProposalResponse> Execute(CreateProposalRequest createRequest);
    }

    public class CreateProposalService : ICreateProposalService
    {
        private readonly MfspApiClient _apiClient;

        public CreateProposalService(MfspApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ProposalResponse> Execute(CreateProposalRequest createRequest)
        {
            var response = await _apiClient.Post<CreateProposalRequest, ProposalResponse>($"/api/v1/client/proposals/create/", createRequest);
            return response;
        }
    }
}
