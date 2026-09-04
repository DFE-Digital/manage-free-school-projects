using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals
{
    public interface IGetProposalService
    {
        public Task<List<GetProposalSummaryResponse>> ExecuteList(string projectId);

        public Task<ProposalResponse> ExecuteSingle(string rid);
    }

    public class GetProposalService : IGetProposalService
    {
        private readonly MfspContext _context;

        public GetProposalService(MfspContext context)
        {
            _context = context;
        }

        public async Task<List<GetProposalSummaryResponse>> ExecuteList(string projectId)
        {
            return await _context.Proposals.Where(x => x.ProjectId == projectId).Select(x => new GetProposalSummaryResponse
            {
                Rid = x.Rid,
                ProjectId = x.ProjectId,
                Proposer = ProjectMapper.ToProposer(x.Proposer),
                Name = GetName(x),
                ProposedFaithStatus = !string.IsNullOrWhiteSpace(x.ProposedFaithStatus)
                    ? ProjectMapper.ToFaithStatus(x.ProposedFaithStatus)
                    : FaithStatus.NotSet,
                ProposedFaithType = !string.IsNullOrWhiteSpace(x.ProposedFaithType) ? ProjectMapper.ToFaithType(x.ProposedFaithType) : null,
                Status = ProposalStatus.Active
            }).ToListAsync();
        }

        public async Task<ProposalResponse> ExecuteSingle(string rid)
        {
            var entity = await _context.Proposals.FirstOrDefaultAsync(x => x.Rid == rid);
            
            if (entity == null)
                return null;
            
            return ProposalMapper.ToProposalResponse(entity);
        }

        private static string GetName(Proposal proposal)
        {
            if (proposal.Proposer == ProjectMapper.ToProposer(ProposalProposer.AcademyTrust))
            {
                return proposal.TrustName;
            }
            else if (proposal.Proposer == ProjectMapper.ToProposer(ProposalProposer.Diocese))
            {
                return proposal.NameOfDiocese;
            }
            else if (proposal.Proposer == ProjectMapper.ToProposer(ProposalProposer.AnotherReligiousOrganisation))
            {
                return proposal.NameOfOtherReligiousOrganisation;
            }
            else if (proposal.Proposer == ProjectMapper.ToProposer(ProposalProposer.LocalAuthorityThatPushedSpecification))
            {
                return ""; // to check
            }
            else if (proposal.Proposer == ProjectMapper.ToProposer(ProposalProposer.AnotherLocalAuthority))
            {
                return proposal.OtherLocalAuthority;
            }
            else if (proposal.Proposer == ProjectMapper.ToProposer(ProposalProposer.JointProposal))
            {
                return proposal.JointProposalLocalAuthority;
            }

            return string.Empty;
        }
    }
}
