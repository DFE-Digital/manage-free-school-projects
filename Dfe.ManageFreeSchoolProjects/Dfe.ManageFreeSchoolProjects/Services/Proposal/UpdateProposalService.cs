using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Proposal
{
    public interface IUpdateProposalService
    {
        public Task<ProposalResponse> Execute(UpdateProposalRequest updateRequest);
    }

    public class UpdateProposalService : IUpdateProposalService
    {
        private readonly MfspApiClient _apiClient;

        public UpdateProposalService(MfspApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ProposalResponse> Execute(UpdateProposalRequest updateRequest)
        {
            var response = await _apiClient.Put<UpdateProposalRequest, ProposalResponse>($"/api/v1/client/proposals/update/", updateRequest);
            return response;
        }
    }
}
