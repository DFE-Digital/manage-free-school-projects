using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
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
    public class NameOfOtherReligiousOrganisationModel(
        IGetProposalService getProposalService,
        IUpdateProposalService updateProposalService,
        ILogger<NameOfOtherReligiousOrganisationModel> logger,
        ErrorService errorService
    ) : UpdateProposalBaseModel
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(SupportsGet = true, Name = "rid")]
        public string Rid { get; set; }

        public ProposalResponse Proposal { get; set; }

        [BindProperty(Name = "name-of-other-religious-organisation")]
        [Display(Name = "Name of the other religious organisation")]
        [Required(ErrorMessage = "Enter the name of the other religious organisation")]
        public string NameOfOtherReligiousOrganisation { get; set; }

        public async Task<IActionResult> OnGet()
        {
            logger.LogMethodEntered();

            var proposal = await getProposalService.ExecuteSingle(Rid);

            if (proposal == null || proposal.Data == null)
            {
                return NotFound();
            }

            Proposal = proposal.Data;

            NameOfOtherReligiousOrganisation = Proposal.NameOfOtherReligiousOrganisation;

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
                NameOfOtherReligiousOrganisation = NameOfOtherReligiousOrganisation
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
