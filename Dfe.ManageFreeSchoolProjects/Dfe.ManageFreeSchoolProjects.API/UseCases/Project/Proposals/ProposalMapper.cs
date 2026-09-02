using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals
{
    public static class ProposalMapper
    {
        public static ProposalResponse ToProposalResponse(Proposal proposal)
        {
            return new ProposalResponse
            {
                Rid = proposal.Rid,
                ProjectId = proposal.ProjectId,
                Proposer = ProjectMapper.ToProposer(proposal.Proposer),
                TrustReferenceNumber = proposal.TrustReferenceNumber,
                TrustName = proposal.TrustName,
                TrustType = proposal.TrustType != null ? ProjectMapper.ToTrustType(proposal.TrustType) : null,
                NameOfDiocese = proposal.NameOfDiocese,
                FaithOfDiocese = proposal.FaithOfDiocese != null ? ProjectMapper.ToDioceseFaithType(proposal.FaithOfDiocese) : null,
                NameOfOtherReligiousOrganisation = proposal.NameOfOtherReligiousOrganisation,
                FaithTypeOfOtherReligiousOrganisation = proposal.FaithTypeOfOtherReligiousOrganisation != null ? ProjectMapper.ToFaithType(proposal.FaithTypeOfOtherReligiousOrganisation) : null,
                OtherFaithTypeOfOtherReligiousOrganisation = proposal.OtherFaithTypeOfOtherReligiousOrganisation,
                OtherLocalAuthority = proposal.OtherLocalAuthority,
                JointProposalLocalAuthority = proposal.JointProposalLocalAuthority,
                ProposedFaithStatus = ProjectMapper.ToFaithStatus(proposal.ProposedFaithStatus),
                ProposedFaithType = ProjectMapper.ToFaithType(proposal.ProposedFaithType),
                OtherFaithType = proposal.OtherFaithType
            };
        }
    }
}
