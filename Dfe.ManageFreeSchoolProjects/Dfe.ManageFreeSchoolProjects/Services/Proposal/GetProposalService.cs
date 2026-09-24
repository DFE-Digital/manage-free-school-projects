using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Proposal
{
    public interface IGetProposalService
    {
        public Task<ApiSingleResponseV2<List<GetProposalSummaryResponse>>> ExecuteList(string projectId);

        public Task<ApiSingleResponseV2<ProposalResponse>> ExecuteSingle(string rid);
    }

    public record GetProposalServicListParameters
    {
        public string ProjectId { get; init; }
    }

    public class GetProposalService(MfspApiClient apiClient) : IGetProposalService
    {
        public async Task<ApiSingleResponseV2<List<GetProposalSummaryResponse>>> ExecuteList(string projectId)
        {
            var endpoint = $"/api/v1/client/proposals/list?projectId={projectId}";

            var result = await apiClient.Get<ApiSingleResponseV2<List<GetProposalSummaryResponse>>>(endpoint);

            return result;
        }

        public async Task<ApiSingleResponseV2<ProposalResponse>> ExecuteSingle(string rid)
        {
            var endpoint = $"/api/v1/client/proposals/{rid}";

            var result = await apiClient.Get<ApiSingleResponseV2<ProposalResponse>>(endpoint);

            return result;
        }
    }
}
