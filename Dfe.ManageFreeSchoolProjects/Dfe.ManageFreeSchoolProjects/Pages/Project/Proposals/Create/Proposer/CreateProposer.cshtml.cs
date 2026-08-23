using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals.Enums;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create.Proposer
{
    public class CreateProposerModel(
        ICreateProposalCache createProposalCache,
        ILogger<CreateProposerModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "proposer")]
        [Display(Name = "proposer")]
        [Required(ErrorMessage = "Select the proposer")]
        public ProposalProposer? Proposer { get; set; }

        [FromQuery(Name = "newProposal")]
        public bool? IsNewProposal { get; set; }

        public IActionResult OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            if (IsNewProposal != null && (bool)IsNewProposal)
            {
                CreateProposalCache.Delete();
            }
            else
            {
                Proposer = CreateProposalCache.Get().Proposer;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            logger.LogMethodEntered();

            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            // update cache
            var cache = CreateProposalCache.Get();

            cache.Proposer = Proposer;
            CreateProposalCache.Update(cache);

            if (Proposer == ProposalProposer.AcademyTrust)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_SearchTrustByTRN, ProjectId));
            }
            else if (Proposer == ProposalProposer.LocalAuthority)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_Faith_Status, ProjectId));
            }
            else if (Proposer == ProposalProposer.AnotherLocalAuthority)
            {
                //return Redirect(string.Format(RouteConstants.Proposals_Create_Faith_Status, ProjectId));
            }

            return Page();
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals, ProjectId);
        }
    }
}
