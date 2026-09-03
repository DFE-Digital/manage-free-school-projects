using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class OtherLocalAuthorityRegionModel(
        IGetProposalService getProposalService,
        ILogger<OtherLocalAuthorityRegionModel> logger,
        ErrorService errorService
    ) : UpdateProposalBaseModel
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(SupportsGet = true, Name = "rid")]
        public string Rid { get; set; }

        public ProposalResponse Proposal { get; set; }

        [BindProperty(Name = "region")]
        [Display(Name = "region")]
        [Required(ErrorMessage = "Select the region")]
        public string Region { get; set; }

        public async Task<IActionResult> OnGet()
        {
            logger.LogMethodEntered();

            var proposal = await getProposalService.ExecuteSingle(Rid);

            if (proposal == null || proposal.Data == null)
            {
                return NotFound();
            }

            Proposal = proposal.Data;

            Region = Proposal.OtherLocalAuthorityRegion;

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

            var region = (int)Enum.Parse<ProjectRegion>(Region);

            return Redirect(string.Format(RouteConstants.Proposals_Edit_Other_Local_Authority, ProjectId, Rid, region));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Details, ProjectId, Rid);
        }
    }
}
