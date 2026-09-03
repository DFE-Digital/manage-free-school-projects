using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class JointProposalRegionModel(
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

            SetBackLink();

            var proposal = await getProposalService.ExecuteSingle(Rid);

            if (proposal == null || proposal.Data == null)
            {
                return NotFound();
            }

            Proposal = proposal.Data;

            var region = GetProjectRegion(Proposal.JointProposalLocalAuthorityRegion);

            Region = region.ToString();

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

            return Redirect(string.Format(RouteConstants.Proposals_Edit_Joint_Proposal_Local_Authority, ProjectId, Rid, region));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Details, ProjectId, Rid);
        }

        public static ProjectRegion GetProjectRegion(string value)
        {
            foreach (ProjectRegion region in Enum.GetValues<ProjectRegion>())
            {
                var field = typeof(ProjectRegion).GetField(region.ToString());

                var description = field?
                    .GetCustomAttribute<DescriptionAttribute>()?
                    .Description;

                if (description == value)
                    return region;
            }

            throw new ArgumentException($"Unknown ProjectRegion: {value}");
        }
    }
}
