using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class FaithOfDioceseModel(
        ICreateProposalCache createProposalCache,
        ILogger<FaithOfDioceseModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "faith-of-diocese")]
        [Display(Name = "faith-of-diocese")]
        [Required(ErrorMessage = "Select the faith of the diocese")]
        public FaithOfDiocese? FaithOfDiocese { get; set; }


        public IActionResult OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            FaithOfDiocese = CreateProposalCache.Get().FaithOfDiocese;

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

            cache.FaithOfDiocese = FaithOfDiocese.Value;
            CreateProposalCache.Update(cache);

            return Redirect(string.Format(RouteConstants.Proposals_Create_Proposed_Faith_Status, ProjectId));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Create_Name_Of_Diocese, ProjectId);
        }
    }
}
