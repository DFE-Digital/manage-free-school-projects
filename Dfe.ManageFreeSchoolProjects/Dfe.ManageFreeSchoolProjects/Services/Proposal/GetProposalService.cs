using Dfe.ManageFreeSchoolProjects.API.Contracts.Dashboard;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Proposal
{
    public interface IGetProposalService
    {
        public Task<List<GetProposalResponse>> ExecuteList(string projectId);
    }

    public record GetProposalServicListParameters
    {
        public string ProjectId { get; init; }
    }

    public class GetProposalService(MfspApiClient apiClient) : IGetProposalService
    {
        public async Task<List<GetProposalResponse>> ExecuteList(string projectId)
        {
            var endpoint = $"/api/v1/client/proposals/list?projectId={projectId}";

            var result = await apiClient.Get<List<GetProposalResponse>>(endpoint);

            return result;
        }
    }
}
