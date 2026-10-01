using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Extensions;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
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

            ApplyProposerAnswers(entity, updateRequest);
            ApplyFaithAnswers(entity, updateRequest);

            await _context.SaveChangesAsync();

            return ProposalMapper.ToProposalResponse(entity);
        }
        private static void ApplyProposerAnswers(Proposal entity, UpdateProposalRequest updateRequest)
        {
            switch (updateRequest.Proposer)
            {
                case ProposalProposer.AcademyTrust:
                    ApplyAcademyTrust(entity, updateRequest);
                    break;
                case ProposalProposer.Diocese:
                    ApplyDiocese(entity, updateRequest);
                    break;
                case ProposalProposer.AnotherReligiousOrganisation:
                    ApplyOtherReligiousOrganisation(entity, updateRequest);
                    break;
                case ProposalProposer.LocalAuthorityThatPushedSpecification:
                    // This proposer has no answers of its own, only the shared faith answers.
                    break;
                case ProposalProposer.AnotherLocalAuthority:
                    ApplyOtherLocalAuthority(entity, updateRequest);
                    break;
                case ProposalProposer.JointProposal:
                    ApplyJointProposal(entity, updateRequest);
                    break;
                default:
                    throw new InvalidOperationException($"Cannot handle proposer type {updateRequest.Proposer}");
            }
        }

        private static void ApplyAcademyTrust(Proposal entity, UpdateProposalRequest updateRequest)
        {
            entity.TrustReferenceNumber = Replace(entity.TrustReferenceNumber, updateRequest.TrustReferenceNumber);
            entity.TrustName = Replace(entity.TrustName, updateRequest.TrustName);
            entity.TrustType = updateRequest.TrustType != null
                ? ProjectMapper.ToTrustType(updateRequest.TrustType.Value)
                : entity.TrustType;
        }

        private static void ApplyDiocese(Proposal entity, UpdateProposalRequest updateRequest)
        {
            entity.NameOfDiocese = Replace(entity.NameOfDiocese, updateRequest.NameOfDiocese);
            entity.FaithOfDiocese = updateRequest.FaithOfDiocese != null
                ? ProjectMapper.ToDioceseFaithType(updateRequest.FaithOfDiocese.Value)
                : entity.FaithOfDiocese;
        }

        private static void ApplyOtherReligiousOrganisation(Proposal entity, UpdateProposalRequest updateRequest)
        {
            entity.NameOfOtherReligiousOrganisation =
                Replace(entity.NameOfOtherReligiousOrganisation, updateRequest.NameOfOtherReligiousOrganisation);
            entity.FaithTypeOfOtherReligiousOrganisation = updateRequest.FaithTypeOfOtherReligiousOrganisation != null
                ? ProjectMapper.ToFaithType(updateRequest.FaithTypeOfOtherReligiousOrganisation.Value)
                : entity.FaithTypeOfOtherReligiousOrganisation;

            // The free text belongs to the "Other" option, so any other faith clears it.
            entity.OtherFaithTypeOfOtherReligiousOrganisation =
                updateRequest.FaithTypeOfOtherReligiousOrganisation == FaithType.Other
                    ? updateRequest.OtherFaithTypeOfOtherReligiousOrganisation
                    : null;
        }

        private static void ApplyOtherLocalAuthority(Proposal entity, UpdateProposalRequest updateRequest)
        {
            entity.OtherLocalAuthorityRegion = updateRequest.OtherLocalAuthorityRegion != null
                ? updateRequest.OtherLocalAuthorityRegion.ToDescription()
                : entity.OtherLocalAuthorityRegion;
            entity.OtherLocalAuthority = Replace(entity.OtherLocalAuthority, updateRequest.OtherLocalAuthority);
        }

        private static void ApplyJointProposal(Proposal entity, UpdateProposalRequest updateRequest)
        {
            entity.JointProposalLocalAuthorityRegion = updateRequest.JointProposalLocalAuthorityRegion != null
                ? updateRequest.JointProposalLocalAuthorityRegion.ToDescription()
                : entity.JointProposalLocalAuthorityRegion;
            entity.JointProposalLocalAuthority =
                Replace(entity.JointProposalLocalAuthority, updateRequest.JointProposalLocalAuthority);
        }

        private static void ApplyFaithAnswers(Proposal entity, UpdateProposalRequest updateRequest)
        {
            entity.ProposedFaithStatus = updateRequest.ProposedFaithStatus != null
                ? ProjectMapper.ToFaithStatus(updateRequest.ProposedFaithStatus.Value)
                : entity.ProposedFaithStatus;
            entity.ProposedFaithType = updateRequest.ProposedFaithType != null
                ? ProjectMapper.ToFaithType(updateRequest.ProposedFaithType.Value)
                : entity.ProposedFaithType;
            entity.OtherFaithType = updateRequest.OtherFaithType ?? entity.OtherFaithType;
        }

        private static string Replace(string current, string replacement) =>
            string.IsNullOrEmpty(replacement) ? current : replacement;
    }
}
