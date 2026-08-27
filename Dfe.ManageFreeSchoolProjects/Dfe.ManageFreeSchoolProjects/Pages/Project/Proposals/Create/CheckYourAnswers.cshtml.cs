using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Dfe.ManageFreeSchoolProjects.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class CheckYourAnswersModel(
        ICreateProposalCache createProposalCache,
        ICreateProposalService createProposalService,
        ILogger<CheckYourAnswersModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        public CreateProposalCacheItem Cache { get; set; }

        public IActionResult OnGet()
        {
            Cache = CreateProposalCache.Get();

            Cache.ReachedCheckYourAnswers = true;
            CreateProposalCache.Update(Cache);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Cache = CreateProposalCache.Get();

            var createRequest = new CreateProposalRequest
            {
                ProjectId = ProjectId,
                Proposer = Cache.Proposer.Value,

                // Academy Trust route - ProposalProposer.AcademyTrust
                TrustReferenceNumber = Cache.Trust?.TRN,
                TrustName = Cache.Trust?.TrustName,
                TrustType = Cache.Trust?.TrustType,

                // Diocese - ProposalProposer.Diocese
                NameOfDiocese = Cache.NameOfDiocese,
                FaithOfDiocese = Cache.FaithOfDiocese,

                // Another religious organisation - ProposalProposer.AnotherReligiousOrganisation
                NameOfOtherReligiousOrganisation = Cache.NameOfOtherReligiousOrganisation,
                FaithTypeOfOtherReligiousOrganisation = Cache.FaithTypeOfOtherReligiousOrganisation,
                OtherFaithTypeOfOtherReligiousOrganisation = Cache.OtherFaithTypeOfOtherReligiousOrganisation,

                // Another local authority - ProposalProposer.AnotherLocalAuthority
                OtherLocalAuthorityRegion = Cache.OtherLocalAuthorityRegion,
                OtherLocalAuthority = Cache.OtherLocalAuthority,

                // Joint proposal between the local authority that published the specification and another local authority - ProposalProposer.JointProposal
                JointProposalLocalAuthorityRegion = Cache.JointProposalLocalAuthorityRegion,
                JointProposalLocalAuthority = Cache.JointProposalLocalAuthority,

                //Proposed Faith (Common)
                ProposedFaithStatus = Cache.ProposedFaithStatus,
                ProposedFaithType = Cache.ProposedFaithType,
                OtherFaithType = Cache.OtherFaithType   
            };

            try
            {
                var response = await createProposalService.Execute(createRequest);
            }
            catch (HttpRequestException e)
            {
                errorService.AddError("projectid", "Error occurred while creating proposal.");

                return Page();
            }

            return Redirect(string.Format(RouteConstants.Proposals, ProjectId));
        }
    }
}
