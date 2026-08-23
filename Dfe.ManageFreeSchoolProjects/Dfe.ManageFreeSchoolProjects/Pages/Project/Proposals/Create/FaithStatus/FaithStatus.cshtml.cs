using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create.FaithStatus
{
    public class FaithStatusModel(
        ICreateProposalCache createProposalCache,
        ILogger<FaithStatusModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "faith-status")]
        [Required(ErrorMessage = "Select the faith status")]
        public API.Contracts.Project.Tasks.FaithStatus Status { get; set; }

        public IActionResult OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            Status = CreateProposalCache.Get().FaithStatus;

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

            cache.FaithStatus = Status;
            CreateProposalCache.Update(cache);

            if (Status == API.Contracts.Project.Tasks.FaithStatus.None)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_Check_Answers, ProjectId));
            }
            else
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_Faith_Type, ProjectId));
            }
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Create_Confirm_Trust, ProjectId);
        }
    }
}
