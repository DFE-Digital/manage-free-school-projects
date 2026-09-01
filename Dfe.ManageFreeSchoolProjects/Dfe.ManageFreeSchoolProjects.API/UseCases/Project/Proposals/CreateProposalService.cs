using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals
{
    public interface ICreateProposalService
    {
        Task<CreateProposalResponse> Execute(CreateProposalRequest createRequest);
    }

    public class CreateProposalService : ICreateProposalService
    {
        private readonly MfspContext _context;

        public CreateProposalService(MfspContext context)
        {
            _context = context;
        }

        public async Task<CreateProposalResponse> Execute(CreateProposalRequest createRequest)
        {
            var rid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 11);

            var entity = new Proposal
            {
                Rid = rid,
                ProjectId = createRequest.ProjectId,
                Proposer = ProjectMapper.ToProposer(createRequest.Proposer),

                // Academy Trust route - ProposalProposer.AcademyTrust
                TrustReferenceNumber = createRequest.TrustReferenceNumber,
                TrustName = createRequest.TrustName,
                TrustType = createRequest.TrustType != null ? ProjectMapper.ToTrustType(createRequest.TrustType.Value) : null,

                // Diocese - ProposalProposer.Diocese
                NameOfDiocese = createRequest.NameOfDiocese,
                FaithOfDiocese = createRequest.FaithOfDiocese != null ? ProjectMapper.ToDioceseFaithType(createRequest.FaithOfDiocese.Value) : null,

                // Another religious organisation - ProposalProposer.AnotherReligiousOrganisation
                NameOfOtherReligiousOrganisation = createRequest.NameOfOtherReligiousOrganisation,
                FaithTypeOfOtherReligiousOrganisation = createRequest.FaithTypeOfOtherReligiousOrganisation != null ? ProjectMapper.ToFaithType(createRequest.FaithTypeOfOtherReligiousOrganisation.Value) : null,
                OtherFaithTypeOfOtherReligiousOrganisation = createRequest.OtherFaithTypeOfOtherReligiousOrganisation,

                // Another local authority - ProposalProposer.AnotherLocalAuthority
                OtherLocalAuthority = createRequest.OtherLocalAuthority,

                // Joint proposal between the local authority that published the specification and another local authority - ProposalProposer.JointProposal
                JointProposalLocalAuthority = createRequest.JointProposalLocalAuthority,

                //Proposed Faith (Common)
                ProposedFaithStatus = ProjectMapper.ToFaithStatus(createRequest.ProposedFaithStatus),
                ProposedFaithType = ProjectMapper.ToFaithType(createRequest.ProposedFaithType),
                OtherFaithType = !string.IsNullOrWhiteSpace(createRequest.OtherFaithType) ? createRequest.OtherFaithType : null
            };

            _context.Proposals.Add(entity);
            await _context.SaveChangesAsync();

            var result = new CreateProposalResponse
            {
                Rid = entity.Rid,
                ProjectId = entity.ProjectId,
                Proposer = ProjectMapper.ToProposer(entity.Proposer),
                TrustReferenceNumber = entity.TrustReferenceNumber,
                TrustName = entity.TrustName,
                TrustType = entity.TrustType != null ? ProjectMapper.ToTrustType(entity.TrustType) : null,
                NameOfDiocese = entity.NameOfDiocese,
                FaithOfDiocese = entity.FaithOfDiocese != null ? ProjectMapper.ToDioceseFaithType(entity.FaithOfDiocese) : null,
                NameOfOtherReligiousOrganisation = entity.NameOfOtherReligiousOrganisation,
                FaithTypeOfOtherReligiousOrganisation = entity.FaithTypeOfOtherReligiousOrganisation != null ? ProjectMapper.ToFaithType(entity.FaithTypeOfOtherReligiousOrganisation) : null,
                OtherFaithTypeOfOtherReligiousOrganisation = entity.OtherFaithTypeOfOtherReligiousOrganisation,
                OtherLocalAuthority = entity.OtherLocalAuthority,
                JointProposalLocalAuthority = entity.JointProposalLocalAuthority,
                ProposedFaithStatus = ProjectMapper.ToFaithStatus(entity.ProposedFaithStatus),
                ProposedFaithType = ProjectMapper.ToFaithType(entity.ProposedFaithType),
                OtherFaithType = entity.OtherFaithType
            };

            return result;
        }
    }
}
