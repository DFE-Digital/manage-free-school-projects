using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals
{
    public interface IUpdateProposalService
    {
        Task<ProposalResponse> Execute(UpdateProposalRequest updateRequest);
    }

    public class UpdateProposalService : IUpdateProposalService
    {
        private readonly MfspContext _context;

        public UpdateProposalService(MfspContext context)
        {
            _context = context;
        }

        public async Task<ProposalResponse> Execute(UpdateProposalRequest updateRequest)
        {
            var entity = await _context.Proposals.FirstOrDefaultAsync(x => x.Rid == updateRequest.Rid);

            if (entity == null)
            {
                throw new InvalidOperationException($"Cannot find proposal with Rid {updateRequest.Rid}.");
            }

            switch (updateRequest.Proposer)
            {
                case ProposalProposer.AcademyTrust:
                    entity.TrustReferenceNumber = string.IsNullOrEmpty(updateRequest.TrustReferenceNumber) ? entity.TrustReferenceNumber : updateRequest.TrustReferenceNumber;
                    entity.TrustName = string.IsNullOrEmpty(updateRequest.TrustName) ? entity.TrustName : updateRequest.TrustName;
                    entity.TrustType = updateRequest.TrustType != null ? ProjectMapper.ToTrustType(updateRequest.TrustType.Value) : entity.TrustType;
                    break;
                case ProposalProposer.Diocese:
                    entity.NameOfDiocese = string.IsNullOrEmpty(updateRequest.NameOfDiocese) ? entity.NameOfDiocese : updateRequest.NameOfDiocese;
                    entity.FaithOfDiocese = updateRequest.FaithOfDiocese != null ? ProjectMapper.ToDioceseFaithType(updateRequest.FaithOfDiocese.Value) : entity.FaithOfDiocese;
                    break;
                case ProposalProposer.AnotherReligiousOrganisation:
                    entity.NameOfOtherReligiousOrganisation = string.IsNullOrEmpty(updateRequest.NameOfOtherReligiousOrganisation) ? entity.NameOfOtherReligiousOrganisation : updateRequest.NameOfOtherReligiousOrganisation;
                    entity.FaithTypeOfOtherReligiousOrganisation = updateRequest.FaithTypeOfOtherReligiousOrganisation != null ? ProjectMapper.ToFaithType(updateRequest.FaithTypeOfOtherReligiousOrganisation.Value) : entity.FaithTypeOfOtherReligiousOrganisation;
                    entity.OtherFaithTypeOfOtherReligiousOrganisation = string.IsNullOrEmpty(updateRequest.OtherFaithTypeOfOtherReligiousOrganisation) ? entity.OtherFaithTypeOfOtherReligiousOrganisation : updateRequest.OtherFaithTypeOfOtherReligiousOrganisation;
                    break;
                case ProposalProposer.LocalAuthorityThatPushedSpecification:
                    break;
                case ProposalProposer.AnotherLocalAuthority:
                    entity.OtherLocalAuthority = string.IsNullOrEmpty(updateRequest.OtherLocalAuthority) ? entity.OtherLocalAuthority : updateRequest.OtherLocalAuthority;
                    break;
                case ProposalProposer.JointProposal:
                    entity.JointProposalLocalAuthority = string.IsNullOrEmpty(updateRequest.JointProposalLocalAuthority) ? entity.JointProposalLocalAuthority : updateRequest.JointProposalLocalAuthority;
                    break;
                default:
                    throw new InvalidOperationException($"Cannot handle proposer type {updateRequest.Proposer}");
            }

            entity.ProposedFaithStatus = updateRequest.ProposedFaithStatus != null ? ProjectMapper.ToFaithStatus(updateRequest.ProposedFaithStatus.Value) : entity.ProposedFaithStatus;
            entity.ProposedFaithType = updateRequest.ProposedFaithType != null ? ProjectMapper.ToFaithType(updateRequest.ProposedFaithType.Value) : entity.ProposedFaithType;
            entity.OtherFaithType = updateRequest.OtherFaithType ?? entity.OtherFaithType;

            await _context.SaveChangesAsync();

            return ProposalMapper.ToProposalResponse(entity);
        }
    }
}
