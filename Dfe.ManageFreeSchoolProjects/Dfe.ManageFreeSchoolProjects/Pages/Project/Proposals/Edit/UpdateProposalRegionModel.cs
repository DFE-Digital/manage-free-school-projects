using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Extensions;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public abstract class UpdateProposalRegionModel(
        IGetProposalService getProposalService,
        ILogger logger,
        ErrorService errorService) : UpdateProposalBaseModel(getProposalService, logger)
    {
        [BindProperty(Name = "region")]
        [Display(Name = "region")]
        [Required(ErrorMessage = "Select the region")]
        public string Region { get; set; }

        protected abstract string GetSelectedRegion(ProposalResponse proposal);

        protected abstract string LocalAuthorityRoute { get; }

        public async Task<IActionResult> OnGet()
        {
            LogPageEntered();

            SetBackLink();

            if (await LoadProposal() == null)
            {
                return NotFound();
            }

            var selectedRegion = GetSelectedRegion(Proposal);

            if (!string.IsNullOrEmpty(selectedRegion))
            {
                Region = selectedRegion.FromDescription<ProjectRegion>().ToString();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            LogPageEntered();

            SetBackLink();

            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);

                return Page();
            }

            var region = (int)Enum.Parse<ProjectRegion>(Region);

            return Redirect(string.Format(LocalAuthorityRoute, ProjectId, Rid, region));
        }
    }
}
