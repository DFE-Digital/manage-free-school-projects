using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class FaithOfOtherReligiousOrganisationModel(
        IGetProposalService getProposalService,
        IUpdateProposalService updateProposalService,
        ILogger<FaithOfOtherReligiousOrganisationModel> logger,
        ErrorService errorService
    ) : UpdateProposalBaseModel
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(SupportsGet = true, Name = "rid")]
        public string Rid { get; set; }

        public ProposalResponse Proposal { get; set; }

        [BindProperty(Name = "faith-type")]
        [Display(Name = "Faith of the other religious organisation")]
        [Required(ErrorMessage = "Select the faith of the other religious organisation")]
        public FaithType? FaithTypeOfOtherReligiousOrganisation { get; set; }

        [BindProperty(Name = "other-faith-type")]
        [Display(Name = "Other faith of the other religious organisation")]
        public string OtherFaithType { get; set; }

        public async Task<IActionResult> OnGet()
        {
            logger.LogMethodEntered();

            var proposal = await getProposalService.ExecuteSingle(Rid);

            if (proposal == null || proposal.Data == null)
            {
                return NotFound();
            }

            Proposal = proposal.Data;

            FaithTypeOfOtherReligiousOrganisation = Proposal.FaithTypeOfOtherReligiousOrganisation;
            OtherFaithType = Proposal.OtherFaithTypeOfOtherReligiousOrganisation;

            SetBackLink();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            logger.LogMethodEntered();

            SetBackLink();

            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            var proposal = await getProposalService.ExecuteSingle(Rid);
            Proposal = proposal.Data;

            var updateRequest = new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = Proposal.Proposer,
                FaithTypeOfOtherReligiousOrganisation = FaithTypeOfOtherReligiousOrganisation,
                OtherFaithTypeOfOtherReligiousOrganisation = FaithTypeOfOtherReligiousOrganisation == FaithType.Other ? OtherFaithType : null
            };

            await updateProposalService.Execute(updateRequest);

            return Redirect(string.Format(RouteConstants.Proposals_Details, ProjectId, Rid));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Details, ProjectId, Rid);
        }
    }
}
