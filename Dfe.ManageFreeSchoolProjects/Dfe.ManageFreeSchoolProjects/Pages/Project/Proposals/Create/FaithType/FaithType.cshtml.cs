using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create.FaithType
{
    public class FaithTypeModel(
        ICreateProposalCache createProposalCache,
        ILogger<FaithTypeModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "faith-type")]
        public API.Contracts.Project.Tasks.FaithType FaithType { get; set; }

        [BindProperty(Name = "other-faith-type")]
        [Display(Name = "Other faith type")]
        public string OtherFaithType { get; set; }

        public IActionResult OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            FaithType = CreateProposalCache.Get().FaithType;

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

            cache.FaithType = FaithType;

            if (FaithType == API.Contracts.Project.Tasks.FaithType.Other && !string.IsNullOrWhiteSpace(OtherFaithType))
            {
                cache.OtherFaithType = OtherFaithType;
            }

            CreateProposalCache.Update(cache);

            return Redirect(string.Format(RouteConstants.Proposals_Create_Check_Answers, ProjectId));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Create_Faith_Status, ProjectId);
        }
    }
}
