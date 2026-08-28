using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class JointProposalRegionModel(
        ICreateProposalCache createProposalCache,
        ILogger<JointProposalRegionModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "region")]
        [Display(Name = "region")]
        [Required(ErrorMessage = "Select the region")]
        public string Region { get; set; }

        public IActionResult OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            Region = CreateProposalCache.Get().JointProposalLocalAuthorityRegion.ToString();

            return Page();
        }

        public IActionResult OnPost()
        {
            logger.LogMethodEntered();

            SetBackLink();

            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            // update cache
            var cache = CreateProposalCache.Get();

            cache.JointProposalLocalAuthorityRegion = Enum.Parse<ProjectRegion>(Region);
            CreateProposalCache.Update(cache);

            return Redirect(string.Format(RouteConstants.Proposals_Create_Joint_Proposal_Local_Authority, ProjectId));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Create_Proposer, ProjectId);
        }
    }
}
